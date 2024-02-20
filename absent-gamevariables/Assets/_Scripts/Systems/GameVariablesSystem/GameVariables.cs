
using UnityEngine;

namespace com.absence.gamevariables
{
    public static class GameVariables
    {
        private static readonly string ACTIVE_BANK_PATH = "GameVariables";

        private static GameVariableBank m_activeVariableBank = null;
        private static GameVariableBank ActiveVariableBank
        {
            get
            {
#if UNITY_EDITOR
                throw new System.Exception("You cannot use GameVariables class in the editor!");
#else
            return m_activeVariableBank;
#endif
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        static void GetVariableBank()
        {
            m_activeVariableBank = Resources.Load(ACTIVE_BANK_PATH) as GameVariableBank;
            Debug.Log(m_activeVariableBank.name);
        }

        public static int? GetInt(string variableName) => ActiveVariableBank.GetInt(variableName);
        public static float? GetFloat(string variableName) => ActiveVariableBank.GetFloat(variableName);
        public static string GetString(string variableName) => ActiveVariableBank.GetString(variableName);
        public static bool? GetBoolean(string variableName) => ActiveVariableBank.GetBoolean(variableName);

        public static bool SetInt(string variableName, int newValue) => ActiveVariableBank.SetInt(variableName, newValue);
        public static bool SetFloat(string variableName, float newValue) => ActiveVariableBank.SetFloat(variableName, newValue);
        public static bool SetString(string variableName, string newValue) => ActiveVariableBank.SetString(variableName, newValue);
        public static bool SetBoolean(string variableName, bool newValue) => ActiveVariableBank.SetBoolean(variableName, newValue);

        public static bool HasInt(string variableName) => ActiveVariableBank.HasInt(variableName);
        public static bool HasFloat(string variableName) => ActiveVariableBank.HasFloat(variableName);
        public static bool HasString(string variableName) => ActiveVariableBank.HasString(variableName);
        public static bool HasBoolean(string variableName) => ActiveVariableBank.HasBoolean(variableName);
        public static bool HasAny(string variableName) => ActiveVariableBank.HasAny(variableName);
    }

}