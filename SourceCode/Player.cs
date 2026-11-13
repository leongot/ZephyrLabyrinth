using System.Diagnostics.Eventing.Reader;

namespace ZephyrLabyrinth
{
    /// <summary> Classe che rappresenta il giocatore e gestisce le sue proprietà e azioni nel gioco.</summary>
    class Player
    {
        /// <summary> Nome del giocatore.</summary>
        public string Name { get; set; }

        /// <summary> Peso massimo che il giocatore può trasportare.</summary>
        public int MaxCarryWeight { get; }

        /// <summary> Vita attuale del giocatore.</summary>
        public int Health { get; set; }

        /// <summary> Potere d'attacco del giocatore.</summary>
        public int AttackPower { get; set; }

        /// <summary> Valore di difesa del giocatore.</summary>
        public int Defence { get; set; }

        /// <summary> Stanza in cui si trova attualmente il giocatore.</summary>
        public Room CurrentRoom { get; set; }

        /// <summary> Inventario degli oggetti posseduti dal giocatore.</summary>
        public Inventory Inventory { get; set; }

        /// <summary> Peso attuale degli oggetti trasportati dal giocatore.</summary>
        public int currentWeight;

        /// <summary> Costruttore per inizializzare un nuovo giocatore.</summary>
        /// <param name="name"> Nome del giocatore.</param>
        /// <param name="maxCarryWeight"> Peso massimo che il giocatore può trasportare.</param>
        /// <param name="health"> Vita iniziale del giocatore (default 100).</param>
        /// <param name="attackPower"> Potere d'attacco iniziale del giocatore (default 10).</param>
        /// <param name="defence"> Valore di difesa iniziale del giocatore (default 15).</param>
        /// <returns> Crea un'istanza di Player con le proprietà inizializzate.</returns>
        public Player(string name, int maxCarryWeight, int health = 100, int attackPower = 10, int defence = 15)
        {
            Name = name;
            MaxCarryWeight = maxCarryWeight;
            Inventory = new Inventory();
            currentWeight = 0;
            Health = health;
            AttackPower = attackPower;
            Defence = defence;
        }

        /// <summary> Gestisce lo spostamento del giocatore nella direzione specificata, se esiste un'uscita.</summary>
        /// <param name="direction"> La direzione verso cui il giocatore vuole muoversi (es. "nord", "sud").</param>
        /// <remarks>
        /// <para> Se la direzione è valida, aggiorna la stanza corrente e mostra la descrizione.</para>
        /// <para> In caso contrario, informa il giocatore e registra un avviso nel log.</para>
        /// </remarks>
        /// <returns> Un Task asincrono che rappresenta il completamento dello spostamento.</returns>
        public async Task Move(string direction)
        {
            if (CurrentRoom.HasExit(direction)) // Controlla se la direzione è valida
            {
                CurrentRoom = CurrentRoom.GetExit(direction); // Cambia stanza
                await WriteSlowly($"\nSYSTEM: Ti sei spostato in {CurrentRoom.Name}.");
                await CurrentRoom.DisplayRoomDescription(); // Mostra descrizione stanza
                await dialogue.CheckDialogueCondition(); // Verifica condizioni dialogo
            }
            else
            {
                await WriteSlowly("\nSYSTEM: Non puoi andare da quella parte."); // Direzione non valida
                Logger.Log($"{player.Name} si è spostato in un direzione senza uscita", "WARN");
            }
        }

        /// <summary> Permette al giocatore di parlare con un personaggio specificato per nome, se presente nella stanza.</summary>
        /// <param name="target"> Il nome del personaggio con cui si vuole parlare.</param>
        /// <remarks>
        /// <para>- Il metodo verifica se il personaggio è presente nella stanza corrente.</para>
        /// <para>- In base al nome, viene avviato un dialogo personalizzato.</para>
        /// <para>- Se il personaggio non esiste, viene mostrato un messaggio di errore.</para>
        /// </remarks>
        /// <returns>Un Task asincrono che rappresenta la conclusione del dialogo (o del fallimento).</returns>
        public async Task Talk(string target)
        {
            // Cerca personaggio interagibile con il nome target
            GameCharacter character = CurrentRoom.GetCharacter(target);

            if (character != null)
            {
                // Dialoghi specifici in base al nome del personaggio
                if (character.Name == "Elara")
                {
                    await dialogue.ElaraDialogue();
                }
                if (character.Name == "Condri")
                {
                    await dialogue.CondriNelloStudio();
                    if (keypuzzle.portboc == true)
                        keypuzzle.OpenDoorStudioAlchemico();
                }
                if (character.Name == "Dea")
                {
                    await dialogue.DeaTalk();
                }
                if (character.Name == "Zephyr")
                {
                    await dialogue.ZephyrFightDialogue();
                }
            }
            else
            {
                await WriteSlowly("\nSYSTEM: Non c'è nessuna persona con quel nome."); // Personaggio non trovato
            }
        }

