using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class CardInteraction : MonoBehaviour
{
    [Header("Monetary System")]
    private int money = 100;
    public float amplitude = 0.7f; // intensité [0-1]  
    public float duration = 0.4f;  // en secondes  
    public TMP_Text moneyText;
    public GameObject manetteDroite;
    public InputAction primaryButton_Pay;
    private bool isCardVisible = false;



    public void Start()
    {
        moneyText.text = money.ToString();
    }

    private void Update()
    {
        if (primaryButton_Pay.WasPressedThisFrame())
        {
            HideShowCard();
        }
    }

    private void HideShowCard()
    {
        isCardVisible = !isCardVisible;
        this.gameObject.transform.GetChild(0).gameObject.SetActive(isCardVisible);
    }


    public void InteractWithSensor(CardSensor cardSensor)
    {
        if (!isCardVisible)
            return;

        ModifyMoney(cardSensor.Value);
        cardSensor.Value = 0; // Empeche les interactions multiples

        manetteDroite.GetComponent<HapticImpulsePlayer>().SendHapticImpulse(amplitude, duration);
    }

    private void ModifyMoney(int value)
    {
        money += value;
        moneyText.text = money.ToString();
    }

}
