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
        public string chip;  // on garde le chip comme string
        public Sprite image;
    }

    [Header("GameObjects à remplir")]
    public GameObject object1;
    public GameObject object2;

    [Header("Liste d'objets")]
    [SerializeField]
    Objet[] list_objets;

    Objet[] current_list_objets;

    void Start()
    {
        // Cloner et trier la liste par chip décroissant
        current_list_objets = (Objet[])list_objets.Clone();
        System.Array.Sort(current_list_objets, 
            (a, b) => int.Parse(b.chip).CompareTo(int.Parse(a.chip)));

        if (current_list_objets.Length >= 2)
        {
            SetGameObject(object1, current_list_objets[0]);
            SetGameObject(object2, current_list_objets[1]);
        }
    }

    void SetGameObject(GameObject obj, Objet data)
    {
        if (obj == null) return;

        // Récupérer tous les Text dans les enfants
        TextMeshProUGUI[] texts = obj.GetComponentsInChildren<TextMeshProUGUI>();

        // On suppose : 
        // texts[0] = nom
        // texts[1] = description
        // texts[2] = chip
        if (texts.Length >= 3)
        {
            texts[0].text = data.nom;
            texts[1].text = data.description;
            texts[2].text = data.chip + "Jetons";
        }

        // Assigner l'image
        Image img = obj.transform.Find("image_object").GetComponent<Image>();
        if (img != null)
        {
            img.sprite = data.image;
        }
    }
}
