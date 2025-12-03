using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InterpreteResult : MonoBehaviour
{
    public List<Transform> rollers;
    private float angleCranCalibrate = 50;
    private int jetonsGagnes = 100;
    private List<int> rolls;
    //public CardSensor cardSensor;

    private void Start()
    {
        Debug.Log(CalculerGain(new List<int> { 1, 2, 3, 4 }));
    }

    public int GetGain()
    {
        rolls = new List<int>();
        GetRolls();
        int gain = CalculerGain(rolls);
        //cardSensor.Value = gain;
        return gain;
    }

    private void GetRolls()
    {
        foreach (Transform roller in rollers)
        {
            float currentX = roller.localEulerAngles.x;
            int indexCranActuel = Mathf.RoundToInt(currentX-angleCranCalibrate / (360f / 5f)); // Assuming 5 symbols
            rolls.Add(indexCranActuel);
        }
    }

    private int CalculerGain(List<int> valeurs)
    {
        var groups = valeurs.GroupBy(v => v)
                            .Select(g => g.Count())
                            .OrderByDescending(c => c)
                            .ToList();

        int maxCount = groups.First(); // nombre max de repetitions

        return maxCount switch
        {
            4 => jetonsGagnes,
            3 => (int)(jetonsGagnes * 0.75),
            2 => (int)(jetonsGagnes * 0.5),
            _ => 0
        };
    }
}
