global using System;
global using static System.Console;
global using static ZephyrLabyrinth.GameLoop;
global using System.Collections.Generic;
global using System.IO;
global using System.Linq;
global using System.Threading.Tasks;
global using System.Text.Json;

namespace ZephyrLabyrinth
{
    /// <summary> Classe principale del gioco contenente il ciclo di gioco, le variabili globali, e i metodi di inizializzazione e gestione comandi.</summary>
    class GameLoop
    {
        // Global variables

        /// <summary>Istanza globale del giocatore. Può essere null se non inizializzato.</summary>
        public static Player? player;
        /// <summary>Gestore del sistema di teletrasporto.</summary>
        public static Teleport teleport = new Teleport();
        // <summary>Dizionario contenente tutte le stanze del gioco, indicizzate per nome.</summary>
        public static Dictionary<string, Room> rooms;
        /// <summary>Generatore casuale condiviso per eventi aleatori.</summary>
        public static Random random = new Random();
        /// <summary>Gestore del suggerimento comandi nel gioco.</summary>        
        public static HelpW helpW = new HelpW();
        /// <summary>Gestore dei dialoghi del gioco.</summary>
        public static Dialogue dialogue = new Dialogue();
        /// <summary>Gestore della battaglia contro Zephyr.</summary>
        public static ZephyrBattle battle = new ZephyrBattle();
        /// <summary>Gestore enigma calderone.</summary> 
        public static KeyPuzzle keypuzzle = new KeyPuzzle();
        /// <summary>Gestore enigma statua.</summary>
        public static KeeperPuzzle keeperpuzzle = new KeeperPuzzle();
        /// <summary>Gestore enigma altare.</summary>
        public static Altarpuzzle altarpuzzle = new Altarpuzzle();
        /// <summary>Gestore enigma serratura.</summary>
        public static LockPuzzle lockpuzzle = new LockPuzzle();
        /// <summary>Configurazione iniziale degli elementi interattivi nel mondo di gioco.</summary>
        public static InteractiveConfig interactiveConfig = new InteractiveConfig();
        /// <summary>Istanza globale di Zephyr, il boss. Può essere null se non ancora presente nel gioco.</summary>
        public static Zephyr? zephyr;
        /// <summary>Istanza globale del personaggio Dea, se presente nel Santuario.</summary>
        public static GameCharacter? Dea;
        /// <summary>Riferimento globale alla stanza del Santuario.</summary>
        public static Room? santuario;
        /// <summary>Riferimento globale alla stanza segreta.</summary>
        public static Room? stanzaSegreta;
        /// <summary>Riferimento globale alla stanza del Salone.</summary>
        public static Room? salone;
        /// <summary>Riferimento globale alla stanza dello Studio Alchemico.</summary>
        public static Room? studioAlchemico;
        /// <summary>Riferimento globale alla stanza della Grotta.</summary>
        public static Room? grotta;
        /// <summary>Riferimento globale alla stanza della Cripta.</summary>
        public static Room? cripta;
        /// <summary>Riferimento globale alla stanza dell'Ingresso.</summary>
        public static Room? ingresso;
        
        /// <summary> Velocità di digitazione globale (configurabile).</summary>
        public static int GlobalTypingSpeed = 40; // Velocità di default, può essere modificata dall'utente nel menù partita configurata

        /// <summary>
        /// Punto di ingresso asincrono principale del gioco.
        /// Gestisce il ciclo principale di avvio menu, input comandi e terminazione.
        /// </summary>
        /// <returns> Task asincrono che rappresenta l'esecuzione del gioco.</returns>
        static async Task Main()
        {
            bool gameRunning = true;
            while (gameRunning)
            {
                bool gameStarted = false;
                Clear();
                await dialogue.MenuOption();

                while (!gameStarted && gameRunning)
                {
                    await WriteSlowly("SYSTEM: Scegli un'opzione: ", -1, false);
                    string? scelta = ReadLine()?.Trim();
                    (gameStarted, gameRunning) = await ProcessMenuChoice(scelta);
                }

                while (gameStarted && gameRunning)
                {
                    if (dialogue.EndBattle == false)
                    {
                        await WriteSlowly("\nSYSTEM: Inserisci un comando ('help' per legenda comandi): ", -1, false);
                        string? command = ReadLine()?.Trim().ToLower();

                        if (!string.IsNullOrEmpty(command))
                        {
                            gameStarted = await ProcessCommand(command);
                            if (!gameStarted)
                                gameRunning = false;
                        }
                    }
                    else
                    {
                        gameStarted = false;
                    }
                }
            }
        }

