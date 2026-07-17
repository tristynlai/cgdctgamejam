using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Yarn.Unity;
using UnityEngine.UI;

public class CustomWaitCommand : MonoBehaviour {    

    
    [YarnCommand("custom_wait")]
    public static IEnumerator CustomWait() {

        //Debug.Log("CustomWait CALLED on: " + gameObject.name);

        // Wait for 1 second
        yield return new WaitForSeconds(3);
        
        // Because this method returns IEnumerator, it's a coroutine. 
        // Yarn Spinner will wait until onComplete is called.
    }    
}