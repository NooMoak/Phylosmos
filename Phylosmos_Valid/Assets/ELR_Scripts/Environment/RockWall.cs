using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockWall : MonoBehaviour
{
    [SerializeField] GameObject[] rocks;
    [SerializeField] Material[] dissolveMats;
    int index = 0;
    float dissolveAmount = 0f;
    bool dissolving = false;
    [SerializeField] AudioClip audioRock;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(dissolving)
        {
            dissolveAmount = Mathf.Lerp(dissolveAmount, 1, 0.05f);
            foreach(GameObject rock in rocks)
            {
                rock.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_DissolveAmount", dissolveAmount);
                
            }
        }
    }

    private void OnTriggerStay(Collider other) 
    {
        if(other.gameObject.tag == "Player" && other.gameObject.GetComponent<PlayerController>().rockAb == true && dissolving == false)
        {
            foreach(GameObject rock in rocks)
            {
                rock.gameObject.GetComponent<MeshRenderer>().material = dissolveMats[index];
                Destroy(rock, 3);
            }
            dissolveAmount = 0;
            dissolving = true;
            GetComponentInChildren<BoxCollider>().isTrigger = true;
            GetComponent<AudioSource>().clip = audioRock;
            GetComponent<AudioSource>().Play();
            Destroy(this.gameObject, 3);
        }
    }
}
