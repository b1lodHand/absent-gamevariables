using com.absence.variablebanks;
using com.absence.variablebanks.internals;
using com.absence.variablesystem;
using UnityEngine;

namespace com.absence.gamevariables.internals
{
    /// <summary>
    /// The static class responsible for initializing the <see cref="GameVariables"/> at the splash screen.
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public static class GameVariablesBootstrapper
    {
        const bool DEBUG_MODE = true;

        static bool m_initialized = false;

        /// <summary>
        /// Use to check if the <see cref="GameVariables"/> is initialized successfully.
        /// </summary>
        public static bool Initialized => m_initialized;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        static void Initialize()
        {
            m_initialized = false;

            VariableBanksCloningHandler.AddCloningCompleteCallbackOrInvoke(() =>
            {
                VariableBank clonedBank = VariableBankManager.GetInstance(GameVariables.EditorBankGuid);

                if (clonedBank == null)
                {
                    Debug.LogWarning("There are no banks for GameVariables found in the project.");
                    return;
                }

                GameVariables.SetRuntimeBank(clonedBank);

                GameVariablesSceneEventsHandler gameVariablesSceneEventsHandler = new GameObject("GV Scene Events Handler [absent-gv]").
                AddComponent<GameVariablesSceneEventsHandler>();

                GameObject.DontDestroyOnLoad(gameVariablesSceneEventsHandler);

                m_initialized = true;

                if (DEBUG_MODE) Debug.Log("<color=white>GameVariables initialized successfully.</color>");
            });
        }
    }

}