using UnityEngine;

public class BamTruckGameplayController : MonoBehaviour
{
    private float internalTimer = 10.0f;

    private GameCustomer currentCustomer;

    private int life = 3;

    private int score = 0;

    public int Life => life;

    public int Score => score;

    public delegate void CustomerChangeEvent(GameCustomer next);

    public delegate void RoundCompleteEvent(bool success, int score);

    public event RoundCompleteEvent OnRoundComplete;

    public event CustomerChangeEvent OnCustomerChanged;

    public void StartGame()
    {
        SwitchCustomer();
    }

    public void Answer(string order)
    {
        bool success = currentCustomer.TryMatchOrder(order);

        if (!success)
        {
            Debug.Log($"Wrong Order! Customer unhappy!");
            OnRoundComplete?.Invoke(false, 0);

            bool ended = DecreaseHealth();

            if (ended) { GameOver(); }
            else { SwitchCustomer(); }

            return;
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

        OnRoundComplete?.Invoke(true, currentCustomer.Score);

        // Some Delay needed

        SwitchCustomer();
    }

    private void SwitchCustomer()
    {
        var oldCustomer = currentCustomer;
        currentCustomer = GetNext(1);

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
            cust.Orders.Add("Triangle");
        }

        cust.Apply();

        return cust;
    }
}