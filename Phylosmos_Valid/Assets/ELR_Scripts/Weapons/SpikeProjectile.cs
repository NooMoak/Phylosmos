using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeProjectile : MonoBehaviour
{
    void Start () 
	{
		Destroy(this.gameObject, 3f);
	}
}
