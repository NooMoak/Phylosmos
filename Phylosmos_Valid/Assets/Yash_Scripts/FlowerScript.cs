using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerScript : MonoBehaviour
{
    Animator anim;
    bool used = false;
    [SerializeField] GameObject secondFlower;
    [SerializeField] GameObject thirdFlower;
    [SerializeField] GameObject slope;
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
        if(other.gameObject.tag == "Player")
        {
            if(other.GetComponent<PlayerController>().flowerAnim == true && used == false)
            {
                anim.SetTrigger("Grow");
                StartCoroutine(WaitAnim());
                used = true;
            }
        }
    }

    IEnumerator WaitAnim()
    {
        yield return new WaitForSeconds(1);
        secondFlower.SetActive(true);
        thirdFlower.SetActive(true);
        slope.SetActive(true);
    }  
    
}
