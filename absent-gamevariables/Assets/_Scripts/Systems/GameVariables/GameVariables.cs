
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

        public static bool TryGetInt(string variableName, out GameVariable_Integer value)
        {
            value = null;

            var found = ActiveVariableBank.GetInt(variableName);
            if (found == null) return false;

            value = found;
            return true;
        }
        public static bool TryGetFloat(string variableName, out GameVariable_Float value)
        {
            value = null;

            var found = ActiveVariableBank.GetFloat(variableName);
            if (found == null) return false;

            value = found;
            return true;
        }
        public static bool TryGetString(string variableName, out GameVariable_String value)
        {
            value = null;

            var found = ActiveVariableBank.GetString(variableName);
            if (found == null) return false;

            value = found;
            return true;
        }
        public static bool TryGetBoolean(string variableName, out GameVariable_Boolean value)
        {
            value = null;

            var found = ActiveVariableBank.GetBoolean(variableName);
            if (found == null) return false;

            value = found;
            return true;
        }

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