using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeAnim : MonoBehaviour
{
    [SerializeField] AudioClip audioShoot;
    // Start is called before the first frame update
    public void AnimShoot()
    {
        GetComponentInParent<SpikeBehavior>().AnimShoot();
        GetComponentInParent<AudioSource>().clip = audioShoot;
        GetComponentInParent<AudioSource>().Play();
    }
}
