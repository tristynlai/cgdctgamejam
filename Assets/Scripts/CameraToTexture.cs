using UnityEngine;

public class CameraToTexture : MonoBehaviour
{
    public RenderTexture renderTexture;

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        if (cam != null && renderTexture != null)
        {
            cam.targetTexture = renderTexture;
        }
    }
}
