using System.Security.Cryptography.X509Certificates;

namespace ZephyrLabyrinth
{
    /// <summary>
    /// Classe che rappresenta lo stato di gioco da salvare e caricare.
    /// Contiene informazioni sul giocatore, sull'inventario, sulle stanze visitate e sullo stato degli enigmi.
    /// </summary>
    public class GameStateManager
    {
        /// <summary>Nome del giocatore.</summary>
        public string PlayerName { get; set; }
        /// <summary>Nome della stanza corrente in cui si trova il giocatore.</summary>
        public string CurrentRoomName { get; set; }
        /// <summary>Lista degli oggetti nell'inventario del giocatore.</summary>
        public List<GameItem> Inventory { get; set; }
        /// <summary>Dizionario che associa ogni stanza a un valore bool che indica se è stata visitata.</summary>
        public Dictionary<string, bool> RoomVisitedStates { get; set; }
        /// <summary>Dizionario che associa il nome della stanza a una lista di equipaggiamenti presenti nella stanza.</summary>
        public Dictionary<string, List<Equipment>> WorldEquipments { get; set; }
        /// <summary> Dizionario che associa il nome della stanza alla lista di oggetti per gli enigmi presenti nella stanza.</summary>
        public Dictionary<string, List<GameItem>> QuestItems { get; set; }
        /// <summary>Capacità massima di peso che il giocatore può trasportare.</summary>
        public int MaxCarryWeight { get; set; }
        /// <summary>Peso corrente degli oggetti trasportati dal giocatore.</summary>
        public int currentWeight { get; set; }
        /// <summary>Flag per indicare se il teletrasporto è stato attivato.</summary>
        public bool TelAtt { get; set; }
        /// <summary>Flag per indicare se il primo teletrasporto è stato effettuato.</summary>
        public bool firstTeleport { get; set; }
        /// <summary>Flag per stato dell'enigma statua.</summary>
        public bool KeeperPuzzle { get; set; }
        /// <summary>Flag che indica se la Pergamena Magica può essere raccolta.</summary>
        public bool MagicScrollPickable { get; set; }
        /// <summary>Flag che indica se la Chiave Magica può essere raccolta.</summary>
        public bool MagicKeyPickable { get; set; }
        /// <summary>Flag che indica se la Master Sword può essere raccolta.</summary>
        public bool MasterSwordPickable { get; set; }
        /// <summary>Flag per stato dell'enigma KeyPuzzle.</summary>
        public bool KeyPuzzle { get; set; }
        /// <summary>Flag per blocco porta dell'enigma calderone.</summary>
        public bool PortBloc { get; set; }
        /// <summary>Flag per stato dell'enigma serratura.</summary>
        public bool LockPuzzle { get; set; }
        /// <summary>Flag per stato dell'enigma altare.</summary>
        public bool AltarPuzzle { get; set; }
        /// <summary>Flag che indica se il cancello è chiuso.</summary>
        public bool CancelloChiuso { get; set; }
        /// <summary>Flag per indicare se l'introduzione a Zephyr è stata mostrata.</summary>
        public bool IntroZephyr { get; set; }
        /// <summary>Valore di salute del giocatore.</summary>
        public int playerHealth { get; set; }
        /// <summary>Valore di attacco del giocatore.</summary>
        public int playerAttackPower { get; set; }
        /// <summary>Valore di difesa del giocatore.</summary>
        public int playerDefence { get; set; }
        /// <summary>Valore di salute di Zephyr.</summary>
        public int zephyrHealth { get; set; }
        /// <summary>Valore di attacco di Zephyr.</summary>
        public int zephyrAttackPower { get; set; }
        /// <summary>Valore di difesa di Zephyr.</summary>
        public int zephyrDefence { get; set; }
        /// <summary>Flag per indicare se si e' entrati nello studio alchemico la prima volta.</summary>
        public bool DiscoveredStudio { get; set; }
        /// <summary>Flag per indicare se si e' entrati nella stanza segreta la prima volta.</summary>
        public bool DiscoveredSecretRoom { get; set; }
        /// <summary>Flag per indicare se Condri è stato spawnato.</summary>
        public bool CondriSpawned { get; set; }
        /// <summary>Flag per indicare se Dea è stata spawnata nel santuario dopo aver risolo enigma altare.</summary>
        public bool DeaIsPresent { get; set; }
        /// <summary>Velocità di digitazione globale configurata dall'utente.</summary>
        public int GlobalTypingSpeed { get; set; }
    }

