using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSaver : MonoBehaviour
{
    public bool knowSpike = false;
    public bool knowLiana = false;
    public bool knowHealer = false;
    public bool knowRock = false;
    public bool knowBoss = false;

    public int spikeCharge = 0;
    public int lianaCharge = 0;
    public int healerCharge = 0;
    public int rockCharge = 0;

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
