using com.absence.variablesystem.banksystembase;
using com.absence.variablesystem.internals;
using System;
using UnityEngine;

namespace com.absence.gamevariables
{
    /// <summary>
    /// The static wrapper class.
    /// </summary>
    public static class GameVariables
    {
        [SerializeField] private static VariableBank m_bank;
        [SerializeField] private static string m_editorBankGuid;

        /// <summary>
        /// Use to get/set the Guid of the bank set in the editor for GameVariables (cannot set runtime).
        /// </summary>
        public static string EditorBankGuid
        {
            get
            {
                return m_editorBankGuid;
            }

            set
            {
                if (Application.isPlaying) throw new Exception("You cannot change editor bank of GameVariables in play mode.");

                m_editorBankGuid = value;
            }
        }

        /// <summary>
        /// Use to set the runtime (cloned) bank of the GameVariables (only works in play mode).
        /// </summary>
        /// <param name="newBank">The new bank to set. You cannot pass null.</param>
        public static void SetRuntimeBank(VariableBank newBank)
        {
            if (!Application.isPlaying) throw new Exception("You can set GameVariables' bank only in play mode.");
            if (newBank == null) throw new Exception("GameVariables' bank cannot get set to null.");

            m_bank = newBank;
        }

        /// <summary>
        /// Use to get the runtime (cloned) bank of the GameVariables (only works in play mode).
        /// </summary>
        /// <returns>Returns the GameVariables' bank. Won't return null when the bank is null, throws an error instead.</returns>
        public static VariableBank GetRuntimeBank()
        {
            if (!Application.isPlaying) throw new Exception("You can get GameVariables' bank only in play mode. Use GameVariables.EditorBankGuid instead.");

            return m_bank;
        }


        /// <summary>
        /// Use to set an integer.
        /// </summary>
        /// <param name="variableName">Target variable name.</param>
        /// <param name="newValue">New value for the target variable.</param>
        /// <returns>False if anything went wrong, true otherwise.</returns>
        public static bool SetInt(string variableName, int newValue) => m_bank.SetInt(variableName, newValue);

        /// <summary>
        /// Use to set a floating point number.
        /// </summary>
        /// <param name="variableName">Target variable name.</param>
        /// <param name="newValue">New value for the target variable.</param>
        /// <returns>False if anything went wrong, true otherwise.</returns>
        public static bool SetFloat(string variableName, float newValue) => m_bank.SetFloat(variableName, newValue);

        /// <summary>
        /// Use to set a string.
        /// </summary>
        /// <param name="variableName">Target variable name.</param>
        /// <param name="newValue">New value for the target variable.</param>
        /// <returns>False if anything went wrong, true otherwise.</returns>
        public static bool SetString(string variableName, string newValue) => m_bank.SetString(variableName, newValue);

        /// <summary>
        /// Use to set a boolean.
        /// </summary>
        /// <param name="variableName">Target variable name.</param>
        /// <param name="newValue">New value for the target variable.</param>
        /// <returns>False if anything went wrong, true otherwise.</returns>
        public static bool SetBoolean(string variableName, bool newValue) => m_bank.SetBoolean(variableName, newValue);


        /// <summary>
        /// Use to get an integer.
        /// </summary>
        /// <param name="variableName">Target variable name.</param>
        /// <param name="foundValue">The value of the target variable.</param>
        /// <returns>True if the variable with the target name exists. False otherwise.</returns>
        public static bool TryGetInt(string variableName, out int foundValue) => m_bank.TryGetInt(variableName, out foundValue);

        /// <summary>
        /// Use to get a floating point number.
        /// </summary>
        /// <param name="variableName">Target variable name.</param>
        /// <param name="foundValue">The value of the target variable.</param>
        /// <returns>True if the variable with the target name exists. False otherwise.</returns>
        public static bool TryGetFloat(string variableName, out float foundValue) => m_bank.TryGetFloat(variableName, out foundValue);

        /// <summary>
        /// Use to get a string.
        /// </summary>
        /// <param name="variableName">Target variable name.</param>
        /// <param name="foundValue">The value of the target variable.</param>
        /// <returns>True if the variable with the target name exists. False otherwise.</returns>
        public static bool TryGetString(string variableName, out string foundValue) => m_bank.TryGetString(variableName, out foundValue);

        /// <summary>
        /// Use to get a boolean.
        /// </summary>
        /// <param name="variableName">Target variable name.</param>
        /// <param name="foundValue">The value of the target variable.</param>
        /// <returns>True if the variable with the target name exists. False otherwise.</returns>
        public static bool TryGetBoolean(string variableName, out bool foundValue) => m_bank.TryGetBoolean(variableName, out foundValue);


        /// <summary>
        /// Use to register a value change callback to an integer.
        /// </summary>
        /// <param name="variableName">Target variable name.</param>
        /// <param name="callbackAction">Callback action to invoke on value change.</param>
        public static void AddListenerToInt(string variableName, Action<VariableValueChangedCallbackContext<int>> callbackAction)
        {
            m_bank.AddValueChangeListenerToInt(variableName, callbackAction);
        }

        /// <summary>
        /// Use to register a value change callback to a floating point number.
        /// </summary>
        /// <param name="variableName">Target variable name.</param>
        /// <param name="callbackAction">Callback action to invoke on value change.</param>
        public static void AddListenerToFloat(string variableName, Action<VariableValueChangedCallbackContext<float>> callbackAction)
        {
            m_bank.AddValueChangeListenerToFloat(variableName, callbackAction);
        }

        /// <summary>
        /// Use to register a value change callback to a string.
        /// </summary>
        /// <param name="variableName">Target variable name.</param>
        /// <param name="callbackAction">Callback action to invoke on value change.</param>
        public static void AddListenerToString(string variableName, Action<VariableValueChangedCallbackContext<string>> callbackAction)
        {
            m_bank.AddValueChangeListenerToString(variableName, callbackAction);
        }

        /// <summary>
        /// Use to register a value change callback to a boolean.
        /// </summary>
        /// <param name="variableName">Target variable name.</param>
        /// <param name="callbackAction">Callback action to invoke on value change.</param>
        public static void AddListenerToBoolean(string variableName, Action<VariableValueChangedCallbackContext<bool>> callbackAction)
        {
            m_bank.AddValueChangeListenerToBoolean(variableName, callbackAction);
        }
    }

}