    /// <summary>Classe statica che gestisce il salvataggio e il caricamento dello stato di gioco da file JSON./// </summary>
    public static class SaveSystem
    {
        // Percorso del file di salvataggio del gioco.
        private static readonly string SaveFilePath = "zephyrlabyrinth.json";

        /// <summary> Salva lo stato di gioco su file in formato JSON.</summary>
        /// <param name="state">Oggetto contenente lo stato di gioco da salvare.</param>
        /// <returns> Task che rappresenta l'operazione di salvataggio.</returns>
        /// <remarks>
        /// Utilizza la serializzazione JSON con formattazione indentata.
        /// In caso di successo, scrive il file di salvataggio e registra un log informativo.
        /// Se si verifica un errore durante il salvataggio, mostra un messaggio di errore e registra un log di errore.
        /// </remarks>
        public static void Save(GameStateManager state)
        {
            try
            {
                string json = JsonSerializer.Serialize(state, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(SaveFilePath, json);
                WriteLine("\nSYSTEM: Salvataggio effettuato correttamente.");
                Logger.Log("Salvataggio effettuato con successo", "INFO");
            }
            catch (Exception ex)
            {
                WriteLine($"\nSYSTEM: Errore durante il salvataggio: {ex.Message}");
                Logger.Log("Salvataggio non andato a buon fine", "ERROR");
            }
        }

        
        /// <summary> Carica lo stato di gioco dal file JSON.</summary>
        /// <returns>
        /// Un'istanza di <see cref="GameStateManager"/> contenente lo stato caricato,
        /// oppure <c>null</c> se il file di salvataggio non esiste o si verifica un errore.
        /// </returns>
        /// <remarks>
        /// Se il file di salvataggio non è presente, viene mostrato un messaggio di avviso e registrato un log.
        /// In caso di errore durante la deserializzazione, viene mostrato un messaggio di errore e registrato un log.
        /// </remarks>
        public static GameStateManager Load()
        {
            try
            {
                if (!File.Exists(SaveFilePath))
                {
                    WriteLine("SYSTEM: Nessun salvataggio trovato.");
                    Logger.Log("Non ci sono partite salvate", "WARN");
                    return null;
                }
                string json = File.ReadAllText(SaveFilePath);
                return JsonSerializer.Deserialize<GameStateManager>(json);
            }
            catch (Exception ex)
            {
                WriteLine($"SYSTEM: Errore durante il caricamento: {ex.Message}");
                Logger.Log("Caricamento di partita non andato a buon fine", "ERROR");
                return null;
            }
        }
    }
    
    /// <summary> Classe statica che gestisce le operazioni di salvataggio e caricamento dello stato di gioco.</summary>
    public class GameState
    {
        /// <summary> Salva lo stato attuale di gioco nel file di salvataggio.
        /// Controlla l'inizializzazione delle variabili principali prima di salvare.</summary>
        /// <returns> Task che rappresenta l'operazione di salvataggio.</returns>
        /// <remarks> Verifica che player, stanza corrente e inventario siano inizializzati prima di procedere.
        /// Imposta il tipo di ogni oggetto nell'inventario (Equipment o Game Item).
        /// Raccoglie gli equipaggiamenti e gli oggetti delle stanze per la serializzazione.
        /// Controlla la presenza di personaggi speciali (Zephyr, Dea) e oggetti chiave in stanze specifiche.
        /// Crea un'istanza di <see cref="GameStateManager"/> con tutti i dati rilevanti e chiama il sistema di salvataggio.</remarks>
        public static void Save()
        {
            if (player == null)
            {
                WriteLine("\nSYSTEM: Errore: player non inizializzato.");
                Logger.Log("Player non inizializzato salvataggio non riuscito", "FATAL");
                return;
            }
            if (player.CurrentRoom == null)
            {
                WriteLine("\nSYSTEM: Errore: player.CurrentRoom non inizializzato.");
                Logger.Log("Player.CurrentRoom non inizializzato salvataggio non riuscito", "FATAL");
                return;
            }
            if (player.Inventory == null)
            {
                WriteLine("\nSYSTEM: Errore: inventario non inizializzato.");
                Logger.Log("Inventario non inizializzato salvataggio non riuscito", "FATAL");
                return;
            }

            // Imposta il tipo per ogni oggetto nell'inventario
            foreach (var item in player.Inventory.inventory)
            {
                if (item is Equipment)
                {
                    item.Type = "Equipment";
                }
                else
                {
                    item.Type = "Game Item";
                }
            }

            // Prepara dizionario di equipaggiamenti nelle stanze attuali per la serializzazione.
            var worldEquipments = rooms.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.items.Where(i => i is Equipment).Cast<Equipment>().ToList());
            var questItems = rooms.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.items.Where(i => i is GameItem).Cast<GameItem>().ToList());

