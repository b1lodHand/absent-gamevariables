
using UnityEngine;

namespace com.absence.gamevariables
{
    [System.Serializable]
    public class GameVariableSetter
    {
        [SerializeField] protected GameVariableBank TargetBank;
        [SerializeField] protected string TargetVariableName = GameVariableBank.Null;

        [SerializeField] protected int IntValue;
        [SerializeField] protected float FloatValue;
        [SerializeField] protected string StringValue;
        [SerializeField] protected bool BoolValue;

        /// <summary>
        /// Sets the target variable in target <see cref="GameVariableBank"/> to intended value.
        /// </summary>
        public virtual void Perform()
        {
            if (TargetBank == null) return;
            if (TargetVariableName == GameVariableBank.Null) return;

            if (TargetBank.HasInt(TargetVariableName))
                TargetBank.SetInt(TargetVariableName, IntValue);
            else if (TargetBank.HasFloat(TargetVariableName))
                TargetBank.SetFloat(TargetVariableName, FloatValue);
            else if (TargetBank.HasString(TargetVariableName))
                TargetBank.SetString(TargetVariableName, StringValue);
            else if (TargetBank.HasBoolean(TargetVariableName))
                TargetBank.SetBoolean(TargetVariableName, BoolValue);
        }
    }

}