        /// <summary> Permette al giocatore di interagire con un oggetto specifico presente nella stanza corrente.</summary>
        /// <param name="target"> Il nome dell'oggetto con cui si vuole interagire.</param>
        /// <remarks>
        /// <para> In base al nome dell’oggetto, viene avviata un’azione o un puzzle specifico.</para>
        /// <para> Se l’oggetto non esiste nella stanza, viene mostrato un messaggio di errore.</para>
        /// <para> Ogni interazione viene registrata nel log.</para>
        /// </remarks>
        /// <returns> Un Task asincrono che rappresenta l’esecuzione dell’interazione.</returns>
        public async Task Interact(string target)
        {
            // Cerca oggetto interagibile
            Interactable interactable = CurrentRoom.GetInteractable(target);
            if (interactable != null)
            {
                // Azioni specifiche in base al nome dell'oggetto
                if (interactable.Name == "Calderone")
                {
                    await keypuzzle.Puzzle();
                    Logger.Log($"Il giocatore ha interagito con {interactable.Name}", "INFO");
                }
                if (interactable.Name == "Tavolino")
                {
                    await keypuzzle.TavolinoInteraction();
                    Logger.Log($"Il giocatore ha interagito con {interactable.Name}", "INFO");
                }
                if (interactable.Name == "Statua")
                {
                    await keeperpuzzle.Puzzle();
                    Logger.Log($"Il giocatore ha interagito con {interactable.Name}", "INFO");
                }
                if (interactable.Name == "Porta")
                {
                    keypuzzle.OpenDoorStudioAlchemico();
                    Logger.Log($"Il giocatore ha interagito con {interactable.Name}", "INFO");
                }
                if (interactable.Name == "Serratura")
                {
                    await lockpuzzle.Puzzle();
                    Logger.Log($"Il giocatore ha interagito con {interactable.Name}", "INFO");
                }
                if (interactable.Name == "Incisione")
                {
                    await dialogue.LockPuzzleIncisione();
                    Logger.Log($"Il giocatore ha interagito con {interactable.Name}", "INFO");
                }
                if (interactable.Name == "Altare")
                {
                    await altarpuzzle.Puzzle();
                    Logger.Log($"Il giocatore ha interagito con {interactable.Name}", "INFO");
                }
                if (interactable.Name == "Cancello")
                {
                    await altarpuzzle.GateInteraction();
                    Logger.Log($"Il giocatore ha interagito con {interactable.Name}", "INFO");
                }
            }
            else
            {
                await WriteSlowly("\nSYSTEM: Non c'è nessun oggetto con quel nome."); // Oggetto non trovato
                Logger.Log($"Il giocatore ha interagito con un oggetto inesistente", "WARN");
            }

        }

