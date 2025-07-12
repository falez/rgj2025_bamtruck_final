using DG.Tweening;
using PDollarGestureRecognizer;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class GestureRecog : MonoBehaviour
{
    [SerializeField] bool allowRecognition = true;

    [Range(0.5f, 1.0f)]
    [SerializeField] float minimumConfidence = 0.8f;

    public bool AllowRecognition { get => allowRecognition; set => allowRecognition = value; }

    private Vector3 virtualKeyPosition;
    private Rect drawArea;

    public delegate void OnGestureEvent(string gestureName, float confidence);

    public event OnGestureEvent onGestureMade;

    private void Start()
    {
        drawArea = new Rect(0, 0, Screen.width, Screen.height * 0.5f);

        TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/");
        foreach (TextAsset gestureXml in gesturesXml)
            trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    private List<Gesture> trainingSet = new List<Gesture>();

    private List<Point> points = new List<Point>();
    private int strokeId = -1;

    private bool recognized;

    private List<LineRenderer> gestureLinesRenderer = new List<LineRenderer>();
    private LineRenderer currentGestureLineRenderer;
    public Transform lineRendererPrefab;
    private int vertexCount = 0;

    Sequence seq;

    bool shouldTrack = false;

    void Update()
    {
        if (!allowRecognition) return;
        if (Touch.activeTouches.Count == 0) return;

        Touch touch = Touch.activeTouches[0];
        Vector2 screenPos = Touch.activeTouches[0].screenPosition;
        virtualKeyPosition = new Vector3(screenPos.x, screenPos.y);

        if (drawArea.Contains(virtualKeyPosition))
        {
            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                if (recognized)
                {
                    recognized = false;
                    strokeId = -1;

                    points.Clear();

                    foreach (LineRenderer lineRenderer in gestureLinesRenderer)
                    {
                        lineRenderer.positionCount = 0;
                        Destroy(lineRenderer.gameObject);
                    }

                    gestureLinesRenderer.Clear();
                }

                ++strokeId;

                Transform tmpGesture = Instantiate(lineRendererPrefab, transform.position, transform.rotation) as Transform;
                currentGestureLineRenderer = tmpGesture.GetComponent<LineRenderer>();
                gestureLinesRenderer.Add(currentGestureLineRenderer);
                vertexCount = 0;

                shouldTrack = true;
            }

        }

        if (!shouldTrack)
            return;

        if (touch.phase == UnityEngine.InputSystem.TouchPhase.Moved)
        {
            points.Add(new Point(virtualKeyPosition.x, -virtualKeyPosition.y, strokeId));

            currentGestureLineRenderer.positionCount = ++vertexCount;
            currentGestureLineRenderer.SetPosition(vertexCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 10)));
        }
        else if (touch.ended)
        {
            recognized = true;
            shouldTrack = false;

            Gesture candidate = new Gesture(points.ToArray());
            Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());

            Debug.Log($"Player gestured {gestureResult.GestureClass} ({gestureResult.Score})");

            if (gestureResult.Score > minimumConfidence)
            {
                onGestureMade?.Invoke(gestureResult.GestureClass, gestureResult.Score);
            }

            ClearLinesAfter(0.25f);
        }
    }

    void ClearLinesAfter(float delay)
    {
        DOTween.Kill(seq, true);

        seq = DOTween.Sequence();
        seq.AppendInterval(delay);
        seq.OnComplete(ClearLines);
        seq.Play();
    }

    void ClearLines()
    {
        strokeId = -1;

        points.Clear();

        foreach (LineRenderer lineRenderer in gestureLinesRenderer)
        {
            lineRenderer.positionCount = 0;
            Destroy(lineRenderer.gameObject);
        }

        gestureLinesRenderer.Clear();
    }
}
