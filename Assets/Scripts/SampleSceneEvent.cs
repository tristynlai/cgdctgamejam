using UnityEngine;

public class SampleSceneEvent : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerPrefs.SetInt("LoadState", 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
