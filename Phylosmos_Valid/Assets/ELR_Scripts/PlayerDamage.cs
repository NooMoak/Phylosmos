using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamage : MonoBehaviour
{
    public float playerHealth;
    float fadeRate = 1.5f;
    float targetAlpha;
    [SerializeField]
    Image healthBar;
    [SerializeField]
    Image redBorders;

    [SerializeField]
    Animator anim;

    private void Start() 
    {
        playerHealth = 100f;
        targetAlpha = 0.7f;
    }

     private void Update() 
    {
        if(playerHealth > 100f)
        {
            playerHealth = 100f;
        }
        healthBar.fillAmount = playerHealth / 100;
        Color curColor = redBorders.color;
        float alphaDiff = Mathf.Abs(curColor.a-targetAlpha);
        if (alphaDiff>0.0001f)
        {
            curColor.a = Mathf.Lerp(curColor.a,targetAlpha,fadeRate*Time.deltaTime);
            redBorders.color = curColor;
        }
        if(alphaDiff <0.0001f)
        {
            curColor.a = 0.7f;
            redBorders.color = curColor;
            targetAlpha = 0.7f;
            redBorders.enabled = false;
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
		if(collision.gameObject.CompareTag("SpikeProjectile"))
        {
            Destroy(collision.gameObject);
            TakeDamage(10f);
        }
    }

    void TakeDamage(float damage)
    {
        Color curColor = redBorders.color;
        //playerHealth -= damage;
        curColor.a = 0.7f;
        redBorders.color = curColor;
        targetAlpha = 0.7f;
        redBorders.enabled = true;
        targetAlpha = 0.2f;
        CheckHealth();
    }

    void CheckHealth()
    {
        if(playerHealth <= 0)
        {
            anim.SetBool("Dead", true);
            Destroy(gameObject, 2);
        }
    }
}
