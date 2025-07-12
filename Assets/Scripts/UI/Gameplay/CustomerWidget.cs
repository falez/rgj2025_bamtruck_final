using UnityEngine;

public class CustomerWidget : MonoBehaviour
{
    [SerializeField] private GameObject neutralFace;
    [SerializeField] private GameObject happyFace;
    [SerializeField] private GameObject angryFace;

    public void MakeNeutralFace()
    {
        neutralFace.SetActive(true);
        happyFace.SetActive(false);
        angryFace.SetActive(false);
    }

    public void MakeHappyFace()
    {
        neutralFace.SetActive(false);
        happyFace.SetActive(true);
        angryFace.SetActive(false);
    }

    public void MakeAngryFace()
    {
        neutralFace.SetActive(false);
        happyFace.SetActive(false);
        angryFace.SetActive(true);
    }
}