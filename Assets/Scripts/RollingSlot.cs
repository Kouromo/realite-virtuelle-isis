using System.Collections;
using UnityEngine;

public class RollingSlot : MonoBehaviour
{
    private const byte NUM_SYMBOLS = 5;

    [Header("Rollers")]
    public GameObject[] rollers;

    [Header("Settings")]
    public float rotationSpeed = 1000f;
    public float minSpinDuration = 1f;
    public float stopDuration = 1f;
    public float symbolAngleCalibration = 50f;

    [Header("Slot sound")]
    public AudioSource audioSource;

    private Coroutine[] rotationCoroutines;
    private bool isSpinning;
    private int remainingRollers;
    private float symbolAngle;

    void Start()
    {
        symbolAngle = 360f / NUM_SYMBOLS;
        if (rollers != null)
            rotationCoroutines = new Coroutine[rollers.Length];
    }

    public void PullLever()
    {
        if (isSpinning || rollers == null || rollers.Length == 0)
            return;

        isSpinning = true;
        remainingRollers = 0;

        // ensure array matches rollers length
        if (rotationCoroutines == null || rotationCoroutines.Length != rollers.Length)
            rotationCoroutines = new Coroutine[rollers.Length];

        for (int i = 0; i < rollers.Length; i++)
        {
            var roller = rollers[i];
            if (roller == null)
                continue;

            remainingRollers++;

            // snap to nearest notch to avoid visual drift
            var rot = roller.transform.localEulerAngles;
            float currentX = NormalizeAngle(rot.x);
            int notch = Mathf.RoundToInt((currentX - symbolAngleCalibration) / symbolAngle);
            roller.transform.localEulerAngles = new Vector3(notch * symbolAngle + symbolAngleCalibration, rot.y, rot.z);

            int finalNotch = Random.Range(0, NUM_SYMBOLS);
            float target = finalNotch * symbolAngle + symbolAngleCalibration;
            float extraSpins = 360f * NUM_SYMBOLS; // at least one full set

            // stop existing coroutine for this roller if any
            if (rotationCoroutines[i] != null)
            {
                StopCoroutine(rotationCoroutines[i]);
                rotationCoroutines[i] = null;
            }

            rotationCoroutines[i] = StartCoroutine(RotateAndStop(roller, target + extraSpins, minSpinDuration, i));
        }

        // if no valid rollers, reset flag
        if (remainingRollers == 0)
            isSpinning = false;
    }

    IEnumerator RotateAndStop(GameObject roller, float addedRotation, float spinDuration, int index)
    {
        float t = 0f;

        // continuous spin phase
        while (t < spinDuration)
        {
            roller.transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime, Space.Self);
            t += Time.deltaTime;
            yield return null;
        }

        // compute target relative to current angle to respect initial rotation
        float current = NormalizeAngle(roller.transform.localEulerAngles.x);
        float absoluteTarget = current + addedRotation;
        float finalAngle = Mathf.Repeat(absoluteTarget, 360f);

        float elapsed = 0f;
        float startAngle = NormalizeAngle(roller.transform.localEulerAngles.x);

        // smooth stop using ease-out (sin)
        while (elapsed < stopDuration)
        {
            float u = Mathf.Clamp01(elapsed / stopDuration);
            float ease = Mathf.Sin(u * Mathf.PI * 0.5f);
            float angle = Mathf.LerpAngle(startAngle, finalAngle, ease);
            var newRot = roller.transform.localEulerAngles;
            newRot.x = angle;
            roller.transform.localEulerAngles = newRot;

            elapsed += Time.deltaTime;
            yield return null;
        }

        var final = roller.transform.localEulerAngles;
        final.x = finalAngle;
        roller.transform.localEulerAngles = final;

        if (audioSource != null)
            audioSource.Play();

        // mark coroutine finished
        if (rotationCoroutines != null && index >= 0 && index < rotationCoroutines.Length)
            rotationCoroutines[index] = null;

        remainingRollers--;
        if (remainingRollers <= 0)
        {
            isSpinning = false;
            Debug.Log("All rollers stopped.");
        }
    }

    private static float NormalizeAngle(float a) => Mathf.Repeat(a, 360f);
}