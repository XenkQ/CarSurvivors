using UnityEditor;
using UnityEngine;
using Assets.Scripts.LevelSystem;

[CustomEditor(typeof(LevelController))]
public class LevelControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelController controller = (LevelController)target;

        GUILayout.Space(10);
        GUILayout.Label("Level Debug Actions", EditorStyles.boldLabel);

        if (GUILayout.Button("Add 20 Exp"))
        {
            controller.AddExp(20f);
        }

        if (GUILayout.Button("Add 220 Exp"))
        {
            controller.AddExp(220f);
        }

        if (GUILayout.Button("Add 22B Exp"))
        {
            controller.AddExp(22000000000f);
        }
    }
}
