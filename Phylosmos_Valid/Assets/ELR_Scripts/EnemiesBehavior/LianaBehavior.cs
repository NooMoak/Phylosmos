using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LianaState
    {
        Sleep, Fight, Stun, Flee, Return, Dead
    }
public class LianaBehavior : MonoBehaviour
{
    public LianaState currentState;
     GameObject player;
    [SerializeField] float fightRadius;
    [SerializeField] float lianaSpeed;
    [SerializeField] GameObject grab;
    [SerializeField] float grabSpeed;
    Vector3 homePosition;
    Vector3 grabTarget;
    Rigidbody rb;
    bool canGrab = true;
    bool grabbing = false;
    bool hasDied = false;
    Animator anim;
    [SerializeField] GameObject targetRotation;
    [SerializeField] float rotateSpeed;
    [SerializeField] Material dissolveMat1;
    [SerializeField] Material dissolveMat2;
    [SerializeField] Material dissolveMat3;
    [SerializeField] Material baseMat1;
    [SerializeField] Material baseMat2;
    [SerializeField] Material baseMat3;
    Renderer matRenderer;
    Material[] baseMaterials;
    Material[] newMaterials;
    float dissolveAmount = 0f;
    // Start is called before the first frame update
    void Start()
    {
        currentState = LianaState.Sleep;
        homePosition = transform.position;
        player = GameObject.FindWithTag("Player");
		rb = GetComponent<Rigidbody>(); 
        anim = GetComponentInChildren<Animator>();
        matRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        baseMaterials = new Material[]{baseMat1, baseMat2, baseMat3};
        newMaterials = new Material[]{dissolveMat1, dissolveMat2, dissolveMat3};
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState == LianaState.Dead)
        {
            if(hasDied == false)
            {
                StopCoroutine("Grab");
                StopCoroutine("Stunned");
                GetComponent<CapsuleCollider>().enabled = false;
                GetComponent<Rigidbody>().useGravity = false;
                //GetComponentInChildren<MeshRenderer>().enabled = false;
                anim.SetBool("IsWalking", false);
                matRenderer.materials = newMaterials;
                matRenderer.materials[0].SetFloat("_DissolveAmount", 0);
                matRenderer.materials[1].SetFloat("_DissolveAmount", 0);
                matRenderer.materials[2].SetFloat("_DissolveAmount", 0);
                dissolveAmount = 0;
                hasDied = true;
            }
            dissolveAmount = Mathf.Lerp(dissolveAmount, 1, 0.02f);
            matRenderer.materials[0].SetFloat("_DissolveAmount", dissolveAmount);
            matRenderer.materials[1].SetFloat("_DissolveAmount", dissolveAmount);
            matRenderer.materials[2].SetFloat("_DissolveAmount", dissolveAmount);
            if(Vector3.Distance(player.transform.position, homePosition) < 120 && Vector3.Distance(player.transform.position, homePosition) > 100)
                StartCoroutine("Respawn");
        }
    }

    void FixedUpdate() 
    {
        if((currentState == LianaState.Sleep || currentState == LianaState.Return) && Vector3.Distance(player.transform.position, homePosition) <= fightRadius)
        {
            currentState = LianaState.Fight;
            anim.SetFloat("StateSpeed", 1f); 
        }
        else if(currentState == LianaState.Fight && Vector3.Distance(player.transform.position, transform.position) > fightRadius - 20){
            rb.MovePosition(Vector3.MoveTowards(transform.position, player.transform.position, lianaSpeed * Time.deltaTime));
            targetRotation.transform.LookAt(player.transform.position);
            targetRotation.transform.rotation = targetRotation.transform.rotation * Quaternion.Euler(0,90,0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation.transform.rotation, rotateSpeed * Time.deltaTime);
            anim.SetBool("IsWalking", true);
        }
        else if (currentState == LianaState.Fight && Vector3.Distance(player.transform.position, transform.position) <= fightRadius - 20 && grabbing == false)
        {
            targetRotation.transform.LookAt(player.transform.position);
            targetRotation.transform.rotation = targetRotation.transform.rotation * Quaternion.Euler(0,90,0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation.transform.rotation, rotateSpeed * Time.deltaTime);
            anim.SetBool("IsWalking", false);
            if(canGrab == true)
            {
                StartCoroutine("Grab");
            }
        }
        else if (currentState == LianaState.Flee && Vector3.Distance(player.transform.position, transform.position) <= fightRadius && grabbing == false)
        {
            rb.MovePosition(Vector3.MoveTowards(transform.position, player.transform.position, -lianaSpeed * 1.5f * Time.deltaTime));
            targetRotation.transform.LookAt(player.transform.position);
            targetRotation.transform.rotation = targetRotation.transform.rotation * Quaternion.Euler(0,90,0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation.transform.rotation, rotateSpeed * Time.deltaTime);
            anim.SetTrigger("Flee");
        }

        if ((currentState == LianaState.Fight || currentState == LianaState.Return || currentState == LianaState.Flee) && Vector3.Distance(player.transform.position, homePosition) > fightRadius && Vector3.Distance(homePosition, transform.position) > 2)
        {
            currentState = LianaState.Return;
            rb.MovePosition(Vector3.MoveTowards(transform.position, homePosition, lianaSpeed/2 * Time.deltaTime));
            anim.SetFloat("StateSpeed", -0.5f); 
        }
        else if (currentState == LianaState.Return && Vector3.Distance(homePosition, transform.position) <= 2)
        {
            currentState = LianaState.Sleep;
            anim.SetBool("IsWalking", false);
        }
    
    }

    IEnumerator Grab()
    {
        canGrab = false;
        anim.SetTrigger("Grab");
        yield return new WaitForSeconds(2f);
        grabbing = false;
        yield return new WaitForSeconds(6f);
        canGrab = true;
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1f);
        transform.position = homePosition;
        GetComponent<EnemyLife>().health = GetComponent<EnemyLife>().maxHealth;
        currentState = LianaState.Sleep;
        GetComponent<CapsuleCollider>().enabled = true;
        GetComponent<Rigidbody>().useGravity = true;
        matRenderer.materials = baseMaterials;
        hasDied = false;
        //GetComponentInChildren<MeshRenderer>().enabled = true;
    }

    IEnumerator Stunned()
    {
        if(currentState != LianaState.Dead)
        {
            currentState = LianaState.Stun;
            yield return new WaitForSeconds(0.5f);
            currentState = LianaState.Flee;
            yield return new WaitForSeconds(1f);
            currentState = LianaState.Fight;
        }
    }

    public void GrabAnim()
    {
        grabbing = true;
        GameObject lianaClone;
        lianaClone = Instantiate(grab, transform.position + new Vector3(0,3,0), transform.rotation);
        lianaClone.GetComponent<GrabScript>().lianaEnemy = gameObject;
        lianaClone.GetComponent<Rigidbody>().AddForce(Vector3.Normalize(-(transform.position - player.transform.position)) * grabSpeed * 50);
    }
}
