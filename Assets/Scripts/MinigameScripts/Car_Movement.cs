using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Movement : MonoBehaviour
{
    public Transform transform;
    public float Speed = 4f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        //Speed = 4f;
         transform = GetComponent<Transform>();
         StartCoroutine(IncreaseSpeed());
    }

    // Update is called once per frame
    void Update() {
        transform.position -= new Vector3(0, Speed * Time.deltaTime, 0);

        if(transform.position.y < -6) {
            Destroy(gameObject);
        }
    }

    IEnumerator IncreaseSpeed() {
        while (true) {
            yield return new WaitForSeconds(5);
            Speed += 0.4f;
        }
    }
}
