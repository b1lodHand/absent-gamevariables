using com.absence.gamevariables;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class GameVariablesEditorWindow : EditorWindow
{
    public static GameVariableBank TargetBank;
    static Vector2 m_scrollPos;

    Editor m_bankEditor;

    [MenuItem("Game/GameVariables")]
    static void ShowWindow()
    {
        var window = EditorWindow.GetWindow<GameVariablesEditorWindow>();
        window.titleContent = new GUIContent() {
            image = EditorGUIUtility.IconContent("d_UnityEditor.GameView").image,
            text = "GameVariables"
        };
    }

    [OnOpenAsset]
    static bool OnOpenAsset(int instanceId, int line)
    {
        if (Selection.activeObject != TargetBank) return false;

        ShowWindow();
        return true;
    }

    private void OnGUI()
    {
        Undo.RecordObject(this, "GameVariables (Editor)");

        TryToFindDefaultBank();

        if(TargetBank == null)
        {
            if(GUILayout.Button(new GUIContent() { image = EditorGUIUtility.IconContent("d_ToolHandleLocal").image, 
                text = "Create the default bank for GameVariables" }))
            {
                CreateDefaultBank();
            }

            EditorGUILayout.HelpBox("There is no default bank for GameVariables to edit here. Create one with the button above to continue.", MessageType.Warning);
        }

        else
        {
            EditorGUILayout.BeginHorizontal();

            GUI.enabled = false;
            EditorGUILayout.ObjectField("Default Bank: ", TargetBank, typeof(GameVariableBank), allowSceneObjects: false);
            GUI.enabled = true;

            if (GUILayout.Button(new GUIContent { image = EditorGUIUtility.IconContent("d_Toolbar Minus").image }))
            {
                DestroyDefaultBank();
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorUtility.SetDirty(this);

        if (TargetBank == null) return;

        Undo.RecordObject(TargetBank, "GameVariables (Bank)");

        m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos);

        if (!m_bankEditor) Editor.CreateCachedEditor(TargetBank, null, ref m_bankEditor);
        else m_bankEditor.OnInspectorGUI();

        EditorGUILayout.EndScrollView();

        EditorUtility.SetDirty(TargetBank);
    }

    private void DestroyDefaultBank()
    {
        Undo.DestroyObjectImmediate(TargetBank);
        TargetBank = null;
        m_bankEditor = null;
    }

    private void TryToFindDefaultBank()
    {
        var foundGuid = AssetDatabase.FindAssets("t:GameVariableBank l:GameVariables").ToList().FirstOrDefault();
        if (foundGuid != null) TargetBank =
                AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(foundGuid), typeof(GameVariableBank)) as GameVariableBank;
        else TargetBank = null;
    }

    private void CreateDefaultBank()
    {
        var bankCreated = ScriptableObject.CreateInstance<GameVariableBank>();
        AssetDatabase.CreateAsset(bankCreated, "Assets/Resources/GameVariables.asset");
        AssetDatabase.SetLabels(bankCreated, new string[] { "GameVariables" });
        AssetDatabase.Refresh();

        TargetBank = bankCreated;
    }
}
