using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LianaAnim : MonoBehaviour
{
   public void GrabAnim()
   {
       GetComponentInParent<LianaBehavior>().GrabAnim();
   }
}
