using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamage : MonoBehaviour
{
    float PlayerHealth;
    float fadeRate = 1.5f;
    float targetAlpha;
    [SerializeField]
    Image healthBar;
    [SerializeField]
    Image redBorders;

    private void Start() 
    {
        PlayerHealth = 100f;
        targetAlpha = 0.7f;
    }

     private void Update() 
    {
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
        PlayerHealth -= damage;
        healthBar.fillAmount = PlayerHealth / 100;
        curColor.a = 0.7f;
        redBorders.color = curColor;
        targetAlpha = 0.7f;
        redBorders.enabled = true;
        targetAlpha = 0.2f;
        CheckHealth();
    }

    void CheckHealth()
    {
        if(PlayerHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
