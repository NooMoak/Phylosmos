using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class SpikeEnemy : EnemyMovement
{
    public Transform target;
    public float chaseRadius;
    public float attackRadius;

    //public Transform homePosition;
	private Rigidbody rb;
    public Animator anim;
    bool shootReady = true;
    [SerializeField]
    float launchForce = 1000f;
    [SerializeField]
    GameObject spikeProjectile;
    [SerializeField]
    GameObject healthBar;
    // Use this for initialization
    void Start()
    {
        currentState = EnemyState.Idle;
        target = GameObject.FindWithTag("Player").transform;
		rb = GetComponent<Rigidbody>(); 
        anim = GetComponent<Animator>();
    }
	
    void Update() 
    {
        healthBar.transform.localScale = new Vector3(5, 15, health * 20);
    }
	// Update is called once per frame
	void FixedUpdate ()
    {
        CheckDistance();
	}

    void CheckDistance()
    {
        if (Vector3.Distance(target.position , transform.position) <= chaseRadius && Vector3.Distance(target.position , transform.position)>attackRadius)
        {
            if (currentState == EnemyState.Idle || currentState == EnemyState.Walk && currentState != EnemyState.Stagger)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                Vector3 targetPos = new Vector3(target.position.x, 0, target.position.z);
                transform.LookAt(target);
                transform.rotation = transform.rotation * Quaternion.Euler(0,90,0);
                ChangeAnim(temp - transform.position);
                rb.MovePosition(temp);
                ChangeState(EnemyState.Walk);
                //anim.SetBool("wakeUp", true);
            }
        }
        else if (Vector3.Distance(target.position, transform.position) <= attackRadius)
        {
            if(currentState == EnemyState.Idle || currentState == EnemyState.Walk && currentState != EnemyState.Stagger)
            {
                Vector3 targetPos = new Vector3(target.position.x, 0, target.position.z);
                transform.LookAt(target);
                transform.rotation = transform.rotation * Quaternion.Euler(0,90,0);
                if(shootReady){
                    StartCoroutine(SpikeShoot());
                }
            }
        } 
        else if (Vector3.Distance(target.position, transform.position) > chaseRadius)
        {
            //anim.SetBool("wakeUp", false);
        }
    }
    
    private IEnumerator SpikeShoot()
    {
		shootReady = false;

        yield return new WaitForSeconds(0.2f);

		GameObject clone1;
        GameObject clone2;
        GameObject clone3;
        Vector3 look = target.position - transform.position;
		clone1 = Instantiate(spikeProjectile, transform.position + new Vector3(0,2,0), transform.rotation);
        clone1.transform.rotation = Quaternion.LookRotation (look) * Quaternion.Euler(0,90,90);
		Vector3 dir = (target.position + new Vector3(0,2,0)) - clone1.transform.position;
		dir = dir.normalized;
		clone1.GetComponent<Rigidbody>().AddForce(dir * launchForce);

        clone2 = Instantiate(spikeProjectile, transform.position + new Vector3(0,2,0), transform.rotation);
        clone2.transform.rotation = Quaternion.LookRotation (look) * Quaternion.Euler(0,135,90);
        clone2.GetComponent<Rigidbody>().AddForce(Quaternion.AngleAxis(45f, Vector3.up) * dir * launchForce);

        clone3 = Instantiate(spikeProjectile, transform.position + new Vector3(0,2,0), transform.rotation);
        clone3.transform.rotation = Quaternion.LookRotation (look) * Quaternion.Euler(0,45,90);
        clone3.GetComponent<Rigidbody>().AddForce(Quaternion.AngleAxis(-45f, Vector3.up) * dir * launchForce);

		yield return new WaitForSeconds(2f);
		shootReady = true;
    }

    private void SetAnimFloat(Vector2 setVector)
    {
        //anim.SetFloat("moveX", setVector.x);
        //anim.SetFloat("moveY", setVector.y);
    }

    private void ChangeAnim(Vector2 direction)
    {
        if(Mathf.Abs(direction.x )> Mathf.Abs (direction.y))
        {
            if (direction.x > 0 )
            {
                //SetAnimFloat(Vector2.right);
            }
            else if (direction.x < 0)
            {
                //SetAnimFloat(Vector2.left);
            }
        }
        else if(Mathf.Abs (direction.x) < Mathf.Abs (direction.y))
        {
            if (direction.y > 0)
            {
                //SetAnimFloat(Vector2.up);
            }
            else if (direction.y< 0)
            {
                //SetAnimFloat(Vector2.down);
            }
        } 
    }

    private void ChangeState(EnemyState newState)
    {

        if (currentState != newState )
        {
            currentState = newState;
        }
    }
}
