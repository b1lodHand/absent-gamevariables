using com.absence.variablesystem;
using System;
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

        /// <summary>
        /// The action which will get invoked when this class unloads the GameVariables bank asset after loading and cloning it.
        /// </summary>
        public static event Action OnUnloadResources = null;

        static bool m_initialized = false;

        /// <summary>
        /// Use to check if the <see cref="GameVariables"/> is initialized successfully.
        /// </summary>
        public static bool Initialized => m_initialized;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        static void Initialize()
        {
            m_initialized = false;

            VariableBank loadedBank = Resources.Load<VariableBank>(Constants.GAMEVARIABLES_BANK_PATH_RUNTIME);

            if (loadedBank == null)
            {
                Resources.UnloadAsset(loadedBank);
                Debug.LogWarning("There are no banks for GameVariables found in the project.");
                return;
            }

            VariableBank clonedBank = (loadedBank as VariableBank).Clone();
            Resources.UnloadAsset(loadedBank);

            OnUnloadResources?.Invoke();
            OnUnloadResources = null;

            GameVariables.SetRuntimeBank(clonedBank);

            GameVariablesSceneEventsHandler gameVariablesSceneEventsHandler = new GameObject("GV Scene Events Handler [absent-gv]").
                AddComponent<GameVariablesSceneEventsHandler>();

            GameObject.DontDestroyOnLoad(gameVariablesSceneEventsHandler);

            m_initialized = true;

            if (DEBUG_MODE) Debug.Log("<color=white>GameVariables initialized successfully.</color>");
        }
    }

}