        /// <summary>
        /// Inizializza il mondo di gioco, le stanze, i personaggi, gli oggetti e carica eventuale configurazione personalizzata.
        /// </summary>
        /// <param name="customConfig">Configurazione personalizzata da utilizzare.</param>
        /// <returns> Task asincrono che rappresenta l'inizializzazione del mondo gioco.</returns>
        static void InitializeGame(GameConfig? customConfig = null)
        {
            player = new Player("Player", 10, 100, 10, 5);

            // Create rooms
            stanzaSegreta = new Room("Stanza Segreta", "\nUna stanza segreta raggiungibile con tanta fortuna, chissa' cosa si cela al suo interno...");
            grotta = new Room("Grotta", "\nUna grotta umida e tetra rivelatasi dalla voragine.");
            ingresso = new Room("Ingresso", "\nEntrata del labirinto, un enorme portone recante una 'incisione' sospetta ha una 'serratura' insolita.");
            salone = new Room("Salone", "\nUn enorme stanza circolare, collegata da tre porte a sud, est e ovest e da un 'cancello' a nord.");
            studioAlchemico = new Room("Studio Alchemico", "\nUn laboratorio ricco di oggetti per la creazione di pozioni. Sono presenti un 'calderone' fumante e un 'tavolino' con strani utensili.");
            cripta = new Room("Cripta", "\nUna stanza cupa e oscura, al centro un'enorme 'statua' raffigurante uno stregone sembra osservarti.");
            santuario = new Room("Santuario", "\nUn silenzio sacro avvolge la sala esagonale, un grande mosaico di vetro filtra la luce proveniente dall'esterno illuminando un 'altare' al centro della stanza.");

            //dizionario per associare nome stanza alla sua istanza
            rooms = new Dictionary<string, Room>
            {
                { "Salone", salone },
                { "Santuario", santuario },
                { "Studio Alchemico", studioAlchemico },
                { "Stanza Segreta", stanzaSegreta},
                { "Ingresso", ingresso},
                { "Cripta", cripta},
                { "Grotta" , grotta}
            };

            // Connect rooms
            grotta.ConnectRoom("nord", ingresso);
            ingresso.ConnectRoom("sud", grotta);
            salone.ConnectRoom("ovest", cripta);
            cripta.ConnectRoom("est", salone);

            // Place characters
            salone.AddCharacter(new GameCharacter("Elara", "Un'elfa magica intrappolata con te.", true));
            studioAlchemico.AddCharacter(new GameCharacter("Condri", "Un piccolo esserino, dotato di grandi poteri magici al servizio del creatore del labirinto.", false));

            // Place interactables
            studioAlchemico.AddInteractable(new Interactable("Calderone", "\nContenitore magico di Condri per creare le pozioni", "Studio Alchemico"));
            studioAlchemico.AddInteractable(new Interactable("Tavolino", "\nPiccolo tavolo con strani utensili.", "Studio Alchemico"));
            cripta.AddInteractable(new Interactable("Statua", "\nUn enorme monumento che custodisce un potere proibito del labirinto", "Cripta"));
            ingresso.AddInteractable(new Interactable("Serratura", "\nUn intricato meccanismo numerico che blocca la porta", "Ingresso"));
            ingresso.AddInteractable(new Interactable("Incisione", "\nSembrano delle istruzioni per far funzionare qualcosa", "Ingresso"));
            salone.AddInteractable(new Interactable("Cancello", "\nUn cancello che puo' essere aperto solo tramite una chiave speciale.", "Salone"));
            santuario.AddInteractable(new Interactable("Altare", "\nUn altare antico con tre piedistalli dorati che reggono oggetti consumati dal tempo.", "Santuario"));

            if (!altarpuzzle.cont)
            {
                santuario.AddItem(altarpuzzle.moneta);
                santuario.AddItem(altarpuzzle.clessidra);
                santuario.AddItem(altarpuzzle.cuore);
            }
            
            if (!keypuzzle.cont)
            {
                studioAlchemico.AddItem(keypuzzle.chiaveMagica);
            }

            if (!keeperpuzzle.cont)
            {
                cripta.AddItem(keeperpuzzle.pergamenaMagica);
            }

            // Se c'è una configurazione personalizzata, applicala
            if (customConfig != null && customConfig.player != null)
            {
                player = new Player
                (
                    customConfig.player.name,
                    customConfig.player.maxCarryWeight,
                    customConfig.player.health,
                    customConfig.player.attackPower,
                    customConfig.player.defence
                );

                player.CurrentRoom = rooms.ContainsKey(customConfig.player.startRoom) ? rooms[customConfig.player.startRoom] : rooms["Grotta"];

                zephyr = new Zephyr()
                {
                    Health = customConfig.zephyr.health,
                    AttackPower = customConfig.zephyr.attackPower,
                    Defence = customConfig.zephyr.defence
                };

                if (customConfig.puzzle != null)
                {
                    lockpuzzle.cont = customConfig.puzzle.LockPuzzle;
                    keeperpuzzle.cont = customConfig.puzzle.KeeperPuzzle;
                    keypuzzle.cont = customConfig.puzzle.KeyPuzzle;
                    keypuzzle.portboc = customConfig.puzzle.PortBloc;
                    altarpuzzle.cont = customConfig.puzzle.altarpuzzle;
                    teleport.telAtt = customConfig.puzzle.TelAtt;
                    altarpuzzle.cancelloChiuso = customConfig.puzzle.CancelloChiuso;
                    var magicScroll = cripta.GetItem("Pergamena Magica");
                    if (magicScroll != null)
                        magicScroll.CanBePickedUp = customConfig.puzzle.MagicScrollPickable;
                    if (keypuzzle.cont)
                    {
                        var magicKey = studioAlchemico.GetItem("Chiave Magica");
                        if (magicKey != null)
                            magicKey.CanBePickedUp = true;
                    }
                    if (!altarpuzzle.cancelloChiuso)
                    {
                        salone.ConnectRoom("nord", santuario);
                        santuario.ConnectRoom("sud", salone);
                    }
                    if (lockpuzzle.cont)
                    {
                        ingresso.ConnectRoom("nord", salone);
                        salone.ConnectRoom("sud", ingresso);
                    }
                    if (altarpuzzle.cont)
                    {
                        // Rimuove la clessidra dal santuario se presente
                        var clessidraInRoom = santuario.GetItem("Clessidra");
                        if (clessidraInRoom != null)
                            santuario.items.Remove(clessidraInRoom);

                        // Aggiunge la Master Sword al santuario
                        Equipment masterSword = new Equipment("Master Sword", true, 4, 20, 0, 0, "La spada leggendaria che può essere estratta solo da chi è degno.");
                        santuario.AddItem(masterSword);

                        // Spawna Zephyr nel salone
                        salone.AddCharacter(new Zephyr());

                        // Spawna la Dea nel santuario
                        GameCharacter dea = new GameCharacter("Dea", "Una dea benevola, custode della 'Master Sword'.", true);
                        santuario.AddCharacter(dea);
                        Dea = dea;

                        Logger.Log("AltarPuzzle completato tramite config: Zephyr, Dea e Master Sword inizializzati", "INFO");
                    }
                }
            }
            else if (player != null)
            {
                player.CurrentRoom = rooms["Grotta"];
            }
        }
        
