using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockAnim : MonoBehaviour
{
    public void ShockWaveAnim()
    {
        GetComponentInParent<RockBehavior>().ShockWaveAnim();
    }
}
