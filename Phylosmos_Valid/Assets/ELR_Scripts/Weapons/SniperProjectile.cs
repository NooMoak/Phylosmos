using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperProjectile : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		Destroy(this.gameObject, 3f);
	}
	
	void OnTriggerEnter(Collider collision) 
	{
		if (collision.gameObject.CompareTag ("Liana"))
        {
            GameObject hit = collision.gameObject;
            hit.GetComponent<EnemyLife>().TakeDamage(3);
            Destroy(this.gameObject);
        }
        if (collision.gameObject.CompareTag ("Spike"))
        {
            GameObject hit = collision.gameObject;
            hit.GetComponent<EnemyLife>().TakeDamage(3);
            Destroy(this.gameObject);
        }
        if (collision.gameObject.CompareTag ("Healer"))
        {
            GameObject hit = collision.gameObject;
            hit.GetComponent<EnemyLife>().TakeDamage(3);
            Destroy(this.gameObject);
        }
        if (collision.gameObject.CompareTag ("Rock"))
        {
            GameObject hit = collision.gameObject;
            hit.GetComponent<EnemyLife>().TakeDamage(3);
            Destroy(this.gameObject);
        }
        if (collision.gameObject.CompareTag ("Boss"))
        {
            GameObject hit = collision.gameObject;
            hit.GetComponent<EnemyLife>().TakeDamage(10);
            Destroy(this.gameObject);
        }
	}
}
