using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class HealerEnemy : EnemyMovement
{
    public Transform target;
    public float chaseRadius;
    public float healRadius;

    //public Transform homePosition;
    private Rigidbody rb;
    bool canHeal = true;
    public Animator anim;
    LineRenderer healLine;
    [SerializeField]
    GameObject healthBar;
    // Use this for initialization
    void Start()
    {
        healLine = GetComponent<LineRenderer>();
        currentState = EnemyState.Idle;
        target = GameObject.FindWithTag("Spike").transform;
        rb = GetComponent<Rigidbody>();
       // anim = GetComponent<Animator>();
    }

    void Update() 
    {
        healthBar.transform.rotation = Quaternion.Euler(0,-45,-30);
        healthBar.transform.localScale = new Vector3(5, 15, health * 20);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckDistance();
    }

    void CheckDistance()
    {
        if(target.gameObject.activeSelf == true){
            if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > healRadius)
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
                    if(Vector3.Distance(target.position, transform.position) > healRadius + 5)
                    {
                    healLine.enabled = false;
                    }
                }
            }
            else if (Vector3.Distance(target.position, transform.position) <= healRadius)
            {
                if (currentState == EnemyState.Idle || currentState == EnemyState.Walk && currentState != EnemyState.Stagger)
                { 
                    Vector3 targetPos = new Vector3(target.position.x, 0, target.position.z);
                    transform.LookAt(target);
                    transform.rotation = transform.rotation * Quaternion.Euler(0,90,0);
                    healLine.enabled = true;
                    healLine.SetPosition(0, transform.position);
                    healLine.SetPosition(1, target.transform.position);
                    if(canHeal)
                    {
                        StartCoroutine("Heal");
                    }
                }
            }
            else if (Vector3.Distance(target.position, transform.position) > chaseRadius)
            {
                //anim.SetBool("wakeUp", false);
            }
        }
        else if (target.gameObject.activeSelf == false)
        {
            healLine.enabled = false;
            target = GameObject.FindWithTag("Spike").transform;
        }
    }

    

    IEnumerator Heal()
    {
        canHeal = false;
        GameObject healed = target.gameObject;
        if(healed.GetComponent<SpikeEnemy>().health < 6f) 
        {
            healed.GetComponent<SpikeEnemy>().health += 2;
        }
        yield return new WaitForSeconds(0.5f);
        canHeal = true;
    }

    private void SetAnimFloat(Vector2 setVector)
    {
        //anim.SetFloat("moveX", setVector.x);
        //anim.SetFloat("moveY", setVector.y);
    }

    private void ChangeAnim(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                //SetAnimFloat(Vector2.right);
            }
            else if (direction.x < 0)
            {
                //SetAnimFloat(Vector2.left);
            }
        }
        else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            if (direction.y > 0)
            {
                //SetAnimFloat(Vector2.up);
            }
            else if (direction.y < 0)
            {
                //SetAnimFloat(Vector2.down);
            }
        }
    }

    private void ChangeState(EnemyState newState)
    {

        if (currentState != newState)
        {
            currentState = newState;
        }
    }
}