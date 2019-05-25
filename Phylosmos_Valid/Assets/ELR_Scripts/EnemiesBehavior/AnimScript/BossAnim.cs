using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnim : MonoBehaviour
{
    public void AnimFireBall()
   {
        GetComponentInParent<FireBossBehavior>().FireBall();
   }

   public void AnimTornado()
   {
        GetComponentInParent<FireBossBehavior>().FireTornado();
   }

}
