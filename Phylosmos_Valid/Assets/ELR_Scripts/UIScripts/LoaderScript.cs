using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadScene());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(11);
        AsyncOperation async = SceneManager.LoadSceneAsync("ELR_NewTuto");
        while (!async.isDone)
        {
            yield return null;
            Debug.Log("loading");
        }
    }
}
