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
    public int money = 100;
    public float amplitude = 1f; // intensité [0-1]  
    public float duration = 0.4f;  // en secondes  
    public TMP_Text moneyText;
    public GameObject manetteDroite;
    public InputAction primaryButton_Pay;
    private bool isCardVisible = true;



    public void Start()
    {
        moneyText.text = money.ToString();
        this.gameObject.transform.position = manetteDroite.transform.position;
    }

    private void Update()
    {
        if (primaryButton_Pay.WasPressedThisFrame())
        {
            HideShowCard();
        }
        this.gameObject.transform.position = manetteDroite.transform.position + new Vector3(0,0,0.2f);

    }

    private void HideShowCard()
    {
        isCardVisible = !isCardVisible;
        this.gameObject.transform.GetChild(0).gameObject.SetActive(isCardVisible);
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collide");
        if(other.TryGetComponent<CardSensor>(out CardSensor sensor))
        {
            InteractWithSensor(sensor);
            if (sensor.rollingSlot != null) //decommenter lors du merge avec slot machine
            {
                sensor.rollingSlot.canRoll = true;
            }
            Debug.Log("as CardSensor");
        }
    }
    public void InteractWithSensor(CardSensor cardSensor)
    {
        Debug.Log("fonctionlance");
        if (!isCardVisible)
            return;

        Debug.Log("passe visible");
        ModifyMoney(cardSensor.Value);
        if(cardSensor.Value!=0)
            manetteDroite.GetComponent<HapticImpulsePlayer>().SendHapticImpulse(amplitude, duration);
        cardSensor.Value = 0; // Empeche les interactions multiples

    }

    private void ModifyMoney(int value)
    {
        if (value > money)
        {
            money = 0;
        }
        else
        {
        money += value;
        }
        moneyText.text = money.ToString();
    }

}
