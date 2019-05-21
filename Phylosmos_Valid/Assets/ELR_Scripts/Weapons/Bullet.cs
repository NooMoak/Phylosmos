using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		Destroy(this.gameObject, 3f);
	}
	
	void OnTriggerEnter(Collider collision) 
	{
		if (collision.gameObject.CompareTag ("Liana"))
        {
			Destroy(this.gameObject);
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
