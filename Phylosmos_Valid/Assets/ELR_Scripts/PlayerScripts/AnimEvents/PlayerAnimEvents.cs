using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvents : MonoBehaviour
{
    [SerializeField] AudioClip audioGun;
    [SerializeField] AudioClip audioSword;
    [SerializeField] AudioClip audioWalk;
    GameObject player;

    private void Start() 
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void PlayerAnimShoot()
    {
        player.GetComponent<PlayerController>().Fire();
        player.GetComponentInParent<AudioSource>().clip = audioGun;
        player.GetComponentInParent<AudioSource>().Play();
    }

    public void PlayerAnimAttack()
    {
        player.GetComponentInParent<PlayerController>().AttackCO();
        player.GetComponentInParent<AudioSource>().clip = audioSword;
        player.GetComponentInParent<AudioSource>().Play();
    }

    public void PlayerAnimAttackStop()
    {
        player.GetComponentInParent<PlayerController>().AttackStop();
    }

    public void PlayerAnimLaser()
    {
        player.GetComponentInParent<PlayerController>().Laser();
    }

    public void PlayerAnimSniperShoot()
    {
        player.GetComponentInParent<PlayerController>().SniperShoot();
    }

    public void PlayerWalkSound()
    {
        GetComponent<AudioSource>().clip = audioWalk;
        GetComponent<AudioSource>().Play();
    }
}
