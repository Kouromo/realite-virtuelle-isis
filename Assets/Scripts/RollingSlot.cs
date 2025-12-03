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
            GameObject currentRoller = rollers[i];

            Vector3 currentRotation = currentRoller.transform.localEulerAngles;
            float currentX = currentRotation.x;

            int indexCranActuel = Mathf.RoundToInt((currentX - angleCranCalibrate) / angleCran);
            float positionBienCranee = indexCranActuel * angleCran + angleCranCalibrate;


            currentRoller.transform.localEulerAngles = new Vector3(positionBienCranee, currentRotation.y, currentRotation.z);

            int notchRandomFinal = Random.Range(0, NUM_SYMBOLS);

            float angleCible = notchRandomFinal * angleCran + angleCranCalibrate;
            float MoreSpin = 360 * NUM_SYMBOLS;

            // Démarrer la rotation depuis cette position normalisée
            StartCoroutine(RotateAndStop(currentRoller, angleCible + MoreSpin, duration));
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

        float durationStopCurrent = 0;

        // Calcul de la cible finale uniquement (sans l'excès de 360*NUM_SYMBOLS)
        float angleFinalPrecise = rotationTotaleCible % 360f;

        // Stocker la rotation de départ pour le lissage
        float angleDepart = rouleau.transform.localEulerAngles.x;

        while (durationStopCurrent < durationStop)
        {
            float t = durationStopCurrent / durationStop;

            float courbeDeceleration = Mathf.Sin(t * Mathf.PI * 0.5f);

            float angleActuel = Mathf.LerpAngle(angleDepart, angleFinalPrecise, courbeDeceleration);

            Vector3 newRot = rouleau.transform.localEulerAngles;
            newRot.x = angleActuel;
            rouleau.transform.localEulerAngles = newRot;

            durationStopCurrent += Time.deltaTime;
            yield return null;
        }

        Vector3 finalRot = rouleau.transform.localEulerAngles;
        finalRot.x = angleFinalPrecise;
        rouleau.transform.localEulerAngles = finalRot;

        // TODO: Notifier le script principal que ce rouleau est arrêté
    }
}
