using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornados : MonoBehaviour
{
    float timer;
    [SerializeField] float tornardosSpeed = 20f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.GetChild(0).transform.localRotation = Quaternion.Euler(new Vector3(0, timer, 0));
        transform.GetChild(1).transform.localRotation = Quaternion.Euler(new Vector3(0, -timer, 0));
        timer += 1f * tornardosSpeed;
    }
}
