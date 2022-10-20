using SceneView;
using UnityEngine;

namespace Utils.Client
{
    [CreateAssetMenu]
    public class LevelViewData : ScriptableObject
    {
        [SerializeField] private string id;

        [Header("Data")] [SerializeField] private ColoringLevelView dirtView;
        [Header("Data")] [SerializeField] private ColoringLevelView coloringColoringView;

        public string ID => id;

        public ColoringLevelView DirtView => dirtView;
        public ColoringLevelView ColoringView => coloringColoringView;
    }
}