        /// <summary> Permette al giocatore di osservare la stanza corrente o gli oggetti al suo interno.</summary>
        /// <param name="target"> La "room" da osservare.</param>
        /// <remarks>
        /// Ogni stanza ha una descrizione specifica. Se si osservano gli oggetti, 
        /// vengono mostrati solo quelli raccolti precedentemente (inventario nella stanza),
        /// con informazioni dettagliate se l'oggetto è equipaggiabile.
        /// </remarks>
        /// <returns> Un Task asincrono che rappresenta l’azione di osservazione.</returns>
        public async Task Observe(string target)
        {
            if (target == "room")
            {
                // Osserva la stanza corrente
                await WriteSlowly($"\nSYSTEM: Stai osservando {CurrentRoom.Name}.");
                if (CurrentRoom == cripta)
                {
                    await dialogue.ObserveCripta();
                }
                else if (CurrentRoom == santuario)
                {
                    await dialogue.ObserveSanctuary();
                }
                else if (CurrentRoom == salone)
                {
                    await dialogue.ObserveSalone();
                }
                else if (CurrentRoom == studioAlchemico)
                {
                    await dialogue.ObserveStudioAlchemico();
                }
                else if (CurrentRoom == stanzaSegreta)
                {
                    await dialogue.ObserveSecretRoom();
                }
                else if (CurrentRoom == ingresso)
                {
                    await dialogue.ObserveIngresso();
                }
                else if (CurrentRoom == grotta)
                {
                    await dialogue.ObserveGrotta();
                }
                Logger.Log($"Il giocatore ha osservato {CurrentRoom.Name}", "INFO");
            }
            else if (target == "items")
            {
                // Osserva gli oggetti presenti nella stanza corrente
                if (CurrentRoom == cripta)
                {
                    await dialogue.ObserveItemsCripta();
                }
                else if (CurrentRoom == santuario)
                {
                    await WriteSlowly("\nSYSTEM: Non ci sono oggetti che attirano la tua attenzione in questa stanza.");
                }
                else if (CurrentRoom == ingresso)
                {
                    await dialogue.ObserveItemsIngresso();
                }
                else if (CurrentRoom == grotta)
                {
                    await dialogue.ObserveItemsGrotta();
                }
                else if (CurrentRoom == salone)
                {
                    await dialogue.ObserveItemsSalone();
                }
                else if (CurrentRoom == studioAlchemico)
                {
                    await dialogue.ObserveItemsStudioAlchemico();
                }
                else if (CurrentRoom == stanzaSegreta)
                {
                    await dialogue.ObserveItemsSecretRoom();
                }
                Logger.Log($"Il giocatore ha osservato gli oggetti della stanza {CurrentRoom.Name}", "INFO");

                var itemsInRoom = CurrentRoom.items;

                if (itemsInRoom.Count == 0)
                {
                    await WriteSlowly("\nSYSTEM: Non hai lasciato oggetti in questa stanza.");
                }
                else
                {
                    if (itemsInRoom.Any(item => item.CanBePickedUp))
                    {
                        await WriteSlowly("\nSYSTEM: Gli oggetti presenti nella stanza sono:");
                    }
                    foreach (var item in itemsInRoom)
                    {
                        if (item.CanBePickedUp)
                        {
                            await WriteSlowly($"\n        {item.Name}: {item.Description} ");
                            await WriteSlowly($"        Peso: {item.Weight}  ");
                        }
                        if (item is Equipment equipment)
                        {
                            await WriteSlowly($"        Stats: {equipment.AttackPower} ATK | {equipment.Defence} DEF | {equipment.Health} HP");
                        }
                    }
                }
            }
        }

        /// <summary> Metodo per assegnare nome giocatore. </summary>
        /// <returns> Un'attività asincrona che gestisce l'inserimento del nome del giocatore.</returns>
        public async Task PlayerInsertName()
        {
            string? Name = ReadLine()?.Trim();
            player.Name = Name;
        }

        /// <summary> Applica danni al giocatore e gestisce eventuali effetti narrativi o eventi di morte.</summary>
        /// <param name="damage"> Il valore dei danni da infliggere al giocatore.</param>
        /// <remarks>
        /// <para> Se il giocatore si trova in stanze specifiche (come lo Studio Alchemico o la Cripta), 
        /// vengono attivati effetti narrativi unici.</para>
        /// <para> Se la salute scende a 0 o meno, il gioco segnala la sconfitta del giocatore.</para>
        /// </remarks>
        /// <returns> Un Task asincrono che rappresenta l'elaborazione del danno.</returns>
        public async Task TakeDamage(int damage)
        {
            // Effetti speciali se si è nello studio alchemico
            if (CurrentRoom == studioAlchemico)
            {
                await WriteSlowly("\nNARRATORE: Il calderone iniza ad agitarsi, il suo contenuto si trasforma in una scossa di potere magico che ti colpisce.");
                await WriteSlowly("           L'intruglio nel calderone smette di reagire ritornando al suo stadio originale.");
            }
            else if (CurrentRoom == cripta)
            {
                await Task.Delay(1500);
                await WriteSlowly("\nStatua: NO! Ora subirai la mia punizione!");
            }

            player.Health -= damage;
            player.Health = Math.Max(player.Health, 0);
            Logger.Log($"{player.Name} ha subito {damage} danni. Salute attuale: {player.Health}", "INFO");

            if (player.Health <= 0)
            {
                Logger.Log($"{player.Name} è stato sconfitto", "INFO");
                await WriteSlowly($"\nSYSTEM: {player.Name} ha perso {damage} HP. Salute attuale: {player.Health} HP");
                await WriteSlowly("SYSTEM: La tua vita è arrivata a 0 HP, hai perso!");
                await WriteSlowly("\nSYSTEM: Premi un qualsiasi tasto per tornare al menu principale.");
                ReadKey(true);
                dialogue.EndBattle = true;
            }
            else
            {
                await WriteSlowly($"\nSYSTEM: {player.Name} ha perso {damage} HP. Salute attuale: {player.Health} HP");
            }
        }

