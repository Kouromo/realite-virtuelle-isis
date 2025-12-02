using TMPro;
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
    private IXRHapticImpulseChannel hapticsChannel = null;

    [Header("Teleportation System")]
    public InputActionProperty buttonA;
    //public XRDirectInteractor rightHandInteractor;
    public Transform rightHandInteractor;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabbable;


    public void Start()
    {
        moneyText.text = money.ToString();
        grabbable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    private void Update()
    {
        if (buttonA.action.WasPressedThisFrame())
        {
            TeleportIntoHand();
        }
    }

    private void TeleportIntoHand()
    {
        if (rightHandInteractor == null)
            return;


        transform.position = new Vector3(
            rightHandInteractor.transform.position.x,
            rightHandInteractor.transform.position.y + 0.1f,
            rightHandInteractor.transform.position.z + 0.1f
        );
        transform.rotation = rightHandInteractor.transform.rotation;

        //rightHandInteractor.interactionManager.SelectEnter((IXRSelectInteractor) rightHandInteractor, grabbable);
    }


    public void InteractWithSensor(int value)
    {
        ModifyMoney(value);

        hapticsChannel ??= GetComponent<IXRHapticImpulseChannel>();
        if (hapticsChannel != null)
        {
            hapticsChannel.SendHapticImpulse(amplitude, duration);
        }
    }

    private void ModifyMoney(int value)
    {
        money += value;
        moneyText.text = money.ToString();
    }

    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        hapticsChannel = args.interactorObject.transform.GetComponent<IXRHapticImpulseChannel>();
    }

    public void OnSelectExited(SelectExitEventArgs args)
    {
        hapticsChannel = null;
    }
}
