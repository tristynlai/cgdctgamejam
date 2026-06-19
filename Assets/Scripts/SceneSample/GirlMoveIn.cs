using UnityEngine;
using Yarn.Unity;

public class GirlMoveIn : MonoBehaviour
{
    public GameObject charGirl;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [YarnCommand("enter")]
    public void Enter() {
        Debug.Log("Enter CALLED on: " + gameObject.name);
        charGirl.SetActive(true);
    }
}
