using com.absence.variablesystem;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace com.absence.gamevariables.Editor
{
    public class GameVariablesEditorWindow : EditorWindow
    {
        public static VariableBank TargetBank;
        static Vector2 m_scrollPos;

        UnityEditor.Editor m_bankEditor;

        [MenuItem("absencee_/absent-gamevariables/Open GameVariables Window")]
        static void ShowWindow()
        {
            var window = EditorWindow.GetWindow<GameVariablesEditorWindow>();
            window.titleContent = new GUIContent()
            {
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

            if (TargetBank == null)
            {
                if (GUILayout.Button(new GUIContent()
                {
                    image = EditorGUIUtility.IconContent("d_ToolHandleLocal").image,
                    text = "Create the default bank for GameVariables"
                }))
                {
                    CreateDefaultBank();
                }

                EditorGUILayout.HelpBox("There is no default bank for GameVariables to edit here. Create one with the button above to continue.", MessageType.Warning);
            }

            else
            {
                EditorGUILayout.BeginHorizontal();

                GUI.enabled = false;
                EditorGUILayout.ObjectField("Default Bank: ", TargetBank, typeof(VariableBank), allowSceneObjects: false);
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

            if (!m_bankEditor) UnityEditor.Editor.CreateCachedEditor(TargetBank, null, ref m_bankEditor);
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
                    AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(foundGuid), typeof(VariableBank)) as VariableBank;
            else TargetBank = null;
        }

        private void CreateDefaultBank()
        {
            var bankCreated = ScriptableObject.CreateInstance<VariableBank>();
            AssetDatabase.CreateAsset(bankCreated, "Assets/Resources/GameVariables.asset");
            AssetDatabase.SetLabels(bankCreated, new string[] { "GameVariables" });
            AssetDatabase.Refresh();

            TargetBank = bankCreated;
        }
    }
}
