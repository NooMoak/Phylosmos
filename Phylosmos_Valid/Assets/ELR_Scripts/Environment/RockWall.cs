using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockWall : MonoBehaviour
{
    [SerializeField] GameObject[] rocks;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other) 
    {
        if(other.gameObject.tag == "Player" && other.gameObject.GetComponent<PlayerController>().rockAb == true)
        {
            foreach(GameObject rock in rocks)
            {
                Destroy(rock);
            }
            Destroy(this.gameObject);
        }
    }
}
