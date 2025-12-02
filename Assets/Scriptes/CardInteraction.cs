using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CardInteraction : MonoBehaviour
{
    public int money = 100;
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor currentInteractor = null;
    public float amplitude = 0.5f; // intensité [0-1]
    public float duration = 0.2f;  // en secondes
    public TMP_Text moneyText;

    public void Start()
    {
        moneyText.text = money.ToString();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CardSensor>(out CardSensor cardSensor))
        {
            if (currentInteractor==null)
                return;

            currentInteractor.xrController.SendHapticImpulse(amplitude, duration);
            ModifyMoney(cardSensor.Value);
        }
    }

    private void ModifyMoney(int value) 
    {
        money += value;
        moneyText.text = money.ToString();
    }
}