        /// <summary> Inizializza gli equipaggiamenti disponibili nelle stanze del gioco.</summary>
        /// <returns> Task asincrono che rappresenta l'inizializzazione degli equipaggiamenti.</returns>
        static void InitializeEquipment()
        {
            GameItem.mappa = new GameItem("Mappa", true,1, "Appunti di Condri per orientarsi nel labririnto");
            salone?.AddItem(GameItem.mappa);
            Equipment.candela = new Equipment("Candela nera", true, 1, 2, 0, 0, "Una candela nera che emana un odore sgradevole.");
            cripta?.AddItem(Equipment.candela);
            Equipment.frammentiOssidiana = new Equipment("Frammenti di Ossidiana", true, 1, 0, 2, 0, "Frammenti di un'antica pietra nera.");
            cripta?.AddItem(Equipment.frammentiOssidiana);
            Equipment.anelloArgento = new Equipment("Anello d'argento", true, 1, 0, 3, 0, "Un anello d'argento lucente.");
            cripta?.AddItem(Equipment.anelloArgento);
            Equipment.palantir = new Equipment("Palantir", true, 2, 0, 0, 5, "Sfera di cristallo che permette di vedere altri luoghi e altri tempi.");
            salone?.AddItem(Equipment.palantir);
            Equipment.specchio = new Equipment("Specchio", true, 3, 3, 3, 0, "Uno specchio che riflette la tua vera immagine.");
            salone?.AddItem(Equipment.specchio);
            Equipment.necronomicon = new Equipment("Necronomicon", true, 2, 0, 0, 3, "Un libro oscuro e misterioso.");
            salone?.AddItem(Equipment.necronomicon);
            Equipment.mela = new Equipment("Mela d'oro", true, 1, 1, 0, 2, "Una mela d'oro che brilla di luce propria.");
            studioAlchemico?.AddItem(Equipment.mela);
            Equipment.pietraFilosofale = new Equipment("Pietra Filosofale", true, 3, 3, 0, 3, "Una pietra che si dice possa trasformare il metallo in oro.");
            studioAlchemico?.AddItem(Equipment.pietraFilosofale);
            Equipment.teschioCristallo = new Equipment("Teschio di cristallo", true, 2, 1, 1, 2, "Un teschio di cristallo che emana un'aura misteriosa.");
            studioAlchemico?.AddItem(Equipment.teschioCristallo);
            Equipment.fiammaPrometeo = new Equipment("Fiamma di Prometeo", true, 4, 4, 0, 5, "Una fiamma che brucia eternamente, simbolo di conoscenza e potere.");
            stanzaSegreta?.AddItem(Equipment.fiammaPrometeo);
            Equipment.behelit = new Equipment("Behelit", true, 2, 0, 5, 0, "Un uovo rosso con un volto umanoide e grottesco.");
            stanzaSegreta?.AddItem(Equipment.behelit);
            Equipment.santoGraal = new Equipment("Santo Graal", true, 5, 0, 0, 100, "Il leggendario Santo Graal, fonte di potere infinito.");
            stanzaSegreta?.AddItem(Equipment.santoGraal);
            Equipment.bastone = new Equipment("Bastone del finto potere", true, 1, 0, 0, 0, "Un bastone che non ha nessuna utilità.");
            grotta?.AddItem(Equipment.bastone);
            Equipment.torcia = new Equipment("Torcia", true, 1, 1, 0, 0, "Una torcia spenta.");
            ingresso?.AddItem(Equipment.torcia);
            Equipment.osso = new Equipment("Femore", true, 1, 0, 0, 1, "Un osso umano.");
            ingresso?.AddItem(Equipment.osso);

        }

