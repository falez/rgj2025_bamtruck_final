using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public struct FoodLineUp
{
    [SerializeField] private IFoodOrder food;
    [SerializeField] private GameGesture gesture;

    public IFoodOrder Food => food;
    public GameGesture Gesture => gesture;

    public FoodLineUp(IFoodOrder food, GameGesture gesture)
    {
        this.food = food;
        this.gesture = gesture;
    }
}

public class BamTruckGameplayController : MonoBehaviour
{
    private const int DIFF2 = 3;
    private const int DIFF3 = 6;
    private const int DIFF4 = 9;
    private const int MINFORJUMBLE = 4;

    [SerializeField] private float duration = 60.0f;

    [SerializeField] private List<FoodOrderSO> foods;

    [SerializeField] private List<GameGesture> gestures;

    private float invDuration = 1.0f;
    private float timeLeft = 1.0f;

    private GameCustomer currentCustomer;

    private int score = 0;

    public int Score => score;
    public float TimeLeft => timeLeft;
    public float NormalizedDuration => timeLeft * invDuration;

    public bool Pause { get => paused; set => paused = value; }

    public delegate void CustomerChangeEvent(GameCustomer next);

    public delegate void CustomerOrderCorrectEvent(int index);

    public delegate void RoundCompleteEvent(bool success, int score, int totalScore);

    public delegate void GameSignalEvent();

    public delegate void GameSignalEvent2(int finalScore);

    public delegate void FoodLineUpChangeEvent(List<FoodLineUp> list, bool first);

    public event CustomerOrderCorrectEvent OnCustomerOrderCorrect;

    public event CustomerOrderCorrectEvent OnCustomerOrderWrong;

    public event RoundCompleteEvent OnRoundComplete;

    public event GameSignalEvent OnGameStarted;

    public event GameSignalEvent2 OnGameEnded;

    public event CustomerChangeEvent OnCustomerChanged;

    public event FoodLineUpChangeEvent OnFoodLineUpChanged;

    private readonly Dictionary<string, IFoodOrder> gestureFoodMap = new();

    private bool gameStarted = false;
    private bool paused = false;
    private int correct = 0;

    private List<FoodLineUp> foodLineUp = new();

    private bool enableJumbling = false;
    private int gestureMadeSinceLastJumble = 0;

    private void Awake()
    {
        invDuration = 1.0f / duration;
    }

    public void EndGame()
    {
        timeLeft = 0.0f;
    }

    public void InitializeValues()
    {
        //life = 3;
        score = 0;
        timeLeft = duration;
        gameStarted = false;
        paused = false;
        correct = 0;
        enableJumbling = false;
        gestureMadeSinceLastJumble = 0;

        gestureFoodMap.Clear();
        foodLineUp.Clear();

        for (int i = 0; i < gestures.Count; i++)
        {
            gestureFoodMap.Add(gestures[i].GestureClass, foods[i]);
            foodLineUp.Add(new FoodLineUp(foods[i], gestures[i]));
        }

        OnFoodLineUpChanged?.Invoke(foodLineUp, true);
    }

    private void JumbleFood()
    {
        gestureFoodMap.Clear();
        foodLineUp.Clear();

        List<GameGesture> copy = new(gestures);

        copy.Shuffle();

        for (int i = 0; i < copy.Count; i++)
        {
            gestureFoodMap.Add(copy[i].GestureClass, foods[i]);
            foodLineUp.Add(new FoodLineUp(foods[i], copy[i]));
        }

        OnFoodLineUpChanged?.Invoke(foodLineUp, false);
    }

    public void StartGame()
    {
        gameStarted = true;
        paused = false;

        OnGameStarted?.Invoke();

        SwitchCustomer();
    }

    private void Update()
    {
        if (!gameStarted) return;
        if (paused) return;

        timeLeft -= Time.deltaTime;

        if (timeLeft < 0.0f)
        {
            timeLeft = 0.0f;
            GameOver();
        }
    }

    private IFoodOrder GetFoodOrderFromGesture(string gestureClass)
    {
        gestureFoodMap.TryGetValue(gestureClass, out IFoodOrder value);
        return value;
    }

    public void Answer(string gesture)
    {
        IFoodOrder match = GetFoodOrderFromGesture(gesture);
        (bool success, int index) = currentCustomer.TryMatchOrder(match.Id);

        Debug.Log($"{gesture} {match.Id}");
        if (!success)
        {
            Debug.Log($"Wrong Order! Customer unhappy!");
            score = Mathf.Max(score - 150, 0);
            OnCustomerOrderWrong?.Invoke(index);

            return;
        }
        else
        {
            OnCustomerOrderCorrect?.Invoke(index);
            ValidateJumble();
        }

        if (currentCustomer.CompletedOrder())
        {
            RewardPointsThenNextCustomer();
        }
    }

    private void ValidateJumble()
    {
        if (!enableJumbling) return;

        gestureMadeSinceLastJumble++;

        if (gestureMadeSinceLastJumble > MINFORJUMBLE)
        {
            float r = Random.Range(0.0f, 1.0f);
            if (r > 0.75f)
            {
                JumbleFood();
                gestureMadeSinceLastJumble = 0;
            }
        }
    }

    private void RewardPointsThenNextCustomer()
    {
        Debug.Log("All order met! Custommer happy!");
        int scr = 100 * currentCustomer.OrderCount;
        score += scr;
        correct++;

        if (correct >= DIFF4)
        {
            enableJumbling = true;
        }

        OnRoundComplete?.Invoke(true, scr, score);

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.6f);
        seq.OnComplete(() => { SwitchCustomer(); });
        seq.Play();
    }

    private void SwitchCustomer()
    {
        var oldCustomer = currentCustomer;

        int diff = 1;
        if (correct >= DIFF2) diff++;
        if (correct >= DIFF3) diff++;

        currentCustomer = GetNext(diff);

        OnCustomerChanged?.Invoke(currentCustomer);
    }

    private void GameOver()
    {
        gameStarted = false;
        OnGameEnded?.Invoke(score);
        Debug.Log("Game Over!");
    }

    private GameCustomer GetNext(int diff)
    {
        GameCustomer cust = new GameCustomer();

        for (int i = 0; i < diff; i++)
        {
            // Should change to randomizing from foodLineUp

            int ran = Random.Range(0, gestures.Count);
            string gestureName = gestures[ran].GestureClass;

            cust.Orders.Add(new FoodOrderInstance(gestureFoodMap[gestureName]));
        }

        return cust;
    }
}