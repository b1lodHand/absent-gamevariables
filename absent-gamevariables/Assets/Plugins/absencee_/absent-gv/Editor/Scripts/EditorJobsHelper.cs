using com.absence.gamevariables.internals;
using com.absence.variablesystem.editor;
using UnityEditor;
using UnityEngine;

namespace com.absence.gamevariables.editor
{
    /// <summary>
    /// The static class responsible for helping the editor-side things of this system.
    /// </summary>
    [InitializeOnLoad]
    [DefaultExecutionOrder(-101)]
    public static class EditorJobsHelper
    {
        static EditorJobsHelper()
        {
            string guid = EditorPrefs.GetString(Constants.GAMEVARIABLES_GUID_PREF_NAME, string.Empty);
            GameVariables.EditorBankGuid = guid;

            if (GameVariablesBootstrapper.Initialized) VariableBankDatabase.Refresh();
            else
            {
                GameVariablesBootstrapper.OnUnloadResources -= VariableBankDatabase.Refresh;
                GameVariablesBootstrapper.OnUnloadResources += VariableBankDatabase.Refresh;
            }
        }
    }

}