using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornados : MonoBehaviour
{
    float timer;
    float distance;
    float tornardosSpeed = 20f;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        distance = Vector3.Distance(transform.position, player.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.GetChild(0).transform.position, transform.GetChild(1).transform.position) > 5)
        {
            GetComponent<Rigidbody>().MovePosition(Vector3.MoveTowards(transform.position, player.transform.position, tornardosSpeed * Time.deltaTime));
            transform.GetChild(1).transform.position += new Vector3(Mathf.Sin(-timer) * 0.5f, 0, Mathf.Cos(-timer) * 0.5f);
            transform.GetChild(0).transform.position += new Vector3(Mathf.Sin(-timer) * 0.5f, 0, -(Mathf.Cos(-timer)) * 0.5f);
            timer += 1 / (distance / 2);
        }
    }
}
