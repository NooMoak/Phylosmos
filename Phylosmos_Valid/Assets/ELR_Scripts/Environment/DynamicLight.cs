using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicLight : MonoBehaviour
{
    Light lt;
    float intensityFloat;
    float rangeFloat;
    float intensityTarget;
    float rangeTarget;
    
    
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
        if(lt.range == rangeTarget){
            rangeTarget = Random.Range(minimumRange, maximumRange);
        }
        else
        {
            lt.range = Mathf.Lerp(lt.range, rangeTarget, 0.2f);
        }

        //intensityFloat = Mathf.PingPong(Time.time * intensitySpeed, maximumIntensity - minimumIntensity);
        //lt.intensity = intensityFloat + minimumIntensity;
        if(lt.intensity == intensityTarget)
        {
            intensityTarget = Random.Range(minimumIntensity, maximumIntensity);
        }
        else
        {
            lt.intensity = Mathf.Lerp(lt.intensity, intensityTarget, 0.2f);
        }
    }
}
