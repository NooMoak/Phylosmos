using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    [SerializeField] int damage;
	PlayerController giveAbility;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag ("Destructible"))
        {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag ("Liana"))
        {
            GameObject hit = collision.gameObject;
			giveAbility = GetComponentInParent<PlayerController>();
            if(giveAbility.lianaCharge < 3)
				giveAbility.lianaCharge += 1;
            hit.GetComponent<EnemyLife>().TakeDamage(damage);
            hit.GetComponent<LianaBehavior>().StartCoroutine("Stunned");
        }
        if (collision.gameObject.CompareTag ("Spike"))
        {
            GameObject hit = collision.gameObject;
			giveAbility = GetComponentInParent<PlayerController>();
            if(giveAbility.spikeCharge < 3)
				giveAbility.spikeCharge += 1;
            hit.GetComponent<EnemyLife>().TakeDamage(damage);
        }
        if (collision.gameObject.CompareTag ("Healer"))
        {
            GameObject hit = collision.gameObject;
            giveAbility = GetComponentInParent<PlayerController>();
            if(giveAbility.healerCharge < 3)
			    giveAbility.healerCharge += 1;
            hit.GetComponent<EnemyLife>().TakeDamage(damage);
        }
        if (collision.gameObject.CompareTag ("Rock"))
        {
            GameObject hit = collision.gameObject;
			giveAbility = GetComponentInParent<PlayerController>();
            if(giveAbility.rockCharge < 3)
				giveAbility.rockCharge += 1;
            hit.GetComponent<EnemyLife>().TakeDamage(damage);
        }
    }
}
