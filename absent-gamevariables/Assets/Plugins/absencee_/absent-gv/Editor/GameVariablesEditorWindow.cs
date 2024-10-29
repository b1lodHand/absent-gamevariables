using com.absence.gamevariables.internals;
using com.absence.variablebanks.editor;
using com.absence.variablesystem.banksystembase;
using com.absence.variablesystem.banksystembase.editor;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace com.absence.gamevariables.editor
{
    /// <summary>
    /// The static class represents the editor window of <see cref="GameVariables"/>.
    /// </summary>
    public class GameVariablesEditorWindow : EditorWindow
    {
        static VariableBank m_targetBank;
        static Vector2 m_scrollPos;

        static Editor m_bankEditor; 

        [MenuItem("absencee_/absent-gv/Open GameVariables Window")]
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
            TryToFindDefaultBank();
            if (Selection.activeObject != m_targetBank) return false;

            ShowWindow();
            return true;
        }

        private void OnGUI()
        {       
            Undo.RecordObject(this, "GameVariables (Editor)");

            if (m_targetBank == null && !Application.isPlaying) TryToFindDefaultBank();
            else if (Application.isPlaying) m_targetBank = GameVariables.GetRuntimeBank();

            EditorGUI.BeginChangeCheck();

            if (m_targetBank == null)
            {
                if (GUILayout.Button(new GUIContent()
                {
                    image = EditorGUIUtility.IconContent("d_ToolHandleLocal").image,
                    text = "Create the default bank for GameVariables"
                }))
                {
                    CreateDefaultBank();
                }

                EditorGUILayout.HelpBox("There is no bank for GameVariables to edit here. Create one with the button above to continue.", MessageType.Warning);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                GUI.enabled = false;
                EditorGUILayout.ObjectField("GameVariables Bank: ", m_targetBank, typeof(VariableBank), allowSceneObjects: false);
                GUI.enabled = true;

                if (Application.isPlaying) GUI.enabled = false;

                Color normalColor = GUI.backgroundColor;
                //GUI.backgroundColor = new Color(171f / 255f, 68f / 255f, 63f / 255f, 1f);
                GUI.backgroundColor = Color.red;

                GUIStyle buttonStyle = new(GUI.skin.button);

                if (GUILayout.Button(new GUIContent { tooltip = "Delete GameVariables bank.", image = EditorGUIUtility.IconContent("d_Toolbar Minus").image }, buttonStyle, GUILayout.ExpandWidth(false), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                {
                    OpenDestroyDialog();
                }

                GUI.backgroundColor = normalColor;
                GUI.enabled = true;

                EditorGUILayout.EndHorizontal();
            }

            if (EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(this);

            if (m_targetBank == null) return;

            Undo.RecordObject(m_targetBank, "GameVariables (Bank)");

            EditorGUI.BeginChangeCheck();

            m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos);

            if (Application.isPlaying)
            {
                EditorGUILayout.HelpBox("Editing is not supported in play mode. But you can see any changes made to GameVariables realtime here.", MessageType.Info);
                GUI.enabled = false;
            }

            try
            {
                if (m_bankEditor == null) Editor.CreateCachedEditor(m_targetBank, null, ref m_bankEditor);
                else if (m_bankEditor.serializedObject == null) Editor.CreateCachedEditor(m_targetBank, null, ref m_bankEditor);
                else if (m_bankEditor.serializedObject.targetObject == null) Editor.CreateCachedEditor(m_targetBank, null, ref m_bankEditor);
                else m_bankEditor.OnInspectorGUI();
            }

            catch
            {
                Editor.CreateCachedEditor(m_targetBank, null, ref m_bankEditor);
            }

            GUI.enabled = true;

            EditorGUILayout.EndScrollView();

            if(EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(m_targetBank);

        }

        private void OpenDestroyDialog()
        {
            bool destroy = EditorUtility.DisplayDialog("Delete GameVariables bank?", 
                "You are deleting the bank associated with absent-gv. Are you sure you want to progress?" +
                "\n\n(You can't undo this action.)", "Delete", "Cancel");

            if (!destroy) return;

            DestroyDefaultBank();
        }

        private static void DestroyDefaultBank()
        {
#if ABSENT_VB_ADDRESSABLES
            AssetDatabase.DeleteAsset(Constants.ADDRESSABLES_FULL_PATH);
#else
            AssetDatabase.DeleteAsset(Constants.RESOURCES_FULL_PATH);
#endif

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            VariableBankDatabase.Refresh();

            EditorPrefs.SetString(Constants.GAMEVARIABLES_GUID_PREF_NAME, string.Empty);
            GameVariables.EditorBankGuid = string.Empty;
            m_targetBank = null;
            m_bankEditor = null;
        }

        private static void TryToFindDefaultBank()
        {
            string targetGuid = EditorPrefs.GetString(Constants.GAMEVARIABLES_GUID_PREF_NAME);
            if (GameVariables.EditorBankGuid != targetGuid) GameVariables.EditorBankGuid = targetGuid;

            if (string.IsNullOrWhiteSpace(targetGuid)) return;

            VariableBankDatabase.Refresh();
            VariableBank foundBank = VariableBankDatabase.GetBankIfExists(targetGuid);

            if (foundBank != null)
            {
                m_targetBank = foundBank;
                EditorPrefs.SetString(Constants.GAMEVARIABLES_GUID_PREF_NAME, foundBank.Guid);
                GameVariables.EditorBankGuid = foundBank.Guid;
            }
            else
            {
                EditorPrefs.SetString(Constants.GAMEVARIABLES_GUID_PREF_NAME, string.Empty);
                GameVariables.EditorBankGuid = string.Empty;
            }
        }

        private static void CreateDefaultBank()
        {
            string path;

#if ABSENT_VB_ADDRESSABLES
            if (!AssetDatabase.IsValidFolder(Constants.ADDRESSABLES_PATH))
                AssetDatabase.CreateFolder("Assets", "Scriptables");

            if (!AssetDatabase.IsValidFolder($"{Constants.ADDRESSABLES_PATH}/{Constants.SUBFOLDER_NAME}"))
                AssetDatabase.CreateFolder(Constants.ADDRESSABLES_PATH, Constants.SUBFOLDER_NAME);

            path = Constants.ADDRESSABLES_FULL_PATH;
            VariableBankCreationHandler.CreateVariableBankAtPath(path, false, true, ApplyCreation);
#else
            if (!AssetDatabase.IsValidFolder(Constants.RESOURCES_PATH)) 
                AssetDatabase.CreateFolder("Assets", "Resources");

            if (!AssetDatabase.IsValidFolder($"{Constants.RESOURCES_PATH}/{Constants.SUBFOLDER_NAME}"))
                AssetDatabase.CreateFolder(Constants.RESOURCES_PATH, Constants.SUBFOLDER_NAME);

            path = Constants.RESOURCES_FULL_PATH;
            VariableBankCreationHandler.CreateVariableBankAtPath(path, false, false, ApplyCreation);
#endif

            return;

            void ApplyCreation(VariableBank bankCreated, bool success) 
            {
                if (!success)
                {
                    VariableBankDatabase.Refresh();
                    m_targetBank = null;
                }

                AssetDatabase.SetLabels(bankCreated, new string[] { "GameVariables" });
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

                EditorPrefs.SetString(Constants.GAMEVARIABLES_GUID_PREF_NAME, bankCreated.Guid);
                GameVariables.EditorBankGuid = bankCreated.Guid;

                VariableBankDatabase.Refresh();

                m_targetBank = bankCreated;
            }
        }
    }
}

