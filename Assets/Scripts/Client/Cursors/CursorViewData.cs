using Client.Audio;
using UnityEngine;

namespace Client.Cursors
{
    [CreateAssetMenu(menuName = "CursorViewData", fileName = "CursorViewData", order = 0)]
    public class CursorViewData : ScriptableObject
    {
        [SerializeField] private CursorView view;
        [SerializeField] private AudioClipName clipName;

        public AudioClipName ClipName => clipName;
        public CursorView View => view;
    }
}