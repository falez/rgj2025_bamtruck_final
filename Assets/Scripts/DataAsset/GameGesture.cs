using UnityEngine;

[CreateAssetMenu(fileName = "New GameGesture", menuName = "Bamzaar/Game/GameGesture")]
public class GameGesture : ScriptableObject
{
    [SerializeField] private Sprite sprite;

    public string GestureClass => name;
    public Sprite Sprite => sprite;
}