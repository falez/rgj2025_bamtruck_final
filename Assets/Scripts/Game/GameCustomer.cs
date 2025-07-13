using System.Collections;
using System.Collections.Generic;

public class GameCustomer
{
    public List<FoodOrderInstance> Orders { get; } = new();

    private int correctCount = 0;

    public (bool, int) TryMatchOrder(string order)
    {
        int index = Orders.FindIndex((x) => x.Id.Equals(order) && !x.met);

        if (index < 0)
            return (false, -1);

        Orders[index].met = true;

        correctCount++;
        return (true, index);
    }

    public int OrderCount => Orders.Count;

    public bool CompletedOrder()
    {
        return correctCount == Orders.Count;
    }
}