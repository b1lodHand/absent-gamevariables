namespace com.absence.gamevariables.internals
{
    /// <summary>
    /// The static class responsible for holding the constant and readonly values used within this system.
    /// </summary>
    public static class Constants
    {
        public const string GAMEVARIABLES_BANK_NAME = "GameVariables";
        public const string GAMEVARIABLES_FOLDER_NAME = "VariableBanks";
        public const string RESOURCES_PATH = "Assets/Resources";

        public const string GAMEVARIABLES_GUID_PREF_NAME = "gamevariables-guid";

        public static readonly string GAMEVARIABLES_BANK_PATH = $"{RESOURCES_PATH}/{GAMEVARIABLES_FOLDER_NAME}/{GAMEVARIABLES_BANK_NAME}.asset";
        public static readonly string GAMEVARIABLES_BANK_PATH_RUNTIME = $"{GAMEVARIABLES_FOLDER_NAME}/{GAMEVARIABLES_BANK_NAME}";
    }

}