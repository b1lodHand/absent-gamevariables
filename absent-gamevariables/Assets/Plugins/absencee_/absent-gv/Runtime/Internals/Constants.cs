namespace com.absence.gamevariables.internals
{
    /// <summary>
    /// The static class responsible for holding the constant and readonly values used within this system.
    /// </summary>
    public static class Constants
    {
        public const string GAMEVARIABLES_BANK_NAME = "GameVariables";
        public const string SUBFOLDER_NAME = "VariableBanks";

        public const string RESOURCES_PATH = "Assets/Resources";
        public const string ADDRESSABLES_PATH = "Assets/Scriptables";

        public const string GAMEVARIABLES_GUID_PREF_NAME = "gamevariables-guid";

        public static readonly string RESOURCES_FULL_PATH = $"{RESOURCES_PATH}/{SUBFOLDER_NAME}/{GAMEVARIABLES_BANK_NAME}.asset";
        public static readonly string RESOURCES_FULL_PATH_RUNTIME = $"{SUBFOLDER_NAME}/{GAMEVARIABLES_BANK_NAME}";

        public static readonly string ADDRESSABLES_FULL_PATH = $"{ADDRESSABLES_PATH}/{SUBFOLDER_NAME}/{GAMEVARIABLES_BANK_NAME}.asset";
    }

}