using UnityEngine;

namespace com.absence.gamevariables.internals
{
    /// <summary>
    /// A small singleton component that provides you a base for scene based event handling of the <see cref="GameVariables"/> package.
    /// </summary>
    public class GameVariablesSceneEventsHandler : MonoBehaviour
    {
        private static GameVariablesSceneEventsHandler m_instance;
        public static GameVariablesSceneEventsHandler Instance => m_instance;

        private void Awake()
        {
            if (m_instance != null && m_instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }
    }

}