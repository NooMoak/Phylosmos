using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockAnim : MonoBehaviour
{
    [SerializeField] AudioClip audioShock;
    public void ShockWaveAnim()
    {
        GetComponentInParent<RockBehavior>().ShockWaveAnim();
        GetComponentInParent<AudioSource>().clip = audioShock;
        GetComponentInParent<AudioSource>().Play();
        GameObject parent = transform.parent.gameObject;
        parent.GetComponentInChildren<ParticleSystem>().Play();
    }
}
