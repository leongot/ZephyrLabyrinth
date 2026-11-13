namespace ZephyrLabyrinth
{
    class Altarpuzzle
    {
        /// <summary>Indica se il puzzle è stato risolto.</summary>
        /// <remarks> True se risolto </remarks>
        public bool cont = false;

        /// <summary> Indica se il cancello è chiuso (true = chiuso).</summary>
        /// <remarks> True se chiuso</remarks>
        public bool cancelloChiuso = true;

        /// <summary>Rappresenta la moneta della giustizia con l'immagine di una donna bendata.</summary>
        /// <remarks> Deve essere in inventario per risolvere enigma </remarks>
        public GameItem moneta = new GameItem("Moneta con donna bendata", false, 1, "Una moneta antica con l'immagine di una donna bendata che regge una bilancia.");
        /// <summary>Rappresenta la clessidra simbolo della saggezza e del tempo sospeso.</summary>
        /// <remarks> Non deve essere in inventario per risolvere enigma </remarks>
        public GameItem clessidra = new GameItem("Clessidra", false, 1, "Una clessidra che sembra fermare il tempo.");
        /// <summary>Rappresenta il cuore ardente, simbolo di coraggio e determinazione.</summary>
        /// <remarks> Deve essere in inventario per risolvere enigma </remarks>
        public GameItem cuore = new GameItem("Cuore di pietra ardente", false, 1, "Un cuore di pietra che emana calore.");

        /// <summary>Gestisce il puzzle nel santuario, permettendo al giocatore di interagire con gli oggetti.</summary>
        /// <remarks>
        /// <para>Mostra l'introduzione al santuario tramite dialogo.</para>
        /// <para>Abilita la raccolta degli oggetti: moneta, clessidra e cuore.</para>
        /// <para>Avvia un ciclo che continua finché il puzzle non è risolto (variabile cont è false).</para>
        /// <para>Durante il ciclo, mostra le opzioni per prendere gli oggetti tramite dialogo.</para>
        /// <para>Permette al giocatore di allontanarsi dall'altare se sceglie di farlo.</para>
        /// </remarks>
        public async Task Puzzle()
        {
            await dialogue.IntroAltar();
            
            moneta.CanBePickedUp = true;
            clessidra.CanBePickedUp = true;
            cuore.CanBePickedUp = true;

            while (!cont)
            {
                await dialogue.AltarTakeItemOption();
                bool leaveAltar = await dialogue.AltarTakeItemChoice();
                if (leaveAltar)
                    break;
            }
        }

        /// <summary>Gestisce l'interazione del giocatore con la clessidra.</summary>
        /// <remarks>
        /// <para>Mostra ripetutamente il menu della clessidra finché il giocatore non decide di uscire.</para>
        /// <para>Utilizza i metodi di dialogo per mostrare le opzioni e leggere la scelta del giocatore.</para>
        /// <para>Al termine dell'interazione, registra un log informativo.</para>
        /// </remarks>
        public async Task ClessidraInteraction()
        {
            bool sceltaClessidra = false;

            /// Loop fino a quando il giocatore non esce dal menu clessidra
            while (!sceltaClessidra)
            {
                await dialogue.ClessidraMenuOption();
                sceltaClessidra = await dialogue.ClessidraOptionChoice();
            }

            Logger.Log("Il giocatore smette di interagire con la clessidra", "INFO");
        }

        /// <summary>Gestisce la risoluzione dell'enigma del santuario tramite l'interazione con il giocatore.</summary>
        /// <remarks>
        /// <para>Mostra la richiesta di risposta all'enigma.</para>
        /// <para>Verifica che la risposta del giocatore corrisponda alla sequenza corretta senza spazi.</para>
        /// <para>Controlla che il giocatore possieda la moneta e il cuore, ma non la clessidra (che deve essere ancora nel santuario).</para>
        /// <para>In caso di risposta corretta:</para>
        /// <para>- Rimuove la clessidra dal santuario.</para>
        /// <para>- Aggiunge la "Master Sword" al santuario.</para>
        /// <para>- Fa comparire il personaggio Zephyr nel salone.</para>
        /// <para>- Aggiunge la Dea nel santuario e la memorizza in una variabile.</para>
        /// <para>- Registra con il logger lo stato di spawn di Zephyr.</para>
        /// <para>In caso di risposta errata, mostra il messaggio di errore.</para>
        /// </remarks>
        public async Task SolveAltarPuzzleWithInteraction()
        {
            await dialogue.AltarGiveAnswer();

            string correctSequence = "GiustiziaSaggezzaCoraggio"; // Sequenza corretta delle parole
            string? playerInput = ReadLine()?.Trim().Replace(" ", ""); // Rimuove spazi per confrontare correttamente

            // Controlla la correttezza della risposta e la presenza degli oggetti necessari
            if (playerInput?.Equals(correctSequence, StringComparison.OrdinalIgnoreCase) == true
                && player.Inventory.HasItem(moneta.Name)
                && player.Inventory.HasItem(cuore.Name)
                && !player.Inventory.HasItem(clessidra.Name) && santuario.Items.Contains(clessidra)) // Assicurati che la clessidra non sia stata presa e sia nel santuario
            {
                // Risposta corretta: rimuove oggetti, aggiunge spada e personaggi
                await dialogue.AltarCorrectAnswer();
                bool presente = santuario.Items.Contains(clessidra);
                Logger.Log($"Clessidra presente nella stanza? {presente}", "INFO");
                await santuario.RemoveItem(clessidra);
                Equipment masterSword = new Equipment("Master Sword", true, 4, 20, 0, 0, "La spada leggendaria che può essere estratta solo da chi è degno.");
                santuario.AddItem(masterSword);
                salone.AddCharacter(new Zephyr());
                GameCharacter dea = new GameCharacter("Dea", "Una dea benevola, custode della 'Master Sword'.", true);
                santuario.AddCharacter(dea);
                Dea = dea;

                if (salone.GetCharacter("Zephyr") != null)
                {
                    Logger.Log("Zephyr e' spawnato nel Salone", "INFO");
                }
                else
                {
                    Logger.Log("Zephyr non e' stato spawnato nel Salone", "ERROR");
                }
            }
            else
            {
                // Risposta errata: messaggio di errore
                await dialogue.AltarCorrectWrongAnswer();
            }
        }

        
        /// <summary>Gestisce l'interazione con il cancello che richiede la chiave magica per essere aperto.</summary>
        /// <remarks>
        /// <para> - Se il cancello è chiuso, informa il giocatore che serve una chiave speciale.</para>
        /// <para> - Se è già stato aperto, mostra un messaggio di stato.</para>
        /// <para> - Se il giocatore possiede la chiave magica e il cancello è chiuso, mostra il menu di interazione del cancello.</para>
        /// </remarks>
        public async Task GateInteraction()
        {
            if (cancelloChiuso)
            {
                await WriteSlowly("\nNARRATORE: Un cancello che puo' essere aperto solo con una chiave speciale.");
            }
            else
            {
                await WriteSlowly("\nSYSTEM: Il cancello e' gia' stato aperto con la chiave magica.");
            }

            // Se il giocatore ha la chiave magica e il cancello è chiuso, mostra menu interazione
                if (player.Inventory.HasItem(keypuzzle.chiaveMagica.Name) && cancelloChiuso)
                {
                    await dialogue.CancelloMenu();
                    await dialogue.CancelloMenuChoice();
                }
        }
    }
}
