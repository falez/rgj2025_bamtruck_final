using System.Collections;
using System.Collections.Generic;

public class GameCustomer
{
    public List<FoodOrderInstance> Orders { get; } = new();

    public int Score { get; private set; }

    private int correctCount;

    public void Apply()
    {
        Score = Orders.Count * 100;
        correctCount = 0;
    }

    public (bool, int) TryMatchOrder(string order)
    {
        int index = Orders.FindIndex((x) => x.name.Equals(order) && !x.met);

        if (index < 0)
            return (false, -1);

        Orders[index].met = true;

        correctCount++;
        return (true, index);
    }

    public bool CompletedOrder()
    {
        return correctCount == Orders.Count;
    }
}