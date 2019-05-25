using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    [SerializeField] GameObject bulletDeflect;

	// Use this for initialization
	void Start () 
	{
		Destroy(this.gameObject, 3f);
	}
	
	void OnTriggerEnter(Collider collision) 
	{
		if (collision.gameObject.CompareTag ("Liana"))
        {
            GameObject deflection = Instantiate(bulletDeflect, transform.position, transform.rotation);
			Destroy(this.gameObject);
            Destroy(deflection, 3);
        }
        if (collision.gameObject.CompareTag ("Spike"))
        {
            GameObject hit = collision.gameObject;
            hit.GetComponent<EnemyLife>().TakeDamage(1);
			Destroy(this.gameObject);
        }
        if (collision.gameObject.CompareTag ("Healer"))
        {
            GameObject hit = collision.gameObject;
            hit.GetComponent<EnemyLife>().TakeDamage(1);
			Destroy(this.gameObject);
        }
        if (collision.gameObject.CompareTag ("Rock"))
        {
            if(collision.gameObject.GetComponent<EnemyLife>().invicible == true)
            {
                GameObject deflection = Instantiate(bulletDeflect, transform.position, transform.rotation);
                Destroy(deflection, 3);
            }
            GameObject hit = collision.gameObject;
            hit.GetComponent<EnemyLife>().TakeDamage(1);
			Destroy(this.gameObject);
        }
        if (collision.gameObject.CompareTag ("Wall"))
        {
			Destroy(this.gameObject);
        }

	}
}
