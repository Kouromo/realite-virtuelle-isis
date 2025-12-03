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
    public float amplitude = 0.5f; // intensité [0-1]  
    public float duration = 0.2f;  // en secondes  
    public TMP_Text moneyText;
    public GameObject manetteDroite;



    public void Start()
    {
        moneyText.text = money.ToString();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pay"))
        {
            HideShowCard();
        }
    }

    private void HideShowCard()
    {
        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }


    public void InteractWithSensor(int value)
    {
        ModifyMoney(value);

        manetteDroite.GetComponent<HapticImpulsePlayer>().SendHapticImpulse(amplitude, duration);
    }

    private void ModifyMoney(int value)
    {
        money += value;
        moneyText.text = money.ToString();
    }

}
