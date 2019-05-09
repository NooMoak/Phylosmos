using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockRotate : MonoBehaviour
    
{
    Animator anim;
    public bool animActive;
   // public float speed = 50.0f;
    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(RotateStart());
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (GameObject.FindWithTag("Player").GetComponent<PlayerController>().rockAb == true)
            {
                anim.SetBool("Move", true);
            }
            else
            {
                anim.SetBool("Move", false);
            }
        }
    }

    /* IEnumerator RotateStart()
     {
         transform.Rotate(Vector3.up * speed * Time.deltaTime);
         yield return new WaitForSeconds(5f);
         transform.Rotate(Vector3.up * speed * Time.deltaTime);
         StopCoroutine(RotateStart());
     }*/
}
