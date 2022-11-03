using System.IO;
using SceneView;
using UnityEditor;
using UnityEngine;
using Utils.Client;

namespace Editor
{
    public class LevelCreationToolWindow : EditorWindow
    {
        private const string LevelsFolderPath = "Assets/Configs/Levels";

        private static ColoringLevelView levelView;
        private static LevelsViewDataStorage storage;
        private static Material baseDirtMaterial;
        
        private static Sprite colorSprite;
        private static Sprite maskSprite;
        
        private static string levelName;
        
        private bool isInitialized = false;    
        
        [MenuItem ("Tools/LevelCreationTool")]
        private static void ShowWindow()
        {
            var levelDataStorage = AssetDatabase.FindAssets("t:LevelsViewDataStorage")[0];
            storage = AssetDatabase.LoadAssetAtPath<LevelsViewDataStorage>(AssetDatabase.GUIDToAssetPath(levelDataStorage));

            var material = AssetDatabase.FindAssets("dirt t:Material")[0];
            baseDirtMaterial = AssetDatabase.LoadAssetAtPath<Material>(AssetDatabase.GUIDToAssetPath(material));

            var level = AssetDatabase.FindAssets("LevelViewBase t:Prefab")[0];
            levelView = AssetDatabase.LoadAssetAtPath<ColoringLevelView>(AssetDatabase.GUIDToAssetPath(level));
            
            var wind = GetWindow(typeof(LevelCreationToolWindow));
            wind.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("LevelsStorage");
            storage = (LevelsViewDataStorage)EditorGUILayout.ObjectField(storage, typeof(LevelsViewDataStorage), true);
            EditorGUILayout.LabelField("Base and dirt material");
            baseDirtMaterial = (Material)EditorGUILayout.ObjectField(baseDirtMaterial, typeof(Material), true);
            EditorGUILayout.LabelField("LevelConfigBasePrefab");
            levelView = (ColoringLevelView)EditorGUILayout.ObjectField(levelView, typeof(ColoringLevelView), true);

            EditorGUILayout.LabelField("Level name");
            levelName = EditorGUILayout.TextField(levelName);

            EditorGUILayout.LabelField("Color sprite");
            colorSprite = (Sprite)EditorGUILayout.ObjectField(colorSprite, typeof(Sprite), true);
            EditorGUILayout.LabelField("Mask sprite");
            maskSprite = (Sprite)EditorGUILayout.ObjectField(maskSprite, typeof(Sprite), true);

            if (GUILayout.Button("Add level"))
            {
                var path = Path.Combine("Assets/Configs/Levels", levelName);
                Material mat = new Material(baseDirtMaterial.shader);
                mat.CopyPropertiesFromMaterial(baseDirtMaterial);
                mat.name = levelName+"_color";
                AssetDatabase.CreateFolder("Assets/Configs/Levels", levelName);
                AssetDatabase.CreateAsset(mat, path);
                // PrefabUtility.SaveAsPrefabAssetAndConnect()
            }
        }
    }
}
