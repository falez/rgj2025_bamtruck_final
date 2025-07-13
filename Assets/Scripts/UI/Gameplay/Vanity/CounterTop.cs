using System.Collections.Generic;
using UnityEngine;

public class CounterTop : MonoBehaviour
{
    public List<Transform> children;

    private void Start()
    {
        children = new();

        foreach (Transform child in transform)
        {
            children.Add(child);
            child.gameObject.SetActive(false);
        }

        children[0].gameObject.SetActive(true);
    }
}