            // Riferimenti agli oggetti speciali in alcune stanze, per stato pickable.
            GameItem magicScroll = null;
            if (rooms != null && rooms.ContainsKey("Cripta"))
                magicScroll = cripta.GetItem("Pergamena Magica");

            GameItem magicKey = null;
            if (rooms != null && rooms.ContainsKey("Studio Alchemico"))
                magicKey = studioAlchemico.GetItem("Chiave Magica");

            GameItem masterSword = null;
            if (rooms != null && rooms.ContainsKey("Santuario"))
                masterSword = santuario.GetItem("Master Sword");

            //verifica se Zephyr esiste nella stanza Salone
            var zephyrInSalone = salone.characters.FirstOrDefault(c => c is Zephyr) as Zephyr;

            if (zephyrInSalone != null)
            {
                zephyr = zephyrInSalone;
                Logger.Log("DEBUG: Zephyr trovato nel Salone durante il salvataggio.", "DEBUG");
            }
            else
            {
                zephyr = null;
                Logger.Log("DEBUG: Zephyr non trovato nel Salone durante il salvataggio.", "WARN");
            }

            //verifica se Dea esiste nella stanza Santuario
            var deaInSantuario = santuario.characters.OfType<GameCharacter>().FirstOrDefault(c => c.Name == "Dea");

            if (deaInSantuario != null && altarpuzzle.cont == true)
            {
                Dea = deaInSantuario;
                Logger.Log("DEBUG: Dea trovato nel Santuario durante il salvataggio.", "DEBUG");
            }
            else if (deaInSantuario == null || altarpuzzle.cont == false)
            {
                Dea = null;
                Logger.Log("DEBUG: Dea non trovato nel Santuario durante il salvataggio.", "WARN");
            }


            // Crea e popola l'istanza di GameStateManager con lo stato attuale.
            GameStateManager state = new GameStateManager
            {
                PlayerName = player.Name,
                CurrentRoomName = player.CurrentRoom.Name,
                Inventory = player.Inventory.inventory.ToList(),
                MaxCarryWeight = player.MaxCarryWeight,
                currentWeight = player.currentWeight,
                LockPuzzle = lockpuzzle.cont,
                KeeperPuzzle = keeperpuzzle.cont,
                MagicScrollPickable = magicScroll?.CanBePickedUp ?? false,
                MagicKeyPickable = magicKey?.CanBePickedUp ?? false,
                MasterSwordPickable = masterSword?.CanBePickedUp ?? false,
                KeyPuzzle = keypuzzle.cont,
                PortBloc = keypuzzle.portboc,
                AltarPuzzle = altarpuzzle.cont,
                TelAtt = teleport.telAtt,
                firstTeleport = teleport.firstTeleportDone,
                CancelloChiuso = altarpuzzle.cancelloChiuso,
                IntroZephyr = dialogue.introZephyr,
                playerHealth = player.Health,
                playerAttackPower = player.AttackPower,
                playerDefence = player.Defence,
                zephyrHealth = zephyr?.Health ?? 1000, // Valore di default se Zephyr non esiste
                zephyrAttackPower = zephyr?.AttackPower ?? 300, // Valore di default se Zephyr non esiste
                zephyrDefence = zephyr?.Defence ?? 150, // Valore di default se Zephyr non esiste
                WorldEquipments = worldEquipments,
                QuestItems = questItems,
                RoomVisitedStates = rooms.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.hasBeenVisited),
                CondriSpawned = dialogue.condriSpawned,
                DeaIsPresent = Dea != null, // Indica se Dea è presente nel santuario
                DiscoveredStudio = keypuzzle.discoveredStudio,
                DiscoveredSecretRoom = teleport.discoveredSecretRoom,
                GlobalTypingSpeed = GameLoop.GlobalTypingSpeed
            };
            Logger.Log($"DEBUG: Salvo Zephyr - HP: {zephyr?.Health}, ATK: {zephyr?.AttackPower}, DEF: {zephyr?.Defence}", "DEBUG");

