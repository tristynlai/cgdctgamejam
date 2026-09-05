using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road_Movement : MonoBehaviour
{
    public Renderer MeshRenderer;
    public float Speed = 1f;
    public float Max_Speed = 2f;
    private Material roadMaterial;

    void Start()
    {
        if (MeshRenderer != null)
        {
            roadMaterial = MeshRenderer.material;
        }
        StartCoroutine(IncreaseSpeed());
    }

    void Update()
    {
        if (roadMaterial != null)
        {
            roadMaterial.mainTextureOffset += new Vector2(0, Speed * Time.deltaTime);
        }
    }

    public void ResetSpeed() 
    {
        Speed = 1f;
        StopAllCoroutines();
        StartCoroutine(IncreaseSpeed());
    }

    IEnumerator IncreaseSpeed() 
    {
        while (true) {
            yield return new WaitForSeconds(5);
            if (Max_Speed > Speed) {
                Speed += 0.1f;
            }
            print("Road Speed: " + Speed);
        }
    }
}
