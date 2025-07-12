using System.Collections;
using System.Collections.Generic;

public class GameCustomer
{
    public List<string> Orders { get; } = new();

    public int Score { get; private set; }

    public void Apply()
    {
        Score = Orders.Count * 100;
    }

    public bool TryMatchOrder(string order)
    {
        int index = Orders.FindIndex((x) => x.Equals(order));

        if (index < 0)
            return false;

        Orders.RemoveAt(index);
        return true;
    }

    public bool CompletedOrder()
    {
        return Orders.Count == 0;
    }
}