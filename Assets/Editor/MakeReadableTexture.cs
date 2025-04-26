using UnityEngine;
using UnityEditor;
using System.IO;

public class MakeWritableSprite
{
    [MenuItem("Tools/Make Selected Sprite Writable")]
    public static void MakeSelectedSpriteWritable()
    {
        Object selected = Selection.activeObject;
        if (selected == null || !(selected is Sprite))
        {
            Debug.LogWarning("Please select a Sprite in the Project view.");
            return;
        }

        Sprite sprite = (Sprite)selected;
        Texture2D originalTex = sprite.texture;

        // Duplicate texture into readable format
        RenderTexture rt = RenderTexture.GetTemporary(originalTex.width, originalTex.height, 0);
        Graphics.Blit(originalTex, rt);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D readableTex = new Texture2D(originalTex.width, originalTex.height, TextureFormat.RGBA32, false);
        readableTex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        readableTex.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(rt);

        // Encode as PNG and save
        byte[] pngData = readableTex.EncodeToPNG();
        string path = AssetDatabase.GetAssetPath(originalTex);
        string writablePath = Path.GetDirectoryName(path) + "/" + Path.GetFileNameWithoutExtension(path) + "_writable.png";

        File.WriteAllBytes(writablePath, pngData);
        AssetDatabase.Refresh();

        // Set import settings: Sprite + Read/Write
        TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(writablePath);
        importer.textureType = TextureImporterType.Sprite;
        importer.isReadable = true;
        importer.SaveAndReimport();

        Debug.Log("Writable sprite created at: " + writablePath);
    }
}
