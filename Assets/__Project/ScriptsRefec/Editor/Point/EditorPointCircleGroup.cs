#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace PandaIsPanda
{
    [CustomEditor(typeof(PointCircleGroup))]
    public class EditorPointCircleGroup : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(10);
            GUILayout.Label("# Actions");
            if (!GUILayout.Button("Spread"))
                return;
            
            Spread();
        }

        private void Spread()
        {
            if (target is not PointCircleGroup cornerGroup)
                return;
            
            cornerGroup.Spread();
            
            EditorUtility.SetDirty(cornerGroup);
        }
    }
}
#endif