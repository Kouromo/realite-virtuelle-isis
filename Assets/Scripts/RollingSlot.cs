using System.Collections;
using UnityEngine;

public class RollingSlot : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip stopSound;

    private const byte NUM_SYMBOLS = 5;
    private float symbolAngle;
    private float symbolAngleCalibration = 50f;

    [Header("Rollers")]
    public GameObject[] rollers;
    private Coroutine[] rotationCoroutines;

    [Tooltip("Rotation speed in degrees per second")]
    public float rotationSpeed = 1000f;

    [Header("Durations")]
    public float minSpinDuration = 1f;
    public float maxSpinDuration = 3f;

    private float stopDuration = 1f;


    private bool isSpinning = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        symbolAngle = 360f / NUM_SYMBOLS; // Using 'f' for float literal
        rotationCoroutines = new Coroutine[rollers.Length];
    }

    // Update is called once per frame
    void Update()
    {
        // No code needed here
    }

    public void PullLever()
    {
        if (isSpinning)
        {
            Debug.Log("Slot machine is already spinning. Wait for it to stop!");
            return;
        }

        isSpinning = true;

        for (int i = 0; i < rollers.Length; i++)
        {
            GameObject currentRoller = rollers[i];

            Vector3 currentRotation = currentRoller.transform.localEulerAngles;
            float currentX = currentRotation.x;

            int currentNotchIndex = Mathf.RoundToInt((currentX - symbolAngleCalibration) / symbolAngle);
            float snappedPosition = currentNotchIndex * symbolAngle + symbolAngleCalibration;
            currentRoller.transform.localEulerAngles = new Vector3(snappedPosition, currentRotation.y, currentRotation.z);

            int finalNotchRandom = Random.Range(0, NUM_SYMBOLS);
            float targetAngle = finalNotchRandom * symbolAngle + symbolAngleCalibration;
            float extraSpins = 360f * NUM_SYMBOLS;

            if (rotationCoroutines[i] != null)
                StopCoroutine(rotationCoroutines[i]);

            rotationCoroutines[i] = StartCoroutine(RotateAndStop(currentRoller, targetAngle + extraSpins, minSpinDuration, i));
        }
    }

    IEnumerator RotateAndStop(GameObject roller, float totalTargetRotation, float minimumDuration, int rollerIndex)
    {
        float elapsedTime = 0f;

        while (elapsedTime < minimumDuration)
        {
            roller.transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime, Space.Self);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        float currentStopDuration = 0f;

        float preciseFinalAngle = totalTargetRotation % 360f;
        float startingAngle = roller.transform.localEulerAngles.x;

        while (currentStopDuration < stopDuration)
        {
            float t = currentStopDuration / stopDuration;
            float decelerationCurve = Mathf.Sin(t * Mathf.PI * 0.5f);

            float currentAngle = Mathf.LerpAngle(startingAngle, preciseFinalAngle, decelerationCurve);

            Vector3 newRot = roller.transform.localEulerAngles;
            newRot.x = currentAngle;
            roller.transform.localEulerAngles = newRot;

            currentStopDuration += Time.deltaTime;
            yield return null;
        }

        Vector3 finalRot = roller.transform.localEulerAngles;
        finalRot.x = preciseFinalAngle;
        roller.transform.localEulerAngles = finalRot;

        audioSource.PlayOneShot(stopSound);

        RollerStopped(rollerIndex);
    }

    /// <summary>
    /// Checks if all rollers have finished spinning.
    /// </summary>
    private void RollerStopped(int rollerIndex)
    {
        // Indicate that this specific roller's coroutine has finished
        rotationCoroutines[rollerIndex] = null;

        // Check if ANY coroutine is still running
        for (int i = 0; i < rotationCoroutines.Length; i++)
            if (rotationCoroutines[i] != null)
                return;

        isSpinning = false;
        Debug.Log("All rollers stopped. Machine is ready to be pulled again!");

        // TODO: Add logic here for checking the winning condition.
    }
}