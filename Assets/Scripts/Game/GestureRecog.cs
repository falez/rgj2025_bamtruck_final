using DG.Tweening;
using PDollarGestureRecognizer;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class GestureRecog : MonoBehaviour
{
    [SerializeField] private bool allowRecognition = true;

    [Range(0.5f, 1.0f)]
    [SerializeField] private float minimumConfidence = 0.8f;

    public bool AllowRecognition { get => allowRecognition; set => ChangeAllowRecognition(value); }

    private Vector3 virtualKeyPosition;
    private Rect drawArea;

    public delegate void OnGestureEvent(string gestureName, float confidence);

    public event OnGestureEvent onGestureMade;

    private InputAction pressAction;
    private InputAction moveAction;

    private List<Gesture> trainingSet = new List<Gesture>();

    private List<Point> points = new List<Point>();
    private int strokeId = -1;

    private TrailRenderer trailRenderer;
    public Transform lineRendererPrefab;

    private bool shouldTrack = false;

    private void Start()
    {
        drawArea = new Rect(0, 0, Screen.width, Screen.height * 0.5f);

        TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/");
        foreach (TextAsset gestureXml in gesturesXml)
            trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));

        pressAction = InputSystem.actions.FindAction("Click");
        moveAction = InputSystem.actions.FindAction("Point");

        Transform tmpGesture = Instantiate(lineRendererPrefab) as Transform;
        trailRenderer = tmpGesture.GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        if (!allowRecognition) return;

        var pt = moveAction.ReadValue<Vector2>();
        Vector2 screenPos = pt;
        virtualKeyPosition = new Vector3(screenPos.x, screenPos.y);

        if (drawArea.Contains(virtualKeyPosition))
        {
            if (pressAction.WasPressedThisFrame())
            {
                points.Clear();

                ++strokeId;

                trailRenderer.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 5));
                trailRenderer.Clear();

                shouldTrack = true;
            }
        }

        if (!shouldTrack)
            return;

        if (pressAction.IsPressed())
        {
            points.Add(new Point(virtualKeyPosition.x, -virtualKeyPosition.y, strokeId));

            trailRenderer.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 5));
        }

        if (pressAction.WasReleasedThisFrame())
        {
            shouldTrack = false;
            strokeId = -1;

            if (points.Count > 0)
            {
                Gesture candidate = new(points.ToArray());
                Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());

                Debug.Log($"Player gestured {gestureResult.GestureClass} ({gestureResult.Score})");

                if (gestureResult.Score > minimumConfidence)
                {
                    onGestureMade?.Invoke(gestureResult.GestureClass, gestureResult.Score);
                }
            }
        }
    }

    private void ChangeAllowRecognition(bool enable)
    {
        allowRecognition = enable;

        shouldTrack = false;

        if (!enable)
        {
            trailRenderer.Clear();
        }
    }
}