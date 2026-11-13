namespace ZephyrLabyrinth
{
    public static class Logger
    {
        /// <summary>
        /// Percorso del file di log.
        /// </summary>
        private static readonly string logFilePath = "gamelog.log";

        /// <summary>
        /// Identificativo univoco della sessione di gioco corrente.
        /// </summary>
        private static string sessionId = Guid.NewGuid().ToString();
        /// <summary>
        /// Scrive un messaggio nel file di log con tag e timestamp.
        /// </summary>
        /// <param name="message">Il messaggio da loggare.</param>
        /// <param name="tag">Il tag che indica il tipo di messaggio.</param>
        /// <returns> void</returns>
        public static void Log(string message, string tag)
        {
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [SESSION: {sessionId}] [{tag}] {message}";

            try
            {
                // Aggiunge la nuova voce di log al file
                File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
            }
            catch
            {
                // In caso di errore durante il logging, ignorare per non bloccare il gioco
            }
        }

        /// <summary>
        /// Genera un nuovo ID sessione per un nuovo inizio di gioco o nuova partita.
        /// </summary>
        /// <returns> void</returns>
        public static void NewSession()
        {
            sessionId = Guid.NewGuid().ToString();
        }
        
        /// <summary>
        /// Elimina il file di log se esiste, per iniziare un log pulito.
        /// </summary>
        /// <returns> void</returns>
        public static void ClearLog()
        {
            if (File.Exists(logFilePath))
                File.Delete(logFilePath);
        }
    }
}
