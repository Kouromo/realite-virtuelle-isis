using System.Collections;
using UnityEngine;

public class RollingSlot : MonoBehaviour
{
    private const byte NUM_SYMBOLS = 5;
    private float angleCran;

    private float angleCranCalibrate = 50;

    public GameObject[] rollers;

    [Tooltip("Rotation speed in degrees per second")]
    public float rotationSpeed = 1000f;

    [Header("Durations")]
    public float duration = 1f;
    public float durationRotationMax = 3f;

    private float durationStop = 1;
    private float durationStopCurrent = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        angleCran = 360 / NUM_SYMBOLS;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PullLever()
    {
        for (int i = 0; i < rollers.Length; i++)
        {
            int notchRandomFinal = Random.Range(0, NUM_SYMBOLS);

            float angleCible = notchRandomFinal * angleCran + angleCranCalibrate;
            float MoreSpin = 360 * NUM_SYMBOLS;

            StartCoroutine(RotateAndStop(rollers[i], angleCible + MoreSpin, duration));
        }
    }

    IEnumerator RotateAndStop(GameObject rouleau, float rotationTotaleCible, float dureeMinimale)
    {
        float tempsEcoule = 0;
        while (tempsEcoule < dureeMinimale)
        {
            rouleau.transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime, Space.Self);
            tempsEcoule += Time.deltaTime;
            yield return null;
        }

        Vector3 rotationFinale = rouleau.transform.localEulerAngles;
        float currentX = rouleau.transform.localEulerAngles.x;
        int indexCranActuel = Mathf.RoundToInt(currentX / angleCran);
        float positionActuelleBienCranee = indexCranActuel * angleCran;

        float angleARoule = rotationTotaleCible - currentX;

        while (durationStopCurrent < durationStop)
        {
            float t = durationStopCurrent / durationStop;
            float courbeDeceleration = Mathf.Sin(t * Mathf.PI * 0.5f);

            float angleParcouru = Mathf.Lerp(0, angleARoule, courbeDeceleration);

            Vector3 newRot = new(positionActuelleBienCranee + angleParcouru, rotationFinale.y, rotationFinale.z);
            rouleau.transform.localEulerAngles = newRot;

            durationStopCurrent += Time.deltaTime;
            yield return null;
        }

        Vector3 rotationFinalePrecise = new(rotationTotaleCible % 360f, rotationFinale.y, rotationFinale.z);
        rouleau.transform.localEulerAngles = rotationFinalePrecise;

        // TODO: Notifier le script principal que ce rouleau est arrêté
    }
}
