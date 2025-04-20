using UnityEngine;
using UnityEditor;
using System.IO;

public class PlanMaterialGenerator
{
    [MenuItem("Tools/Generate PlanMAT Variants")]
    public static void GenerateVariants()
    {
        string sourcePath = "Assets/Materials/PlanMAT.mat";
        string targetDir = "Assets/Materials/GeneratedMaterials/PlanVariants/";

        if (!Directory.Exists(targetDir))
            Directory.CreateDirectory(targetDir);

        Material baseMat = AssetDatabase.LoadAssetAtPath<Material>(sourcePath);
        if (baseMat == null)
        {
            Debug.LogError("Base material not found at: " + sourcePath);
            return;
        }

        Color[] colors = new Color[]
        {
            new Color32(15, 255, 0, 255),
            new Color32(255, 255, 0, 255),
            new Color32(255, 223, 0, 255),
            new Color32(255, 191, 0, 255),
            new Color32(255, 159, 0, 255),
            new Color32(255, 127, 0, 255),
            new Color32(255, 95, 0, 255),
            new Color32(255, 63, 0, 255),
            new Color32(255, 31, 0, 255),
            new Color32(255, 0, 0, 255)
        };

        for (int i = 0; i < colors.Length; i++)
        {
            Material newMat = new Material(baseMat);
            newMat.SetColor("_Colour", colors[i]); // If your shader uses a different name, update this

            string name = $"PlanMAT{i:D2}";
            string assetPath = $"{targetDir}{name}.mat";

            AssetDatabase.CreateAsset(newMat, assetPath);
            Debug.Log($"Created {name} with color {colors[i]}");
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
