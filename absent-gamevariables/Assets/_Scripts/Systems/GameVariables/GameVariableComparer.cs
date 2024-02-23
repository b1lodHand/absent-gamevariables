
using UnityEngine;

namespace com.absence.gamevariables
{
    [System.Serializable]
    public class GameVariableComparer
    {
        public enum ComparisonType
        {
            [InspectorName("<")] LessThan = 0,
            [InspectorName("≤")] LessOrEqual = 1,
            [InspectorName("=")] EqualsTo = 2,
            [InspectorName("≥")] GreaterOrEqual = 3,
            [InspectorName(">")] GreaterThan = 4,
        }

        [SerializeField] protected ComparisonType CompareBy = ComparisonType.EqualsTo;
        [SerializeField] protected GameVariableBank TargetBank;
        [SerializeField] protected string TargetVariableName = GameVariableBank.Null;

        [SerializeField] protected int IntValue;
        [SerializeField] protected float FloatValue;
        [SerializeField] protected string StringValue;
        [SerializeField] protected bool BoolValue;

        public virtual bool GetResult()
        {
            if (TargetBank == null) return true;
            if (TargetVariableName == GameVariableBank.Null) return true;

            if (TargetBank.HasString(TargetVariableName))
                return TargetBank.GetString(TargetVariableName).Value == StringValue;
            else if (TargetBank.HasBoolean(TargetVariableName))
                return TargetBank.GetBoolean(TargetVariableName).Value == BoolValue;
            else if (TargetBank.HasInt(TargetVariableName))
                return CompareNumerics(TargetBank.GetInt(TargetVariableName).Value, IntValue);
            else if (TargetBank.HasFloat(TargetVariableName))
                return CompareNumerics(TargetBank.GetFloat(TargetVariableName).Value, FloatValue);

            return true;
        }

        bool CompareNumerics(int a, int b)
        {
            switch (CompareBy)
            {
                case ComparisonType.LessThan:
                    return a < b;
                case ComparisonType.LessOrEqual:
                    return a <= b;
                case ComparisonType.EqualsTo:
                    return a == b;
                case ComparisonType.GreaterOrEqual:
                    return a >= b;
                case ComparisonType.GreaterThan:
                    return a > b;
                default:
                    return false;
            }
        }
        bool CompareNumerics(float a, float b)
        {
            switch (CompareBy)
            {
                case ComparisonType.LessThan:
                    return a < b;
                case ComparisonType.LessOrEqual:
                    return a <= b;
                case ComparisonType.EqualsTo:
                    return a == b;
                case ComparisonType.GreaterOrEqual:
                    return a >= b;
                case ComparisonType.GreaterThan:
                    return a > b;
                default:
                    return false;
            }
        }
    }
}