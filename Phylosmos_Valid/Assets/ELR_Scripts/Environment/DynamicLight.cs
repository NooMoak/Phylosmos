using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicLight : MonoBehaviour
{
    Light lt;
    float intensityFloat;
    float rangeFloat;
    
    
    [SerializeField] float minimumRange;
    [SerializeField] float maximumRange;
    [SerializeField] float rangeSpeed;

    [SerializeField] float minimumIntensity;
    [SerializeField] float maximumIntensity;
    [SerializeField] float intensitySpeed;
    // Start is called before the first frame update
    void Start()
    {
        lt = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        //rangeFloat = Mathf.PingPong(Time.time * rangeSpeed, maximumRange - minimumRange);
        //lt.range = rangeFloat + minimumRange;
        lt.range = Random.Range(minimumRange, maximumRange);

        //intensityFloat = Mathf.PingPong(Time.time * intensitySpeed, maximumIntensity - minimumIntensity);
        //lt.intensity = intensityFloat + minimumIntensity;
        lt.intensity = Random.Range(minimumIntensity, maximumIntensity);
    }
}
