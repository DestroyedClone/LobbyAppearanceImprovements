namespace LobbyAppearanceImprovements
{
    internal static class LAILogging
    {
        internal static BepInEx.Logging.ManualLogSource _logger = null;
        public static void Init(BepInEx.Logging.ManualLogSource manualLogSource)
        {
            _logger = manualLogSource;
        }

        public static void LogError(string message, ConfigSetup.LoggingStyle loggingStyle)
        {
            if (ConfigSetup.ShowLoggingText.Value >= loggingStyle)
                _logger.LogError(message);
        }
        public static void LogMessage(string message, ConfigSetup.LoggingStyle loggingStyle)
        {
            if (ConfigSetup.ShowLoggingText.Value >= loggingStyle)
                _logger.LogMessage(message);
        }
        public static void LogWarning(string message, ConfigSetup.LoggingStyle loggingStyle)
        {
            if (ConfigSetup.ShowLoggingText.Value >= loggingStyle)
                _logger.LogWarning(message);
        }
    }
}