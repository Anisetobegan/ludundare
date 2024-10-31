using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class SpriteCreator : EditorWindow
{
    RenderTexture renderTexture;
    string spriteName;
    //GameObject renderCamera;
    //RawImage rawImage;

    [MenuItem("Window/Sprite Creator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<SpriteCreator>(); 
    }

    private void OnGUI()
    {
        GUILayout.Label("Sprite Creator", EditorStyles.boldLabel);

        spriteName = EditorGUILayout.TextField("Sprite Name", spriteName);

        renderTexture = EditorGUILayout.ObjectField("Render Camera", renderTexture, typeof(RenderTexture), false) as RenderTexture;

        if (GUILayout.Button("Create Sprite"))
        {            
            TakeScreenShot();
        }
    }

    public void TakeScreenShot()
    {
        if (spriteName == string.Empty)
        {
            Debug.LogError("Empty Sprite Name!");
            return;
        }

        if (renderTexture == null)
        {
            Debug.LogError("No Camera!");
            return;
        }

        byte[] bytes = ToTexture2D(renderTexture).EncodeToPNG();        
        string dirPath = Application.dataPath + "/CreatedSprites";
        if (!System.IO.Directory.Exists(dirPath))
        {
            System.IO.Directory.CreateDirectory(dirPath);
        }
        System.IO.File.WriteAllBytes(dirPath + "/" + spriteName + ".png", bytes);
        Debug.Log("File saved as: " + dirPath + "/" + spriteName + ".png");
    }

    Texture2D ToTexture2D(RenderTexture texture)
    {
        Texture2D newTexture = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false);
        RenderTexture.active = texture;
        newTexture.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
        newTexture.Apply();
        return newTexture;
    }
}
