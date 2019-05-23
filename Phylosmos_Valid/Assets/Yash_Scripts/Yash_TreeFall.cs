using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yash_TreeFall : MonoBehaviour
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
    public void OnTriggerStay(Collider other)
    {
        /*if (GameObject.FindWithTag("Player").GetComponent<PlayerController>().lianaAnim == true)
        {
            anim.SetBool("Fall", true);
            
        }
        */
    }
}
