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

    public bool spike1 = false;
    public bool spike2 = false;
    public bool spike3 = false;
    public bool liana1 = false;
    public bool liana2 = false;
    public bool liana3 = false;
    public bool healer1 = false;
    public bool healer2 = false;
    public bool healer3 = false;
    public bool rock1 = false;
    public bool rock2 = false;
    public bool rock3 = false;
    public bool villager1 = false;
    public bool villager2 = false;
    public bool villager3 = false;
    public bool villager4 = false;
    public bool villager5 = false;
    public bool villager6 = false;
    public bool villager7 = false;
    public bool boss1 = false;
    public bool boss2 = false;
    public bool boss3 = false;
    public bool boss4 = false;

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