        /// <summary> Ricollega i riferimenti statici della classe Equipment agli oggetti effettivi presenti nelle stanze,
        /// dopo il caricamento di una partita salvata.</summary>
        /// <remarks> Non ritorna nulla (void).</remarks>
        static void ReInitializeEquipment()
        {
           Equipment.candela = Inventory.GetItemAnywhere("Candela nera") as Equipment;
           Equipment.frammentiOssidiana = Inventory.GetItemAnywhere("Frammenti di Ossidiana") as Equipment;
           Equipment.anelloArgento = Inventory.GetItemAnywhere("Anello d'argento") as Equipment;

           GameItem.mappa = Inventory.GetItemAnywhere("Mappa") ?? new GameItem("Mappa", true, 1, "Appunti di Condri per orientarsi nel labirinto");

           Equipment.palantir = Inventory.GetItemAnywhere("Palantir") as Equipment;
           Equipment.specchio = Inventory.GetItemAnywhere("Specchio") as Equipment;
           Equipment.necronomicon = Inventory.GetItemAnywhere("Necronomicon") as Equipment;

           Equipment.mela = Inventory.GetItemAnywhere("Mela d'oro") as Equipment;
           Equipment.pietraFilosofale = Inventory.GetItemAnywhere("Pietra Filosofale") as Equipment;
           Equipment.teschioCristallo = Inventory.GetItemAnywhere("Teschio di Cristallo") as Equipment;

           Equipment.fiammaPrometeo = Inventory.GetItemAnywhere("Fiamma di Prometeo") as Equipment;
           Equipment.behelit = Inventory.GetItemAnywhere("Behelit") as Equipment;
           Equipment.santoGraal = Inventory.GetItemAnywhere("Santo Graal") as Equipment;

           Equipment.bastone = Inventory.GetItemAnywhere("Bastone del finto potere") as Equipment;
           Equipment.torcia = Inventory.GetItemAnywhere("Torcia") as Equipment;
           Equipment.osso = Inventory.GetItemAnywhere("Femore") as Equipment;
        }

