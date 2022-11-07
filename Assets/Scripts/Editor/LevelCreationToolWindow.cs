using System.IO;
using System.Security.Cryptography;
using PaintIn3D;
using SceneView;
using UnityEditor;
using UnityEngine;
using Utils.Client;

namespace Editor
{
    public class LevelCreationToolWindow : EditorWindow
    {
        private const string LevelsFolderPath = "Assets/Configs/Levels";

        private static GameObject levelView;
        private static LevelsViewDataStorage storage;
        private static Material baseDirtMaterial;
        
        private static Sprite colorSprite;
        private static Sprite maskSprite;
        
        private static string levelName;
        
        private bool isInitialized = false;    
        
        [MenuItem ("Tools/LevelCreationTool")]
        private static void ShowWindow()
        {
            SetDefaultData();

            var wind = GetWindow(typeof(LevelCreationToolWindow));
            wind.Show();
        }

        private static void SetDefaultData()
        {
            var levelDataStorage = AssetDatabase.FindAssets("t:LevelsViewDataStorage")[0];
            storage = AssetDatabase.LoadAssetAtPath<LevelsViewDataStorage>(
                AssetDatabase.GUIDToAssetPath(levelDataStorage));

            var material = AssetDatabase.FindAssets("dirt t:Material")[0];
            baseDirtMaterial = AssetDatabase.LoadAssetAtPath<Material>(AssetDatabase.GUIDToAssetPath(material));

            var level = AssetDatabase.FindAssets("LevelViewBase t:Prefab")[0];
            levelView = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(level));
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("LevelsStorage");
            storage = (LevelsViewDataStorage)EditorGUILayout.ObjectField(storage, typeof(LevelsViewDataStorage), true);
            EditorGUILayout.LabelField("Base and dirt material");
            baseDirtMaterial = (Material)EditorGUILayout.ObjectField(baseDirtMaterial, typeof(Material), true);
            EditorGUILayout.LabelField("LevelConfigBasePrefab");
            levelView = (GameObject)EditorGUILayout.ObjectField(levelView, typeof(GameObject), true);

            EditorGUILayout.LabelField("Level name");
            levelName = EditorGUILayout.TextField(levelName);

            EditorGUILayout.LabelField("Color sprite");
            colorSprite = (Sprite)EditorGUILayout.ObjectField(colorSprite, typeof(Sprite), true);
            EditorGUILayout.LabelField("Mask sprite");
            maskSprite = (Sprite)EditorGUILayout.ObjectField(maskSprite, typeof(Sprite), true);

            if (GUILayout.Button("Add level"))
            {
                CreateLevelMaterial();
                
                var path = Path.Combine("Assets\\Prefabs\\Levels", levelName, $"{levelName}Dirt.prefab");

                var levelPref = PrefabUtility.InstantiatePrefab(levelView) as GameObject;
                levelPref.GetComponentInChildren<P3dPaintableTexture>().Texture = maskSprite.texture; 
                levelPref.GetComponentInChildren<P3dChangeCounter>().MaskTexture = maskSprite.texture; 
                levelPref.GetComponentInChildren<P3dChangeCounter>().Texture = maskSprite.texture;
                var maskSavedPref = PrefabUtility.SaveAsPrefabAsset(levelPref, path);
                DestroyImmediate(levelPref);
                
                path = Path.Combine("Assets\\Prefabs\\Levels", levelName, $"{levelName}Color.prefab");

                levelPref = PrefabUtility.InstantiatePrefab(levelView) as GameObject;
                levelPref.GetComponentInChildren<P3dPaintableTexture>().Texture = maskSprite.texture; 
                levelPref.GetComponentInChildren<P3dChangeCounter>().MaskTexture = maskSprite.texture; 
                levelPref.GetComponentInChildren<P3dChangeCounter>().Texture = maskSprite.texture;
                var colorSavedPref = PrefabUtility.SaveAsPrefabAsset(levelPref, path);
                DestroyImmediate(levelPref);
            }
        }

        private static void CreateLevelMaterial()
        {
            var path = Path.Combine("Assets\\Prefabs\\Levels", levelName);
            Material mat = new Material(baseDirtMaterial.shader);
            mat.name = levelName;
            mat.CopyPropertiesFromMaterial(baseDirtMaterial);
            mat.mainTexture = maskSprite.texture;
            AssetDatabase.CreateFolder("Assets\\Prefabs\\Levels", levelName);
            path = Path.Combine(path, $"{mat.name}.mat");
            AssetDatabase.CreateAsset(mat, path);
        }

        private void OnFocus()
        {
            SetDefaultData();
        }
    }
}
