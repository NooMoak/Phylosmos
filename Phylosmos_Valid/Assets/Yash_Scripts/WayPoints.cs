using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    [SerializeField]
    protected float debugDrawRadius = 1.0f;

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, debugDrawRadius);
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "NPC")
        {
            NpcPatrol.patrolWaiting = true;
        }
    }*/
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
