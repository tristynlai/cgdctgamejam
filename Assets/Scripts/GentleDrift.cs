using UnityEngine;

public class GentleDrift : MonoBehaviour
{
    public float speed = 10f; 
  RectTransform rt;
Vector2 startPos;
    void Start()
    {
        rt = GetComponent<RectTransform>();
        startPos = rt.anchoredPosition;
    }

void Update()
    {
        Vector2 pos = rt.anchoredPosition;
        pos.x += 10f * Time.deltaTime;

        if (pos.x >= startPos.x + 1920f)
        {
            pos.x -= 1920f;
        }

        rt.anchoredPosition = pos;
    }
}