        /// <summary> Metodo che gestisce quando il giocatore si allontana da un enigma/oggetto interagibile.</summary>
        /// <param name="enigmaObject"> L'oggetto interagibile da cui il giocatore si allontana.</param>
        /// <returns> Un'attività asincrona che gestisce l'allontanamento del giocatore dall'oggetto.</returns>
        public async Task PlayerGoesAwayFrom(Interactable enigmaObject)
        {
            string objectName = enigmaObject.Name;
            string roomName = enigmaObject.RoomName;

            // Dizionario articoli da usare per l'oggetto
            var articoliOggetti = new Dictionary<string, string>
            {
                { "Calderone", "dal" },
                { "Statua", "dalla" },
                { "Altare", "dall'" },
                { "Serratura", "dalla" }
            };

            // Dizionario articoli da usare per la stanza
            var articoliRoom = new Dictionary<string, string>
            {
                { "Cripta", "della" },
                { "Santuario", "del" },
                { "Ingresso", "dell'" },
                { "Studio Alchemico", "dello" }
            };

            string articoloOggetto = articoliOggetti.ContainsKey(objectName) ? articoliOggetti[objectName] : "da";
            string articoloRoom = articoliRoom.ContainsKey(roomName) ? articoliRoom[roomName] : "del";

            await WriteSlowly($"\nSYSTEM: {player.Name} si allontana {articoloOggetto} {objectName}.");
            Logger.Log($"Il giocatore è uscito dall'enigma {articoloRoom} {roomName}", "INFO");
        }

        /// <summary> Avvisa il giocatore in caso di input non valido.</summary>
        /// <returns> Un'attività asincrona che gestisce l'avviso di input non valido.</returns>
        public async Task PlayerInputWarn()
        {
            await WriteSlowly("\nSYSTEM: Opzione non valida.");
            Logger.Log("Il giocatore ha inserito un input non valido", "WARN");
        }

        /// <summary> Mostra le statistiche attuali del giocatore, inclusi salute, attacco, difesa e peso trasportato.</summary>
        /// <remarks> I dati visualizzati includono anche il nome del giocatore e sono presentati con effetto di scrittura lenta.</remarks>
        /// <returns> Un Task asincrono per supportare la visualizzazione progressiva delle informazioni.</returns>
        public async Task ShowStats()
        {
            await WriteSlowly($"\nLe statistiche di {player.Name}: ");
            await WriteSlowly($"\nSalute: {player.Health} HP");
            await WriteSlowly($"Attacco: {player.AttackPower} ATK");
            await WriteSlowly($"Difesa:   {player.Defence} DEF");
            await WriteSlowly($"\nPeso trasportato: {player.currentWeight}/{player.MaxCarryWeight}");
        }

        /// <summary>Visualizza la mappa del labirinto se il giocatore possiede l'oggetto "mappa".</summary>
        /// <returns>Task asincrono che completa la visualizzazione della mappa o mostra un messaggio d'errore.</returns>
        public async Task Map()
        {
            if (GameItem.mappa == null)
            {
                await WriteSlowly("\nSYSTEM: La mappa non è stata inizializzata correttamente.");
                Logger.Log("Errore: GameItem.mappa è null.", "ERROR");
                return;
            }

            if (Inventory.HasItem(GameItem.mappa.Name))
            {
                await WriteSlowly(@"
                         ┌──────────────┐
                         │  Santuario   │
                         └──────────────┘
                                │
                                │
     ┌────────────┐       ┌────────────┐       ┌──────────────┐
     │   Cripta   │───────│   Salone   │───────│ Studio Alch. │
     └────────────┘       └────────────┘       └──────────────┘
                                │
                                │
                          ┌────────────┐
                          │  Ingresso  │
                          └────────────┘
                                │
                                │
                          ┌────────────┐
                          │   Grotta   │
                          └────────────┘");
            }
            else
            {
                await WriteSlowly("\nSYSTEM: Non hai la mappa del labirinto");
            }
        }
    }
}
