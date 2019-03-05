using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Ce script permet de charger la première scène du jeu donc il est associé au bouton start. Normalement il peut charger n'importe quelle scene, il faudra changer la valeur en fonction du numéro de la scene qu'on aura configuré dans les parametres de built.

public class LoadSceneOnClick : MonoBehaviour
{


    public void LoadByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }



}
