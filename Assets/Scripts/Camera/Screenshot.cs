using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Screenshot : MonoBehaviour
{
    [SerializeField]
    protected string screenshotName;

    [SerializeField]
    private Vector2Int resolution = new Vector2Int(256, 256);

    [Button("Screenshot")]
    public void TakeScreenshot()
    {
        TakeScreenshot(screenshotName);
    }

    public void TakeScreenshot(string name)
    {
        RenderTexture renderTexture = new RenderTexture(resolution.x, resolution.y, 16, RenderTextureFormat.ARGB32);
        GetComponent<Camera>().targetTexture = renderTexture;
        GetComponent<Camera>().Render();

        RenderTexture.active = renderTexture;
        Texture2D texture = new Texture2D(resolution.x, resolution.y, TextureFormat.ARGB32, true);
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();

        byte[] bytes = texture.EncodeToPNG();
        if (!System.IO.Directory.Exists("Screenshots"))
        {
            System.IO.Directory.CreateDirectory("Screenshots");
        }
        System.IO.File.WriteAllBytes($"Screenshots/" + name + ".png", bytes);
        Debug.Log(bytes.Length / 1024 + "Kb was saved as: " + $"Screenshots/" + name + ".png");

        GetComponent<Camera>().targetTexture = null;
    }
}
