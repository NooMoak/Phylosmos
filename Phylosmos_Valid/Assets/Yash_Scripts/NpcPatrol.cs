using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcPatrol : MonoBehaviour

{

   
    public static bool patrolWaiting = true;

    [SerializeField]
    float totalWaitTime = 3f;

    [SerializeField]
    float switchProbablity = 0.2f;

    [SerializeField]
    List<WayPoints> patrolPoints;

    NavMeshAgent navMeshAgent;
    public int currentPatrolIndex;
    public  bool travelling;
    public static bool waiting;
    public bool patrolForward;
    public float waitTimer;

    

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();

        if (navMeshAgent == null)
        {
            Debug.Log("Agent");
        }
        else
        {

            if (navMeshAgent != null && patrolPoints.Count >= 2)
            {
                currentPatrolIndex = 0;
                SetDestination();
            }
            else
            {
                Debug.Log("Add more points for patrol");
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (travelling && navMeshAgent.remainingDistance <= 1.0f)
        {
            travelling = false;

            if (patrolWaiting)
            {
                waiting = true;
                waitTimer =0f;
            }
            else
            {
                ChangePatrolPoint();
                SetDestination();
            }
        }

        if (waiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= totalWaitTime)
            {
                waiting = false;

                ChangePatrolPoint();
                SetDestination();
            }
        }
        
    }

    private void SetDestination()
    {
        if (patrolPoints != null)
        {
            Vector3 targetVector = patrolPoints[currentPatrolIndex].transform.position;
            navMeshAgent.SetDestination(targetVector);
            travelling = true;
        }
    }

    private void ChangePatrolPoint()
    {
        if (UnityEngine.Random.Range (0f,1f)<= switchProbablity )
        {
            patrolForward = !patrolForward;
        }
        if (patrolForward )
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        }
        else
        {
            if (--currentPatrolIndex < 0)
            {
                currentPatrolIndex = patrolPoints.Count - 1;
            }
        }
    }
}