        /// <summary> Stampa il testo a schermo lentamente, con un ritardo opzionale tra i caratteri.</summary>
        /// <param name="text">Testo da stampare.</param>
        /// <param name="delay">Ritardo in millisecondi tra la stampa di ogni carattere. Se -1, usa GlobalTypingSpeed.</param>
        /// <param name="nextLine">Se true, aggiunge una nuova riga alla fine del testo.</param>
        /// <returns> Task asincrono che rappresenta l'operazione di scrittura.</returns>
        public static async Task WriteSlowly(string text, int delay = -1, bool nextLine = true)
        {
            // Se delay è -1, usa la velocità globale configurabile
            int actualDelay = delay == -1 ? GlobalTypingSpeed : delay;
            
            foreach (char c in text)
            {
                Write(c);
                if (actualDelay > 0)
                    await Task.Delay(actualDelay);
            }
            if (nextLine)
            {
                WriteLine();
            }
        }
      
        /// <summary> Gestisce i comandi inseriti dal giocatore e esegue le relative azioni.</summary>
        /// <param name="command">Comando inserito dal giocatore.</param>
        /// <returns> Task&lt;bool&gt; che indica se il gioco deve continuare o meno.</returns>
        /// <remarks> True se il gioco deve continuare, false se deve terminare.</remarks>
        static async Task<bool> ProcessCommand(string command)
        {
            string[] parts = command.Split(' ', 2);
            string action = parts[0];
            string target = parts.Length > 1 ? parts[1] : string.Empty;

            switch (action)
            {
                case "move":
                    if (!string.IsNullOrEmpty(target))
                    {
                        await player.Move(target);
                        Logger.Log($"{player.Name} si è spostato in {player.CurrentRoom}.", "INFO");
                    }
                    else
                    {
                        WriteLine("\nSYSTEM: Muovi verso dove? Specifica una direzione (nord, sud, est, ovest).");
                        Logger.Log($"Il giocatore non ha specificato la direzione", "WARN");
                    }
                    break;

                case "take":
                    if (!string.IsNullOrEmpty(target)) await player.Inventory.TakeItem(target);
                    else WriteLine("\nSYSTEM: Prendere cosa? Specifica un oggetto.");
                    Logger.Log($"Il giocatore non ha specificato un oggetto da raccogliere.", "WARN");
                    
                    break;

                case "drop":
                    if (!string.IsNullOrEmpty(target)) await player.Inventory.DropItem(target);
                    else WriteLine("\nSYSTEM: Lasciare cosa? Specifica un oggetto.");
                    Logger.Log($"Il giocatore non ha specificato un oggetto da droppare.", "WARN");
                    break;

                case "inventory":
                    await player.Inventory.ShowInventory();
                    break;
                
                case "stats":
                    await player.ShowStats();
                    break;

                case "talk":
                    if (!string.IsNullOrEmpty(target)) await player.Talk(target);
                    else WriteLine("\nSYSTEM: Parla con chi? Specifica una persona.");
                    Logger.Log($"Il giocatore non ha specificato un gamecharacter con cui parlare.", "WARN");
                    break;

                case "interact":
                    if (!string.IsNullOrEmpty(target)) await player.Interact(target);
                    else WriteLine("\nSYSTEM: Interagire con cosa? Specifica un oggetto.");
                    Logger.Log($"Il giocatore non ha specificato un interactable con cui interagire.", "WARN");
                    break;

                case "observe":
                    if (!string.IsNullOrEmpty(target)) await player.Observe(target);
                    else WriteLine("\nSYSTEM: Osserva cosa? Specifica se room o items.");
                    Logger.Log($"Il giocatore non ha specificato cosa osservare.", "WARN");
                    break;

                case "map":
                    await player.Map();
                    break;

                case "teleport":
                    await teleport.Teleportation();
                    break;

                case "save":
                    GameState.Save();
                    Logger.Log("Il giocatore ha salvato la partita corrente", "INFO");
                    break;

                case "load":
                    await GameState.LoadGame();
                    Logger.Log("Il giocatore ha caricato una partita salvata", "INFO");
                    break;

                case "help":
                    HelpW.ShowHelpWindow();
                    Logger.Log("Il giocatore ha aperto l'help menu dei comandi", "INFO");
                    break;

                case "quit":
                    Logger.Log("Il giocatore sta chiudendo il gioco", "INFO");
                    await WriteSlowly("\nSYSTEM: Grazie per aver giocato! Arrivederci...........\n", 0, true);
                    return false;

                default:
                    WriteLine("\nSYSTEM: Comando non valido. Digita 'help' per la lista dei comandi.\n");
                    Logger.Log("Il giocatore ha digitato un comando non valido", "WARN");
                    break;
            }
            return true;
        }
      