            // Effettua il salvataggio tramite SaveSystem.
            SaveSystem.Save(state);
        }

        /// <summary> Carica lo stato di gioco da file e ripristina il mondo di gioco e il giocatore.</summary>
        /// <returns>Restituisce <c>true</c> se il caricamento ha avuto successo, <c>false</c> altrimenti.</returns>
        /// <remarks>
        /// <para>- Crea o aggiorna il giocatore con nome e capacità massima peso.</para>
        /// <para>- Imposta la stanza corrente del giocatore (con fallback su "Grotta").</para>
        /// <para>- Ricostruisce l'inventario del giocatore, ricreando Equipment e GameItem.</para>
        /// <para>- Ripristina lo stato degli enigmi e variabili di gioco.</para>
        /// <para>- Ripristina la presenza e stato degli oggetti speciali nelle stanze (Pergamena Magica, Chiave Magica, Master Sword).</para>
        /// <para>- Ricollega le stanze in base allo stato degli enigmi (puzzle completati o meno).</para>
        /// <para>- Gestisce il personaggio Zephyr nella stanza Salone, creando nuova istanza se necessario e ripristinando le sue statistiche.</para>
        /// <para>- Ripristina le statistiche del giocatore.</para>
        /// <para>- Ripristina lo stato di visita delle stanze.</para>
        /// <para>- Ripristina gli equipaggiamenti e gli oggetti missione nelle stanze se mancanti e non presenti nell'inventario.</para>
        /// </remarks>
        public static async Task<bool> LoadGame()
        {
            GameStateManager loaded = SaveSystem.Load();
            if (loaded != null)
            {
                // Crea o aggiorna il giocatore con nome e capacità massima peso.
                player = new Player(loaded.PlayerName, loaded.MaxCarryWeight);
                Logger.Log($"DEBUG: Caricato Zephyr - HP: {loaded.zephyrHealth}, ATK: {loaded.zephyrAttackPower}, DEF: {loaded.zephyrDefence}", "DEBUG");

                // Imposta la stanza corrente del giocatore, fallback su "Grotta".
                if (rooms.ContainsKey(loaded.CurrentRoomName))
                    player.CurrentRoom = rooms[loaded.CurrentRoomName];
                else
                    player.CurrentRoom = rooms["Grotta"];

                // Ricostruisce lo stack inventario dal salvataggio.
                if (loaded.Inventory != null)
                {
                    var reconstructedItems = new List<GameItem>();
                    foreach (var item in loaded.Inventory)
                    {
                        if (item.Type == "Equipment")
                        {
                            // Ricostruisci Equipment
                            var eq = new Equipment(
                                item.Name,
                                item.CanBePickedUp,
                                item.Weight,
                                item.AttackPower ?? 0,
                                item.Defence ?? 0,
                                item.Health ?? 0,
                                item.Description);
                            reconstructedItems.Add(eq);
                        }
                        else
                        {
                            // Ricostruisci GameItem
                            var gi = new GameItem(
                                item.Name,
                                item.CanBePickedUp,
                                item.Weight,
                                item.Description);
                            reconstructedItems.Add(gi);
                        }
                    }
                    player.Inventory.inventory = new Stack<GameItem>(reconstructedItems.AsEnumerable().Reverse());
                }

                player.currentWeight = loaded.currentWeight;

                // Ripristina lo stato degli enigmi e variabili di gioco.
                lockpuzzle.cont = loaded.LockPuzzle;
                keeperpuzzle.cont = loaded.KeeperPuzzle;
                keypuzzle.cont = loaded.KeyPuzzle;
                altarpuzzle.cont = loaded.AltarPuzzle;
                keypuzzle.portboc = loaded.PortBloc;
                teleport.telAtt = loaded.TelAtt;
                teleport.firstTeleportDone = loaded.firstTeleport;
                altarpuzzle.cancelloChiuso = loaded.CancelloChiuso;
                dialogue.introZephyr = loaded.IntroZephyr;
                dialogue.condriSpawned = loaded.CondriSpawned;
                keypuzzle.discoveredStudio = loaded.DiscoveredStudio;
                teleport.discoveredSecretRoom = loaded.DiscoveredSecretRoom;

                // Ripristina la velocità di digitazione globale
                GameLoop.GlobalTypingSpeed = loaded.GlobalTypingSpeed;

                //Ripristina oggetti speciali nelle stanze se mancanti
                RestoreItemIfMissing("Cripta", new GameItem("Pergamena Magica", true, 1, "Una pergamena antica dal grande potere.")
                { CanBePickedUp = loaded.MagicScrollPickable },
                                    () => keeperpuzzle.cont && loaded.MagicScrollPickable);

                RestoreItemIfMissing("Studio Alchemico", new GameItem("Chiave Magica", true, 1, "Una chiave infusa di magia.")
                { CanBePickedUp = loaded.MagicKeyPickable },
                                    () => keypuzzle.cont && loaded.MagicKeyPickable);

                RestoreItemIfMissing("Santuario", new Equipment("Master Sword", true, 1, 20, 5, 0, "La spada leggendaria che può essere estratta solo da chi è degno.")
                { CanBePickedUp = loaded.MasterSwordPickable },
                                    () => altarpuzzle.cont && loaded.MasterSwordPickable);

                // Ricollega le stanze in base allo stato degli enigmi.
                if (lockpuzzle.cont == true)
                {
                    salone.ConnectRoom("sud", ingresso);
                    ingresso.ConnectRoom("nord", salone);
                }

                if (keypuzzle.portboc == false)
                {
                    studioAlchemico.ConnectRoom("ovest", salone);
                    salone.ConnectRoom("est", studioAlchemico);
                }

                if (altarpuzzle.cancelloChiuso == false)
                {
                    salone.ConnectRoom("nord", santuario);
                    santuario.ConnectRoom("sud", salone);
                }

                // Gestione del personaggio Zephyr in stanza Salone.
                if (altarpuzzle.cont == true)
                {
                    if (!salone.characters.Any(c => c is Zephyr))
                    {
                        zephyr = new Zephyr();
                        salone.AddCharacter(zephyr);
                        Logger.Log("DEBUG: Creo nuova istanza Zephyr", "DEBUG");
                    }
                    else
                    {
                        zephyr = (Zephyr)salone.characters.First(c => c is Zephyr);
                        Logger.Log("DEBUG: Zephyr già presente", "DEBUG");
                    }
                    zephyr.Health = loaded.zephyrHealth;
                    zephyr.AttackPower = loaded.zephyrAttackPower;
                    zephyr.Defence = loaded.zephyrDefence;
                    Logger.Log($"DEBUG: Zephyr valori dopo caricamento - HP: {zephyr.Health}, ATK: {zephyr.AttackPower}, DEF: {zephyr.Defence}", "DEBUG");
                }

                // Ripristina le statistiche del giocatore.
                player.Health = loaded.playerHealth;
                player.AttackPower = loaded.playerAttackPower;
                player.Defence = loaded.playerDefence;

                // Ripristina lo stato di visita delle stanze.
                if (loaded.RoomVisitedStates != null)
                {
                    foreach (var pair in loaded.RoomVisitedStates)
                    {
                        if (rooms.TryGetValue(pair.Key, out var room))
                        {
                            room.hasBeenVisited = pair.Value;
                        }
                    }
                }

                // Ripristina gli equipaggiamenti nelle stanze se mancanti e non presenti nell'inventario.
                if (loaded.WorldEquipments != null)
                {
                    foreach (var kvp in loaded.WorldEquipments)
                    {
                        if (rooms.TryGetValue(kvp.Key, out var room))
                        {
                            foreach (var equipment in kvp.Value)
                            {
                                if (room.GetItem(equipment.Name) == null && !player.Inventory.inventory.Any(i => i.Name == equipment.Name))
                                {
                                    room.AddItem(equipment);
                                }
                            }
                        }
                    }
                }

                if (loaded.QuestItems != null)
                {
                    foreach (var kvp in loaded.QuestItems)
                    {
                        if (rooms.TryGetValue(kvp.Key, out var room))
                        {
                            foreach (var questItem in kvp.Value)
                            {
                                if (room.GetItem(questItem.Name) == null && !player.Inventory.inventory.Any(i => i.Name == questItem.Name))
                                {
                                    room.AddItem(questItem);
                                }
                            }
                        }
                    }
                }

                //Ripristina gli oggetti dell'enigma altarpuzzle
                if (!altarpuzzle.cont)
                {
                    var santuario = GameLoop.santuario;

                    if (!ItemExistsAnywhere(altarpuzzle.moneta.Name))
                    {
                        santuario.AddItem(altarpuzzle.moneta);
                        Logger.Log($"{altarpuzzle.moneta.Name} ripristinata nel santuario.", "DEBUG");
                    }

                    if (!ItemExistsAnywhere(altarpuzzle.clessidra.Name))
                    {
                        santuario.AddItem(altarpuzzle.clessidra);
                        Logger.Log($"{altarpuzzle.clessidra.Name} ripristinata nel santuario.", "DEBUG");
                    }

                    if (!ItemExistsAnywhere(altarpuzzle.cuore.Name))
                    {
                        santuario.AddItem(altarpuzzle.cuore);
                        Logger.Log($"{altarpuzzle.cuore.Name} ripristinata nel santuario.", "DEBUG");
                    }
                }

                if (loaded.DeaIsPresent)
                {
                    var deaInSantuario = santuario.characters.OfType<GameCharacter>().FirstOrDefault(c => c.Name == "Dea");

                    if (deaInSantuario == null)
                    {
                        Dea = new GameCharacter("Dea", "Una dea benevola, custode della 'Master Sword'.", true);
                        santuario.AddCharacter(Dea);
                        Logger.Log("DEBUG: Dea spawnata nel Santuario al caricamento.", "DEBUG");
                    }
                    else
                    {
                        Logger.Log("WARN: Dea gia' presente in Santuario!", "WARN");
                    }
                }
                else
                {
                    Logger.Log("DEBUG: Dea non presente nel salvataggio.", "DEBUG");
                }

                //ripristina gli oggetti dell'enigma KeyPuzzle
                if (!keypuzzle.cont)
                {
                    var studioAlchemico = GameLoop.studioAlchemico;

                    if (!ItemExistsAnywhere(keypuzzle.chiaveMagica.Name))
                    {
                        studioAlchemico.AddItem(keypuzzle.chiaveMagica);
                        keypuzzle.chiaveMagica.CanBePickedUp = loaded.MagicKeyPickable;
                        Logger.Log($"{keypuzzle.chiaveMagica.Name} ripristinata nello Studio Alchemico.", "DEBUG");
                    }
                }

                //ripristina gli oggetti dell'enigma KeeperPuzzle
                if (!keeperpuzzle.cont)
                {
                    var cripta = GameLoop.cripta;

                    if (!ItemExistsAnywhere(keeperpuzzle.pergamenaMagica.Name))
                    {
                        cripta.AddItem(keeperpuzzle.pergamenaMagica);
                        keeperpuzzle.pergamenaMagica.CanBePickedUp = loaded.MagicScrollPickable;
                        Logger.Log($"{keeperpuzzle.pergamenaMagica.Name} ripristinata nella Cripta.", "DEBUG");
                    }
                }


                // Stampa informazioni di stato a video dopo il caricamento.
                WriteLine("\nSYSTEM: Gioco caricato.");
                WriteLine($"\nNome: {loaded.PlayerName}");
                WriteLine($"Posizione: {loaded.CurrentRoomName}");
                WriteLine("Inventario: " + string.Join(", ", loaded.Inventory.Select(i => i.Name)));
                WriteLine($"Peso trasportato: {loaded.currentWeight}/{loaded.MaxCarryWeight}");
                WriteLine($"Statistiche personaggio: {loaded.playerAttackPower} ATK | {loaded.playerDefence} DEF | {loaded.playerHealth} HP");
                if (teleport.telAtt == true) WriteLine("Teletrasporto: Attivo");
                else WriteLine("Teletrasporto: Non attivo");
                WriteLine("\nStato Enigmi:");
                if (lockpuzzle.cont == true) WriteLine("Enigma Serratura: Risolto");
                else WriteLine("Enigma Serratura: Non risolto");
                if (keeperpuzzle.cont == true) WriteLine("Enigma Statua: Risolto");
                else WriteLine("Enigma Statua: Non risolto");
                if (keypuzzle.cont == false) WriteLine("Enigma Calderone: Non risolto");
                else WriteLine("Enigma Calderone: Risolto");
                if (altarpuzzle.cont == true) WriteLine("Enigma Altare: Risolto");
                else WriteLine("Enigma Altare: Non risolto");
                await WriteSlowly("\nSYSTEM: Ricorda che i nomi menzionati nei dialoghi che sono tra '' sono oggetti o persone con qui puoi interagire.");
                await WriteSlowly("\nSYSTEM: Premi un qualsiasi tasto per continuare...");
                ReadKey(true);
                Clear();
                return true; // Caricamento riuscito
            }
            else
            {
                // Gestione caso nessun salvataggio trovato.
                await WriteSlowly("\nSYSTEM: Devi prima iniziare una nuova partita e salvarla per poterla caricare.");
                await WriteSlowly("\nSYSTEM: Premi un qualsiasi tasto per tornare al menu principale...");
                Logger.Log("Tentativo di caricamento senza file di salvataggio esistente", "WARN");
                ReadKey(true);
                return false; // Caricamento fallito, nessun salvataggio trovato
            }

        }

        /// <summary>
        /// Verifica se un oggetto con il nome specificato esiste ovunque, 
        /// sia nell'inventario del giocatore che in tutte le stanze del gioco.
        /// </summary>
        /// <param name="itemName">Il nome dell'oggetto da cercare.</param>
        /// <returns><c>true</c> se l'oggetto esiste nell'inventario o in una qualsiasi stanza, <c>false</c> altrimenti.</returns>
        public static bool ItemExistsAnywhere(string itemName)
        {
            // Controlla nell'inventario del giocatore
            if (player.Inventory.HasItem(itemName))
                return true;

            // Controlla in tutte le stanze
            foreach (var room in rooms.Values)
            {
                if (room.items.Any(i => i.Name == itemName))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Ripristina un oggetto in una stanza se è stato rimosso e soddisfa le condizioni di rigenerazione.
        /// </summary>
        /// <param name="roomName">Il nome della stanza in cui tentare il ripristino dell'oggetto.</param>
        /// <param name="itemToRestore">L'oggetto da ripristinare.</param>
        /// <param name="creationCondition">Una funzione che restituisce <c>true</c> se l'oggetto deve essere rigenerato.</param>
        /// <remarks>
        /// <para>- La stanza deve esistere.</para>
        /// <para>- L'oggetto non deve già essere presente nella stanza.</para>
        /// <para>- L'oggetto non deve essere già presente nell'inventario del giocatore.</para>
        /// <para>- La funzione <paramref name="creationCondition"/> deve restituire <c>true</c>.</para>
        /// </remarks>
        private static void RestoreItemIfMissing(string roomName, GameItem itemToRestore, Func<bool> creationCondition)
        {
            if (!rooms.TryGetValue(roomName, out var room))
                return;

            if (room.GetItem(itemToRestore.Name) != null)
                return;

            if (player.Inventory.inventory.Any(i => i.Name == itemToRestore.Name))
                return;

            if (creationCondition())
            {
                room.AddItem(itemToRestore);
                Logger.Log($"Oggetto '{itemToRestore.Name}' rigenerato nella stanza '{roomName}'", "INFO");
            }
        }
    }
}
