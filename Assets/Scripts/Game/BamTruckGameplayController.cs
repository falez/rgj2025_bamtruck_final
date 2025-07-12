using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BamTruckGameplayController : MonoBehaviour
{
    public float duration = 60.0f;

    public List<FoodOrder> foods;
    private List<string> gestures = new() { "Triangle", "Square", "S", "RightArrow" };

    private float timeLeft;

    private GameCustomer currentCustomer;

    private int life = 3;

    private int score = 0;

    public int Life => life;

    public int Score => score;
    public float TimeLeft => timeLeft;

    public bool Pause { get => paused; set => paused = value; }

    public delegate void CustomerChangeEvent(GameCustomer next);

    public delegate void CustomerOrderCorrectEvent(int index);

    public delegate void RoundCompleteEvent(bool success, int score);

    public delegate void GameSignalEvent();

    public event CustomerOrderCorrectEvent OnCustomerOrderCorrect;

    public event RoundCompleteEvent OnRoundComplete;

    public event GameSignalEvent OnGameStarted;

    public event GameSignalEvent OnGameEnded;

    public event CustomerChangeEvent OnCustomerChanged;

    private readonly Dictionary<string, FoodOrder> gestureFoodMap = new();

    private bool gameStarted = false;
    private bool paused = false;
    private int correct = 0;

    public void StartGame()
    {
        life = 3;
        score = 0;
        timeLeft = duration;
        gameStarted = true;
        paused = false;
        correct = 0;

        gestureFoodMap.Clear();

        OnGameStarted?.Invoke();

        for (int i = 0; i < gestures.Count; i++)
        {
            gestureFoodMap.Add(gestures[i], foods[i]);
        }

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

    private FoodOrder GestureToOrder(string gestureClass)
    {
        gestureFoodMap.TryGetValue(gestureClass, out FoodOrder value);
        return value;
    }

    public void Answer(string gesture)
    {
        FoodOrder match = GestureToOrder(gesture);
        (bool success, int index) = currentCustomer.TryMatchOrder(match.name);

        Debug.Log($"{gesture} {match.name}");
        if (!success)
        {
            Debug.Log($"Wrong Order! Customer unhappy!");
            OnRoundComplete?.Invoke(false, 0);

            bool ended = DecreaseHealth();

            if (ended)
            {
                GameOver();
            }
            else
            {
                Sequence seq = DOTween.Sequence();
                seq.AppendInterval(0.6f);
                seq.OnComplete(() =>
                {
                    SwitchCustomer();
                });
                seq.Play();
            }

            return;
        }
        else
        {
            OnCustomerOrderCorrect?.Invoke(index);
        }

        if (currentCustomer.CompletedOrder())
        {
            RewardPointsThenNextCustomer();
        }
    }

    private void RewardPointsThenNextCustomer()
    {
        Debug.Log("All order met! Custommer happy!");
        score += currentCustomer.Score;
        correct++;

        OnRoundComplete?.Invoke(true, currentCustomer.Score);

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.6f);
        seq.OnComplete(() =>
        {
            SwitchCustomer();
        });

        seq.Play();
        // Some Delay needed
    }

    private void SwitchCustomer()
    {
        var oldCustomer = currentCustomer;

        int diff = 1;

        if (correct > 5) diff++;
        if (correct > 10) diff++;

        currentCustomer = GetNext(diff);

        OnCustomerChanged?.Invoke(currentCustomer);

        // Go out current if null
        if (oldCustomer != null)
            Debug.Log("Customer leaving...");
        // make new customer
        // come in
        Debug.Log("Next customer entering...");
        CustomerReady();
    }

    private void CustomerReady()
    {
        Debug.Log($"Customer asking for {string.Join(",", currentCustomer.Orders)}");
    }

    private void GameOver()
    {
        gameStarted = false;
        OnGameEnded?.Invoke();
        Debug.Log("Game Over!");
    }

    private bool DecreaseHealth()
    {
        life--;
        return life <= 0;
    }

    private GameCustomer GetNext(int diff)
    {
        GameCustomer cust = new GameCustomer();

        for (int i = 0; i < diff; i++)
        {
            int ran = Random.Range(0, gestures.Count);
            string gestureName = gestures[ran];

            cust.Orders.Add(new FoodOrderInstance(gestureFoodMap[gestureName]));
        }

        cust.Apply();

        return cust;
    }
}