        /// <summary> Gestisce la scelta del menu principale e ritorna lo stato di avvio e continuazione del gioco.</summary>
        /// <param name="scelta">Scelta inserita dall'utente.</param>
        /// <returns> Tuple contenente lo stato di avvio del gioco e se il gioco deve continuare.
        /// <br/>  - gameStarted: true se il gioco è stato avviato con successo.
        /// <br/>  - gameRunning: true se il gioco deve continuare a funzionare (false per uscire).</returns>
        private static async Task<(bool gameStarted, bool gameRunning)> ProcessMenuChoice(string? scelta)
        {
            bool gameStarted = false;
            bool gameRunning = true;
            switch (scelta)
            {
                case "1":
                    Clear();
                    await WriteSlowly("Vuoi cancellare il log precedente? (s/n) ");
                    string? risposta = ReadLine()?.Trim().ToLower();
                    if (risposta == "s")
                        Logger.ClearLog();
                    Logger.NewSession();
                    Clear();

                    InitializeGame();
                    InitializeEquipment();
                    await dialogue.Introduzione();
                    await player.PlayerInsertName();
                    await dialogue.Introduzione2();

                    gameStarted = true;
                    dialogue.EndBattle = false;
                    break;

                case "2":
                    Clear();
                    Logger.NewSession();
                    InitializeGame();
                    bool loadSuccess = await GameState.LoadGame();
                    ReInitializeEquipment();

                    if (loadSuccess)
                        gameStarted = true;
                    else
                    {
                        // Se il caricamento fallisce, mostra di nuovo il menu
                        Clear();
                        await dialogue.MenuOption();
                    }

                    dialogue.EndBattle = false;
                    break;

                case "3":
                    Clear();
                    await WriteSlowly("\nSYSTEM: Uscita dal programma...\n");
                    gameRunning = false;
                    gameStarted = false; //qui devi settarlo a false perché esci dal programma
                    break;

                case "4":
                    Clear();
                    Logger.NewSession();
                    await WriteSlowly("SYSTEM: Avvio configurazione interattiva...\n");
                    
                    var customConfig = await interactiveConfig.StartInteractiveConfiguration();
                    
                    if (customConfig != null)
                    {
                        Clear();
                        await WriteSlowly("SYSTEM: Inizializzo il gioco con la configurazione personalizzata...\n");
                        InitializeGame(customConfig);
                        InitializeEquipment();
                        
                        // Se il giocatore ha personalizzato il nome, non mostrare l'introduzione completa
                        if (customConfig.player.name != "Player")
                        {
                            await WriteSlowly($"SYSTEM: Benvenuto {customConfig.player.name}! Il gioco inizia dalla stanza: {customConfig.player.startRoom}\n");
                            await WriteSlowly($"SYSTEM: Le tue statistiche: {player.AttackPower} ATK | {player.Defence} DEF | {player.Health} HP\n");
                            gameStarted = true;
                        }
                        else
                        {
                            // Mostra introduzione normale e chiedi il nome
                            await dialogue.Introduzione();
                            await player.PlayerInsertName();
                            await dialogue.Introduzione2();
                            gameStarted = true;
                        }
                        
                        dialogue.EndBattle = false;
                    }
                    else
                    {
                        // L'utente ha annullato la configurazione, torna al menu
                        Clear();
                        await dialogue.MenuOption();
                    }
                    break;

                default:
                    await WriteSlowly("\nSYSTEM: Scelta non valida. Riprova.\n");
                    break;
            }

            return (gameStarted, gameRunning);
        }
    }
}
