using System;
using System.Collections.Generic;
using System.Linq;
using Editor;
using UnityEditor;
using UnityEngine;

namespace Utils.Client
{
    [CreateAssetMenu]
    public class LevelsViewDataStorage : ScriptableObject, IBootstrapable
    {
        [SerializeField] private List<LevelViewData> levels;

        private Dictionary<string, LevelViewData> levelsDict;

        public List<string> levelsList => levelsDict.Keys.ToList();

        public LevelViewData GetData(string levelId)
        {
            return levelsDict[levelId];
        }

        public void Bootstrap() 
        {
            levelsDict = new Dictionary<string, LevelViewData>(levels.Count);
            foreach (var lvl in levels)
            {
                levelsDict[lvl.ID] = lvl;
            }
        }

#if UNITY_EDITOR
        public bool LevelIdExists(string id)
        {
            return levels.Any(lvl => lvl.ID == id);
        }

        public void AddLevel(LevelViewData level)
        {
            if (LevelIdExists(level.ID))
            {
                throw new LevelExistsException(level.ID);
                return;
            }
            
            levels.Add(level);
            EditorUtility.SetDirty(this);
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }
#endif
    }
}