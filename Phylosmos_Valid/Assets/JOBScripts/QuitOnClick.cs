using System.Collections;
using UnityEngine;

//Ce script permet de quitter le jeu

public class QuitOnClick : MonoBehaviour
{
    public void Quit ()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif

    }
}