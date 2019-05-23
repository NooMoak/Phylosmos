using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerScript : MonoBehaviour
{
    public Animator anim;
    public GameObject flowerColldier;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        flowerColldier.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerStay(Collider other)
    {
        if(GameObject.FindWithTag("Player").GetComponent<PlayerController>().flowerAnim == true)
        {
            anim.SetBool("Grow", true);
            flowerColldier.SetActive(true);
        }
    }
        
    
}
