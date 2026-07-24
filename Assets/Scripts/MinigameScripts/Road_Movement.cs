using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road_Movement : MonoBehaviour
{
    public Renderer MeshRenderer;
    public float Speed = 1f;
    public float Max_Speed = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(IncreaseSpeed());
    }

    // Update is called once per frame
    void Update()
    {
        MeshRenderer.material.mainTextureOffset += new Vector2(0, Speed * Time.deltaTime);
    }

    public void ResetSpeed() {
        Speed = 1f;
        StopAllCoroutines();
        StartCoroutine(IncreaseSpeed());
    }

    IEnumerator IncreaseSpeed() {
        while (true) {
            yield return new WaitForSeconds(5);
            if (Max_Speed > Speed ) {
                Speed += 0.1f;
            }
            print("Road Speed: " + Speed);
        }
    }
}
