using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

//Ce script permet de selectionner les boutons du menu avec les fleches directionnelles + la touche entrée ou la souris.

public class SelectOnInput : MonoBehaviour {

    public EventSystem eventSystem;
    public GameObject selectedObject;

    private bool buttonSelected;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
	if (Input.GetAxisRaw ("Vertical") != 0 && buttonSelected == false)
        {
            eventSystem.SetSelectedGameObject(selectedObject);
            buttonSelected = true;
        }
	}

    private void OnDisable()
    {
        buttonSelected = false;
    }
}
