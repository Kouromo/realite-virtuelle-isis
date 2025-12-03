using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gestionObject : MonoBehaviour
{
    [System.Serializable]
    public struct Objet
    {
        public string nom;
        public string description;
        public string chip;
        public Sprite image;
    }

    [Header("Objets à afficher")]
    public GameObject object1;
    public GameObject object2;

    [Header("Liste d'objets")]
    [SerializeField] private Objet[] list_objets;

    private int index = 0;

    void Start()
    {
        AfficherPaire();
    }

    void AfficherPaire()
    {
        // Plus d'objets → tout cacher
        if (index >= list_objets.Length)
        {
            object1.SetActive(false);
            object2.SetActive(false);
            return;
        }

        // Afficher le premier de la paire
        SetGameObject(object1, list_objets[index]);
        object1.SetActive(true);

        // Vérifier si un deuxième existe
        if (index + 1 < list_objets.Length)
        {
            SetGameObject(object2, list_objets[index + 1]);
            object2.SetActive(true);
        }
        else
        {
            // S'il reste 1 seul objet → cacher le 2e
            object2.SetActive(false);
        }
    }

    void SetGameObject(GameObject obj, Objet data)
    {
        TextMeshProUGUI[] texts = obj.GetComponentsInChildren<TextMeshProUGUI>();
        texts[0].text = data.nom;
        texts[1].text = data.description;
        texts[2].text = data.chip + " Jetons";

        Image img = obj.transform.Find("image_object").GetComponent<Image>();
        img.sprite = data.image;
    }

    // ---------- MÉTHODES PUBLIQUES POUR CHOISIR ----------
    public void ChoisirObjet1()
    {
        Debug.Log("Objet choisi : index " + index);
        PasserALaSuite();
    }

    public void ChoisirObjet2()
    {
        Debug.Log("Objet choisi : index " + (index + 1));
        PasserALaSuite();
    }

    void PasserALaSuite()
    {
        index += 2;
        AfficherPaire();
    }
}
