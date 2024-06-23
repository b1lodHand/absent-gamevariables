using com.absence.variablesystem;

namespace com.absence.gamevariables
{
    /// <summary>
    /// Variable setter sub-type specifically designed to work with the <see cref="GameVariables"/>.
    /// </summary>
    [System.Serializable]
    public sealed class GVSetter : BaseVariableSetter
    {
        public override bool HasFixedBank => true;

        /// <summary>
        /// Use to refresh the view (in editor) of this manipulator when needed.
        /// </summary>
        public void Refresh()
        {
            string guid = GameVariables.EditorBankGuid;
            if (string.IsNullOrWhiteSpace(guid))
            {
                m_targetBankGuid = string.Empty;
                return;
            }

            m_targetBankGuid = guid;
        }

        protected override VariableBank GetRuntimeBank() => GameVariables.GetRuntimeBank();
    }
}