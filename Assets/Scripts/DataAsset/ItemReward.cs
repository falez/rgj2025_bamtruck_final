using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemData", menuName = "Bamzaar/ItemData")]
public class ItemDataSO : ScriptableObject, IItem
{
    [SerializeField] private Sprite sprite;

    public string Id => name;

    public Sprite Sprite => sprite;
}