using UnityEngine;

public class CardSensor : MonoBehaviour
{
    public int Value;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CardInteraction>(out CardInteraction card))
        {
            card.InteractWithSensor(Value);
        }
    }
}
