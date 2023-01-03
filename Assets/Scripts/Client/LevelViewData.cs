using SceneView;
using UnityEngine;
using UnityEngine.Serialization;

namespace Client
{
    [CreateAssetMenu]
    public class LevelViewData : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private Sprite levelResult;

        [Header("Data")] [SerializeField] private ColoringLevelView dirtView;

        [FormerlySerializedAs("coloringColoringView")] [Header("Data")] [SerializeField]
        private ColoringLevelView coloringView;

        public string ID => id;

        public ColoringLevelView DirtView => dirtView;
        public ColoringLevelView ColoringView => coloringView;

        public Sprite LevelResult => levelResult;

#if UNITY_EDITOR
        public void SetData(string id, ColoringLevelView dirt, ColoringLevelView color, Sprite resultSprite)
        {
            this.id = id;
            this.dirtView = dirt;
            this.coloringView = color;
            this.levelResult = resultSprite;
        }
#endif
    }
}