using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionManager : MonoBehaviour
{
    //A copier coller dans un script pour le transformer en MyInstance (remplacer DescriptionManager par le nom du script)
    private static DescriptionManager myInstance;

    public static DescriptionManager MyInstance
    {
        get
        {
            if(myInstance == null)
            {
                myInstance = FindObjectOfType<DescriptionManager>();

            }

            return myInstance;
        }
    }

    //Pour appeler ce script : NomDuScript.MyInstance.NomDeLaFonction();

    //Mettre en public les variables que tu veux appeler avec ton MyInstance

    //Permet de faire apparaître dans l'inspecteur une variable privée
    [SerializeField]
    //Permet d'agrandir la taille de l'espace du texte dans l'inspecteur (taille minimale, taille maximale)
    [TextArea(3,50)]
    private string primitif;

    [SerializeField]
    [TextArea(3, 50)]
    private string minéral;

    [SerializeField]
    [TextArea(3, 50)]
    private string squelette;

    public Text description;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PrimitifTexte()
    {
        description.text = description.text + " " + primitif;
    }

    public void MinéralText()
    {
        description.text = description.text + " " + minéral;
    }

    public void SqueletteTexte()
    {
        Debug.Log("Je suis là");
        description.text = description.text + " " + squelette;
    }
}

