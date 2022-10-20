using System.Collections.Generic;
using System.Linq;
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
    }
}