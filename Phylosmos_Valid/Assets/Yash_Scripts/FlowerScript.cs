using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerScript : MonoBehaviour
{
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if(GameObject.FindWithTag("Player").GetComponent<PlayerController>().flowerAnim == true)
        {
            anim.SetBool("Grow", true);
            Debug.Log("FlowerAnim");
        }
    }
        
    
}
