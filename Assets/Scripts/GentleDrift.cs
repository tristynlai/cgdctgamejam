using UnityEngine;

public class GentleDrift : MonoBehaviour
{

  RectTransform rt;
Vector2 startPos;
    void Start()
    {
        rt = GetComponent<RectTransform>();
        startPos = rt.anchoredPosition;
    }

    void Update()
    {

    }
}