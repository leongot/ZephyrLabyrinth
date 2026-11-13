namespace ZephyrLabyrinth
{
    class LockPuzzle
    {
        /// <summary> Indica se il puzzle è già stato risolto (true = risolto).</summary>
        public bool cont = false;

        /// <summary> Le 4 combinazioni binarie generate casualmente (4 cifre ciascuna).</summary>
        public string binaryNumber1 { get; private set; } = "";
        public string binaryNumber2 { get; private set; } = "";
        public string binaryNumber3 { get; private set; } = "";
        public string binaryNumber4 { get; private set; } = "";

        /// <summary> Somma decimale delle combinazioni binarie convertite.</summary>
        public int sum { get; private set; }

        /// <summary> Generatore di numeri casuali per creare le combinazioni.</summary>
        private Random random = new Random();

        /// <summary> Gestisce il puzzle della serratura basato su combinazioni binarie.</summary>
        /// <remarks>
        /// <para>- Se il puzzle non è ancora stato risolto, genera quattro numeri binari casuali di 4 bit, calcola la loro somma in decimale e avvia il ciclo interattivo per tentare di risolvere l'enigma.
        /// <para>- Se il puzzle è già stato completato, informa il giocatore e registra il log.
        /// </remarks>
        /// <returns> Un Task asincrono che rappresenta la gestione del puzzle.</returns>
        public async Task Puzzle()
        {
            if (!cont)
            {
                Logger.Log("Inizializzazione variabili del Puzzle", "INFO");

                // Genera quattro numeri binari casuali di lunghezza 4
                binaryNumber1 = GenerateRandomBinaryNumber(4);
                binaryNumber2 = GenerateRandomBinaryNumber(4);
                binaryNumber3 = GenerateRandomBinaryNumber(4);
                binaryNumber4 = GenerateRandomBinaryNumber(4);

                // Calcola la somma dei valori decimali corrispondenti
                sum = Convert.ToInt32(binaryNumber1, 2)
                    + Convert.ToInt32(binaryNumber2, 2)
                    + Convert.ToInt32(binaryNumber3, 2)
                    + Convert.ToInt32(binaryNumber4, 2);

                // Mostra le combinazioni al giocatore tramite dialogo
                await dialogue.LockPuzzleShowNumbers();

                // Loop per gestire le scelte del giocatore fino a uscita
                while (true)
                {
                    await dialogue.IntroLockPuzzle();

                    // Chiede all'utente cosa fare, esce se ritorna true
                    bool exit = await dialogue.LockPuzzleChoice(this);
                    if (exit) break;
                }
            }
            else
            {
                // Caso in cui il puzzle è già stato risolto
                await WriteSlowly("SYSTEM: La serratura è stata già aperta");
                Logger.Log("L'enigma risulta già completato", "INFO");
            }
        }

        /// <summary> Genera una stringa rappresentante un numero binario casuale di una data lunghezza.</summary>
        /// <param name="length">Lunghezza del numero binario da generare.</param>
        /// <returns> Stringa contenente un numero binario.</returns>
        private string GenerateRandomBinaryNumber(int length)
        {
            string binaryNumber = "";
            for (int i = 0; i < length; i++)
            {
                // Aggiunge casualmente '0' o '1' alla stringa
                binaryNumber += random.Next(2).ToString();
            }
            return binaryNumber;
        }
    }
}
