using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LianaWall : MonoBehaviour
{
    Renderer matRenderer;
    [SerializeField] Material dissolveMat;
    float dissolveAmount = 0f;
    bool dissolving = false;
    [SerializeField] AudioClip audioCut;
    // Start is called before the first frame update
    void Start()
    {
        matRenderer = GetComponentInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(dissolving)
        {
            dissolveAmount = Mathf.Lerp(dissolveAmount, 1, 0.05f);
            matRenderer.material.SetFloat("_DissolveAmount", dissolveAmount);
        }
    }

    public void Fade()
    {
        matRenderer.material = dissolveMat;
        dissolveAmount = 0f;
        dissolving = true;
        GetComponent<BoxCollider>().isTrigger = true;
        GetComponent<AudioSource>().clip = audioCut;
        GetComponent<AudioSource>().Play();
        Destroy(this.gameObject, 3);
    }
}
