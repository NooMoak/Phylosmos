using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerDamage : MonoBehaviour
{
    public float playerHealth;
    float fadeRate = 1.5f;
    float targetAlpha;
    [SerializeField] Image healthBar;
    [SerializeField] Image redBorders;
    [SerializeField] Animator anim;
    [SerializeField] GameObject blackFade;
    [SerializeField] GameObject UI;
    [SerializeField] AudioClip audioHurt1;
    [SerializeField] AudioClip audioHurt2;
    bool invicible = false;

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
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, playerHealth/100, 0.2f);
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
        if(collision.gameObject.CompareTag("EnemyGrab"))
        {
            TakeDamage(10f);
        }
    }

    public void TakeDamage(float damage)
    {
        if(invicible == false)
        {
            Color curColor = redBorders.color;
            playerHealth -= damage;
            curColor.a = 0.7f;
            redBorders.color = curColor;
            targetAlpha = 0.7f;
            redBorders.enabled = true;
            Camera.main.gameObject.GetComponent<CameraController>().HurtCam();
            targetAlpha = 0.2f;
            int random = Random.Range(1,3);
            if(random == 1)
                GetComponent<AudioSource>().clip = audioHurt1;
            if(random == 2)
                GetComponent<AudioSource>().clip = audioHurt2;
            GetComponent<AudioSource>().Play();
            CheckHealth();
        }
    }

    void CheckHealth()
    {
        if(playerHealth <= 0)
        {
            anim.SetTrigger("Dead");
            GetComponent<PlayerController>().currentState = PlayerState.Stagger;
            invicible = true;
            StartCoroutine(Death());
        }
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(2);
        blackFade.GetComponent<Animator>().SetBool("FadeOut", true);
        UI.SetActive(false);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
