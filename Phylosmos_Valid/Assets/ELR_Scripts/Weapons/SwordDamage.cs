using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    [SerializeField] int damage;
	PlayerController giveAbility;
    DataSaver dataSaver;

    private void Start() 
    {
        dataSaver = FindObjectOfType<DataSaver>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag ("Destructible"))
        {
            collision.gameObject.GetComponent<LianaWall>().Fade();
        }
        if (collision.gameObject.CompareTag ("Liana"))
        {
            GameObject hit = collision.gameObject;
			giveAbility = GetComponentInParent<PlayerController>();
            if(giveAbility.lianaCharge < 3)
            {
				giveAbility.lianaCharge += 1;
                FindObjectOfType<DataSaver>().lianaCharge += 1;
            }
            hit.GetComponent<EnemyLife>().TakeDamage(damage);
            hit.GetComponent<LianaBehavior>().StartCoroutine("Stunned");

            if(dataSaver.knowLiana == false)
            {
                dataSaver.knowLiana = true;
            }
        }
        if (collision.gameObject.CompareTag ("Spike"))
        {
            GameObject hit = collision.gameObject;
			giveAbility = GetComponentInParent<PlayerController>();
            if(giveAbility.spikeCharge < 3)
            {
				giveAbility.spikeCharge += 1;
                FindObjectOfType<DataSaver>().spikeCharge += 1;
            }
            hit.GetComponent<EnemyLife>().TakeDamage(damage);

            if(dataSaver.knowSpike == false)
            {
                dataSaver.knowSpike = true;
            }
        }
        if (collision.gameObject.CompareTag ("Healer"))
        {
            GameObject hit = collision.gameObject;
            giveAbility = GetComponentInParent<PlayerController>();
            if(giveAbility.healerCharge < 3)
            {
			    giveAbility.healerCharge += 1;
                FindObjectOfType<DataSaver>().healerCharge += 1;
            }
            hit.GetComponent<EnemyLife>().TakeDamage(damage);

            if(dataSaver.knowHealer == false)
            {
                dataSaver.knowHealer = true;
            }
        }
        if (collision.gameObject.CompareTag ("Rock"))
        {
            GameObject hit = collision.gameObject;
			giveAbility = GetComponentInParent<PlayerController>();
            if(giveAbility.rockCharge < 3)
            {
				giveAbility.rockCharge += 1;
                FindObjectOfType<DataSaver>().rockCharge += 1;
            }
            hit.GetComponent<EnemyLife>().TakeDamage(damage);

            if(dataSaver.knowRock == false)
            {
                dataSaver.knowRock = true;
            }
        }
    }
}
