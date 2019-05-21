using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LianaAnim : MonoBehaviour
{
    [SerializeField] AudioClip audioGrab;
   public void GrabAnim()
   {
       GetComponentInParent<LianaBehavior>().GrabAnim();
       GetComponentInParent<AudioSource>().clip = audioGrab;
        GetComponentInParent<AudioSource>().Play();
   }
}
