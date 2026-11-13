using System.Text.Json;
using static ZephyrLabyrinth.GameLoop;

namespace ZephyrLabyrinth
{
    /// <summary>
    /// Classe per gestire la configurazione interattiva del gioco tramite menu.
    /// Permette al giocatore di personalizzare statistiche, puzzle e variabili prima di iniziare una partita.
    /// </summary>
    public class InteractiveConfig
    {
        ///<summary>Oggetto che contiene configurazione corrente del gioco</summary>
        private GameConfig config;
        ///<summary>Velocita' con cui vengono stampati i caratteri a schermo</summary>
        private int typingSpeed = 30; // Velocità di default (millisecondi tra caratteri)

        /// <summary>
        /// Costruttore che inizializza una configurazione con valori predefiniti.
        /// </summary>
        public InteractiveConfig()
        {
            config = new GameConfig
            {
                player = new PlayerConfig
                {
                    name = "Player",
                    maxCarryWeight = 10,
                    health = 100,
                    attackPower = 10,
                    defence = 5,
                    startRoom = "Grotta"
                },
                zephyr = new ZephyrConfig
                {
                    health = 150,
                    attackPower = 20,
                    defence = 8
                },
                puzzle = new PuzzleConfig
                {
                    LockPuzzle = false,
                    KeeperPuzzle = false,
                    MagicScrollPickable = false,
                    KeyPuzzle = false,
                    PortBloc = false,
                    altarpuzzle = false,
                    TelAtt = false,
                    CancelloChiuso = true
                }
            };
        }

        /// <summary>
        /// Avvia il menu di configurazione interattiva.
        /// </summary>
        /// <returns>La configurazione personalizzata dal giocatore, o null se annullata.</returns>
        public async Task<GameConfig?> StartInteractiveConfiguration()
        {
            bool configuring = true;
            
            await WriteSlowly("=== CONFIGURAZIONE INTERATTIVA ===\n");
            await WriteSlowly("Benvenuto nel sistema di configurazione avanzata!\n");
            await WriteSlowly("Qui puoi personalizzare tutte le impostazioni del gioco prima di iniziare.\n");

            while (configuring)
            {
                await ShowConfigurationMenu();
                string? choice = ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        await ConfigurePlayer();
                        break;
                    case "2":
                        await ConfigureZephyr();
                        break;
                    case "3":
                        await ConfigurePuzzles();
                        break;
                    case "4":
                        await ShowCurrentConfiguration();
                        break;
                    case "5":
                        await SaveConfiguration();
                        break;
                    case "6":
                        await LoadConfiguration();
                        break;
                    case "7":
                        await ConfigureTypingSpeed();
                        break;
                    case "8":
                        configuring = false;
                        return config;
                    case "9":
                        return null; // Annulla configurazione
                    default:
                        await WriteSlowly("Opzione non valida. Riprova.\n");
                        break;
                }

                if (configuring)
                {
                    await WriteSlowly("\nPremi INVIO per continuare...");
                    ReadLine();
                    Clear();
                }
            }

            return config;
        }

        /// <summary>
        /// Mostra il menu principale di configurazione.
        /// </summary>
        private async Task ShowConfigurationMenu()
        {
            await WriteSlowly("\n=== MENU CONFIGURAZIONE ===\n");
            await WriteSlowly("1. Configura Giocatore\n");
            await WriteSlowly("2. Configura Zephyr\n");
            await WriteSlowly("3. Configura Puzzle\n");
            await WriteSlowly("4. Visualizza Configurazione Attuale\n");
            await WriteSlowly("5. Salva Configurazione\n");
            await WriteSlowly("6. Carica Configurazione\n");
            await WriteSlowly("7. Configura Velocità Testo\n");
            await WriteSlowly("8. Inizia Partita con Questa Configurazione\n");
            await WriteSlowly("9. Annulla e Torna al Menu Principale\n");
            await WriteSlowly("\nScegli un'opzione: ", 0, false);
        }

        /// <summary>
        /// Configura le statistiche del giocatore.
        /// </summary>
        private async Task ConfigurePlayer()
        {
            await WriteSlowly("\n=== CONFIGURAZIONE GIOCATORE ===\n");
            await WriteSlowly("Inserire le statistiche del giocatore. (INVIO con valore vuoto per mantenere il valore attuale)\n");
            await WriteSlowly($"Nome attuale: {config.player.name}\n");
            await WriteSlowly("Inserisci nuovo nome: ", 0, false);
            string? newName = ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(newName))
                config.player.name = newName;

            config.player.health = await GetIntInput($"Salute (attuale: {config.player.health}): ", config.player.health, 1, 1000);
            config.player.attackPower = await GetIntInput($"Potenza Attacco (attuale: {config.player.attackPower}): ", config.player.attackPower, 1, 100);
            config.player.defence = await GetIntInput($"Difesa (attuale: {config.player.defence}): ", config.player.defence, 0, 100);
            config.player.maxCarryWeight = await GetIntInput($"Peso Max Trasportabile (attuale: {config.player.maxCarryWeight}): ", config.player.maxCarryWeight, 1, 100);

            await WriteSlowly("\n=== SELEZIONE STANZA DI PARTENZA ===\n");
            await WriteSlowly("Stanze disponibili:\n");
            await WriteSlowly("- Grotta - Stanza iniziale standard (SICURA)\n");
            await WriteSlowly("- Ingresso - Richiede Lock Puzzle risolto per procedere\n");
            await WriteSlowly("- Salone - Stanza centrale, accesso libero solo alla Cripta\n");
            await WriteSlowly("- Studio Alchemico - Parla con Condri per collegarti liberamente al Salone\n");
            await WriteSlowly("- Cripta - Accessibile liberamente dal Salone (SICURA)\n");
            await WriteSlowly("- Santuario - Dal Salone: richiede chiave magica per aprire il cancello\n");
            await WriteSlowly("- Stanza Segreta - ATTENZIONE: Accessibile SOLO con teletrasporto attivo!\n");
            
            await WriteSlowly($"\nStanza di partenza attuale: {config.player.startRoom}\n");
            await WriteSlowly("Inserisci nuova stanza di partenza (INVIO con valore vuoto per mantenere attuale): ", 0, false);
            string? newRoom = ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(newRoom))
            {
                string[] validRooms = { "Grotta", "Ingresso", "Salone", "Studio Alchemico", "Cripta", "Santuario", "Stanza Segreta" };
                if (validRooms.Contains(newRoom))
                {
                    // Avvertimenti specifici per stanze problematiche
                    if (newRoom == "Stanza Segreta")
                    {
                        await WriteSlowly("\n!!!AVVERTIMENTO!!! : Hai scelto la Stanza Segreta!\n");
                        await WriteSlowly("Questa stanza è accessibile SOLO tramite teletrasporto.\n");
                        await WriteSlowly("Se il teletrasporto non è attivo, il giocatore sarà BLOCCATO!\n");
                        await WriteSlowly("Vuoi continuare comunque? (s/n): ", 0, false);
                        string? confirmSecret = ReadLine()?.Trim().ToLower();
                        if (confirmSecret != "s")
                        {
                            await WriteSlowly("Mantengo la stanza attuale.\n");
                            return;
                        }
                        else
                        {
                            await WriteSlowly("SUGGERIMENTO: Assicurati di attivare il teletrasporto nella configurazione puzzle!\n");
                        }
                    }
                    else if (newRoom == "Santuario")
                    {
                        await WriteSlowly("\n!!!AVVERTIMENTO!!! : Hai scelto il Santuario!\n");
                        await WriteSlowly("Questa stanza richiede che il cancello sia aperto tramite la Chiave Magica o la selezione di apertura cancello.\n");
                        await WriteSlowly("Se il cancello non è aperto, potresti avere difficoltà di navigazione.\n");
                        await WriteSlowly("Vuoi continuare comunque? (s/n): ", 0, false);
                        string? confirmSanctuary = ReadLine()?.Trim().ToLower();
                        if (confirmSanctuary != "s")
                        {
                            await WriteSlowly("Mantengo la stanza attuale.\n");
                            return;
                        }
                    }
                    else if (newRoom == "Ingresso")
                    {
                        await WriteSlowly("\nINFO: Hai scelto l'Ingresso.\n");
                        await WriteSlowly("Se il Lock Puzzle non è risolto, potrai muoverti solo verso la Grotta.\n");
                        await WriteSlowly("Per accedere al Salone, assicurati che il Lock Puzzle sia risolto.\n");
                    }
                    else if (newRoom == "Salone")
                    {
                        await WriteSlowly("\nINFO: Hai scelto il Salone.\n");
                        await WriteSlowly("Se il Lock Puzzle non è risolto, NON potrai tornare all'Ingresso o alla Grotta!\n");
                        await WriteSlowly("Dal Salone puoi accedere liberamente solo alla Cripta.\n");
                        await WriteSlowly("- Studio Alchemico: richiede teletrasporto o collegamento da Condri\n");
                        await WriteSlowly("- Santuario: richiede chiave magica per aprire il cancello\n");
                        await WriteSlowly("Per permettere il ritorno all'Ingresso/Grotta, assicurati che il Lock Puzzle sia risolto.\n");
                    }
                    else if (newRoom == "Studio Alchemico")
                    {
                        await WriteSlowly("\nINFO: Hai scelto lo Studio Alchemico.\n");
                        await WriteSlowly("Puoi parlare con Condri per creare un collegamento permanente con il Salone.\n");
                        await WriteSlowly("Una volta creato il collegamento, potrai muoverti liberamente tra le due stanze.\n");
                    }
                    
                    config.player.startRoom = newRoom;
                    await WriteSlowly($"Stanza di partenza impostata su: {newRoom}\n");
                }
                else
                    await WriteSlowly("Stanza non valida. Mantengo quella attuale.\n");
            }

            await WriteSlowly("Configurazione completata!\n");
        }

        /// <summary>
        /// Configura le statistiche di Zephyr.
        /// </summary>
        private async Task ConfigureZephyr()
        {
            await WriteSlowly("\n=== CONFIGURAZIONE ZEPHYR ===\n");
            await WriteSlowly("Inserire le statistiche del personaggio Zephyr. (INVIO con valore vuoto per mantenere il valore attuale)\n");
            config.zephyr.health = await GetIntInput($"Salute Zephyr (attuale: {config.zephyr.health}): ", config.zephyr.health, 1, 2000);
            config.zephyr.attackPower = await GetIntInput($"Potenza Attacco Zephyr (attuale: {config.zephyr.attackPower}): ", config.zephyr.attackPower, 1, 200);
            config.zephyr.defence = await GetIntInput($"Difesa Zephyr (attuale: {config.zephyr.defence}): ", config.zephyr.defence, 0, 200);

            await WriteSlowly("Configurazione completata!\n");
        }

        /// <summary>
        /// Configura lo stato dei puzzle.
        /// </summary>
        private async Task ConfigurePuzzles()
        {
            await WriteSlowly("\n=== CONFIGURAZIONE PUZZLE ===\n");
            await WriteSlowly("Per ogni puzzle, scegli se deve essere già risolto (s/n)\n");
            await WriteSlowly("NOTA: Alcuni puzzle influenzano l'accessibilità delle stanze!\n\n");

            await WriteSlowly("--- PUZZLE PRINCIPALI ---\n");
            
            // Lock Puzzle
            await WriteSlowly("Lock Puzzle - Puzzle della serratura dell'Ingresso\n");
            await WriteSlowly("- Se risolto (s): Collega Ingresso <-> Salone\n");
            await WriteSlowly("- Se non risolto (n): Dall'Ingresso si può andare solo alla Grotta\n");
            config.puzzle.LockPuzzle = await GetBoolInput($"   Stato attuale: {config.puzzle.LockPuzzle} - Nuovo valore: ", config.puzzle.LockPuzzle);
            
            // Key Puzzle
            await WriteSlowly("\nKey Puzzle - Puzzle della chiave magica\n");
            await WriteSlowly("- Se risolto (s): Permette di raccogliere la chiave magica per accedere al Santuario\n");
            await WriteSlowly("- Se non risolto (n): Il puzzle dovrà essere risolto per ottenere la chiave magica\n");
            config.puzzle.KeyPuzzle = await GetBoolInput($"   Stato attuale: {config.puzzle.KeyPuzzle} - Nuovo valore: ", config.puzzle.KeyPuzzle);

            // Keeper Puzzle
            await WriteSlowly("\nKeeper Puzzle - Puzzle del guardiano della Cripta\n");
            await WriteSlowly("- Se risolto (s): Permette di raccogliere la pergamena magica per attivare il teletrasporto\n");
            await WriteSlowly("- Se non risolto (n): Il puzzle dovrà essere risolto per ottenere la pergamena magica\n");
            config.puzzle.KeeperPuzzle = await GetBoolInput($"Stato attuale: {config.puzzle.KeeperPuzzle}): ", config.puzzle.KeeperPuzzle);

            // Altare Puzzle
            await WriteSlowly("\nAltar Puzzle - Puzzle dell'Altare\n");
            await WriteSlowly("- Se risolto (s): Genera Zephyr nel Salone e permette di parlare con la Dea\n");
            await WriteSlowly("- Se non risolto (n): Zephyr non sarà generato e non potrai parlare con la Dea\n");
            config.puzzle.altarpuzzle = await GetBoolInput($"Stato attuale: {config.puzzle.altarpuzzle} - Nuovo valore: ", config.puzzle.altarpuzzle);

            await WriteSlowly("\n--- CONFIGURAZIONE VARIABILI OGGETTI ---\n");
            
            // Pergamena Magica
            config.puzzle.MagicScrollPickable = await GetBoolInput($"Pergamena Magica raccoglibile (attuale: {config.puzzle.MagicScrollPickable}): ", config.puzzle.MagicScrollPickable);

            // Portale bloccato
            await WriteSlowly("\nPorta bloccata - Blocca l'ingresso allo Studio Alchemico\n");
            await WriteSlowly("- Se bloccato (s): Non puoi accedere allo Studio Alchemico\n");
            await WriteSlowly("- Se non bloccato (n): Il collegamento tra Salone e Studio Alchemico è attivo\n");
            config.puzzle.PortBloc = await GetBoolInput($"Stato attuale: {config.puzzle.PortBloc} - Nuovo valore: ", config.puzzle.PortBloc);
            
            // Teletrasporto
            await WriteSlowly("Teletrasporto attivo\n");
            await WriteSlowly("- Se attivo (s): Permette l'accesso alla Stanza Segreta\n");
            await WriteSlowly("- Se non attivo (n): La Stanza Segreta è INACCESSIBILE\n");
            config.puzzle.TelAtt = await GetBoolInput($"   Stato attuale: {config.puzzle.TelAtt} - Nuovo valore: ", config.puzzle.TelAtt);

            // Cancello
            await WriteSlowly("\nCancello - Blocca l'accesso al Santuario\n");
            await WriteSlowly("- Se chiuso (s): Il Santuario è accessibile solo aprendo il cancello con la chiave magica\n");
            await WriteSlowly("- Se aperto (n): Il Santuario è accessibile\n");
            config.puzzle.CancelloChiuso = await GetBoolInput($"Stato attuale: {config.puzzle.CancelloChiuso} - Nuovo valore: ", config.puzzle.CancelloChiuso);

            // Controllo coerenza configurazione
            await CheckConfigurationConsistency();
            
            await WriteSlowly("Configurazione completata!\n");
        }
        
        /// <summary>
        /// Configura la velocità di digitazione del testo.
        /// </summary>
        private async Task ConfigureTypingSpeed()
        {
            await WriteSlowly("\n=== CONFIGURAZIONE VELOCITÀ TESTO ===\n");
            await WriteSlowly("Configura la velocità di visualizzazione del testo nel gioco.\n");
            await WriteSlowly("Valori più bassi = testo più veloce, valori più alti = testo più lento\n");
            await WriteSlowly("NOTA: Questa velocità sarà usata anche durante il gameplay!\n\n");
            
            await WriteSlowly($"Velocità attuale: {typingSpeed}ms\n");
            await WriteSlowly("Inserisci la nuova velocità di digitazione (0-200ms , premi INVIO con valore vuoto per mantenere attuale): ", 0, false);
            await WriteSlowly("Velocità indicative:\n");
            await WriteSlowly("- 0   = Istantaneo (nessun ritardo)\n");
            await WriteSlowly("- 10  = Molto veloce\n");
            await WriteSlowly("- 30  = Normale (default)\n");
            await WriteSlowly("- 50  = Lento\n");
            await WriteSlowly("- 80  = Molto lento\n");
            await WriteSlowly("- 150 = Lentissimo\n\n");
            
            bool configuring = true;
            while (configuring)
            {
                int newSpeed = await GetIntInput($"Velocità attuale: {typingSpeed}ms - Nuova velocità (0-200): ", typingSpeed, 0, 200);
                
                if (newSpeed != typingSpeed)
                {
                    typingSpeed = newSpeed;
                    await WriteSlowly($"Velocità impostata a {typingSpeed}ms\n");
                    await WriteSlowly("\n=== ANTEPRIMA VELOCITÀ ===\n");
                    
                    if (typingSpeed == 0)
                    {
                        await WriteSlowly("Questo testo appare istantaneamente!", typingSpeed);
                    }
                    else
                    {
                        await WriteSlowly("Questo è un esempio della nuova velocità di scrittura!", typingSpeed);
                    }
                    
                    await WriteSlowly("\nSei soddisfatto della velocità? (s/n): ", 0, false);
                    string? satisfied = ReadLine()?.Trim().ToLower();
                    
                    if (satisfied == "s")
                    {
                        // Imposta la velocità anche per il gioco
                        SetGlobalTypingSpeed(typingSpeed);
                        await WriteSlowly("Velocità impostata anche per il gioco!\n");
                        configuring = false;
                    }
                    else
                    {
                        await WriteSlowly("Vuoi provare un'altra velocità? (s/n): ", 0, false);
                        string? retry = ReadLine()?.Trim().ToLower();
                        if (retry != "s")
                        {
                            configuring = false;
                        }
                        // Se retry == "s", il loop continua
                    }
                }
                else
                {
                    configuring = false;
                }
            }
            
            await WriteSlowly("Configurazione completata!\n");
        }
        
        /// <summary>
        /// Controlla la coerenza della configurazione e avverte l'utente di potenziali problemi.
        /// </summary>
        private async Task CheckConfigurationConsistency()
        {
            await WriteSlowly("\n=== CONTROLLO COERENZA CONFIGURAZIONE ===\n");
            
            bool hasIssues = false;
            
            // Controlla Stanza Segreta senza teletrasporto
            if (config.player.startRoom == "Stanza Segreta" && !config.puzzle.TelAtt)
            {
                await WriteSlowly("   ATTENZIONE: Stanza di partenza 'Stanza Segreta' ma teletrasporto NON attivo!\n");
                await WriteSlowly("   Il giocatore sarà BLOCCATO!\n");
                hasIssues = true;
            }
            
            // Controlla Santuario senza Key Puzzle
            if (config.player.startRoom == "Santuario" && config.puzzle.CancelloChiuso)
            {
                await WriteSlowly("   ATTENZIONE: Stanza di partenza 'Santuario' ma Cancello NON aperto!\n");
                await WriteSlowly("   Potrebbero esserci problemi di navigazione.\n");
                hasIssues = true;
            }
            
            // Controlla Salone senza Lock Puzzle
            if (config.player.startRoom == "Salone" && !config.puzzle.LockPuzzle)
            {
                await WriteSlowly("   INFO: Stanza di partenza 'Salone' ma Lock Puzzle NON risolto.\n");
                await WriteSlowly("   Il giocatore NON potrà tornare all'Ingresso o alla Grotta!\n");
            }
            
            if (!hasIssues)
            {
                await WriteSlowly("Configurazione coerente - Nessun problema rilevato!\n");
            }
        }

        /// <summary>
        /// Mostra la configurazione attuale.
        /// </summary>
        private async Task ShowCurrentConfiguration()
        {
            await WriteSlowly("\n=== CONFIGURAZIONE ATTUALE ===\n");
            
            await WriteSlowly("\n--- GIOCATORE ---\n");
            await WriteSlowly($"Nome: {config.player.name}\n");
            await WriteSlowly($"Salute: {config.player.health}\n");
            await WriteSlowly($"Attacco: {config.player.attackPower}\n");
            await WriteSlowly($"Difesa: {config.player.defence}\n");
            await WriteSlowly($"Peso Max: {config.player.maxCarryWeight}\n");
            await WriteSlowly($"Stanza Partenza: {config.player.startRoom}\n");

            await WriteSlowly("\n--- ZEPHYR ---\n");
            await WriteSlowly($"Salute: {config.zephyr.health}\n");
            await WriteSlowly($"Attacco: {config.zephyr.attackPower}\n");
            await WriteSlowly($"Difesa: {config.zephyr.defence}\n");

            await WriteSlowly("\n--- PUZZLE ---\n");
            await WriteSlowly($"Lock Puzzle: {config.puzzle.LockPuzzle}\n");
            await WriteSlowly($"Keeper Puzzle: {config.puzzle.KeeperPuzzle}\n");
            await WriteSlowly($"Key Puzzle: {config.puzzle.KeyPuzzle}\n");
            await WriteSlowly($"Altare Puzzle: {config.puzzle.altarpuzzle}\n");
            await WriteSlowly($"Pergamena Raccoglibile: {config.puzzle.MagicScrollPickable}\n");
            await WriteSlowly($"Portale Bloccato: {config.puzzle.PortBloc}\n");
            await WriteSlowly($"Teletrasporto Attivo: {config.puzzle.TelAtt}\n");
            await WriteSlowly($"Cancello Chiuso: {config.puzzle.CancelloChiuso}\n");

            await WriteSlowly("\n--- IMPOSTAZIONI INTERFACCIA ---\n");
            await WriteSlowly($"Velocità Testo: {typingSpeed}ms\n");
        }

        /// <summary>
        /// Salva la configurazione corrente in un file.
        /// </summary>
        private async Task SaveConfiguration()
        {
            await WriteSlowly("\n=== SALVATAGGIO CONFIGURAZIONE ===\n");
            
            await WriteSlowly("SUGGERIMENTI per il nome del file:\n");
            await WriteSlowly("- Usa nomi descrittivi come: 'facile', 'difficile', 'speedrun'\n");
            await WriteSlowly("- Includi la stanza di partenza: 'salone-facile', 'grotta-normale'\n");
            await WriteSlowly("- Aggiungi dettagli: 'tutto-risolto', 'solo-lockpuzzle', 'hardcore'\n");
            await WriteSlowly("- Esempi: 'config-base', 'test-veloce', 'partita-personalizzata'\n\n");
            
            await WriteSlowly("Inserisci il nome del file di configurazione (senza estensione): ", 0, false);
            string? fileName = ReadLine()?.Trim();
            
            if (string.IsNullOrEmpty(fileName))
            {
                await WriteSlowly("Nome file non valido.\n");
                return;
            }

            string fullPath = $"{fileName}.json";
            
            // Controlla se il file esiste già
            if (File.Exists(fullPath))
            {
                await WriteSlowly($"ATTENZIONE: Il file '{fileName}.json' esiste già!\n");
                await WriteSlowly("Vuoi sovrascriverlo? (s/n): ", 0, false);
                string? confirm = ReadLine()?.Trim().ToLower();
                if (confirm != "s")
                {
                    await WriteSlowly("Salvataggio annullato.\n");
                    return;
                }
            }
            
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                string json = JsonSerializer.Serialize(config, options);
                await File.WriteAllTextAsync(fullPath, json);
                
                await WriteSlowly($"Configurazione salvata in: {Path.GetFullPath(fullPath)}\n");
            }
            catch (Exception ex)
            {
                await WriteSlowly($"Errore durante il salvataggio: {ex.Message}\n");
            }
        }

        /// <summary>
        /// Carica una configurazione da file.
        /// </summary>
        private async Task LoadConfiguration()
        {
            await WriteSlowly("\n=== CARICAMENTO CONFIGURAZIONE ===\n");
            
            // Cerca i file di configurazione disponibili
            string[] jsonFiles = Directory.GetFiles(".", "*.json");
            
            if (jsonFiles.Length == 0)
            {
                await WriteSlowly("Nessun file di configurazione trovato nella directory corrente.\n");
                return;
            }
            
            await WriteSlowly("File di configurazione disponibili:\n");
            for (int i = 0; i < jsonFiles.Length; i++)
            {
                string configName = Path.GetFileNameWithoutExtension(jsonFiles[i]);
                await WriteSlowly($"- {configName}\n");
            }
            
            await WriteSlowly("\nInserisci il nome del file di configurazione (senza estensione): ", 0, false);
            string? fileName = ReadLine()?.Trim();
            
            if (string.IsNullOrEmpty(fileName))
            {
                await WriteSlowly("Nome file non valido.\n");
                return;
            }

            string fullPath = $"{fileName}.json";
            
            if (!File.Exists(fullPath))
            {
                await WriteSlowly($"File {fullPath} non trovato.\n");
                return;
            }

            try
            {
                string json = await File.ReadAllTextAsync(fullPath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var loadedConfig = JsonSerializer.Deserialize<GameConfig>(json, options);
                if (loadedConfig != null && loadedConfig.player != null && loadedConfig.zephyr != null && loadedConfig.puzzle != null)
                {
                    config = loadedConfig;
                    await WriteSlowly("Configurazione caricata con successo!\n");
                }
                else
                {
                    await WriteSlowly("Errore: configurazione non valida nel file.\n");
                }
            }
            catch (Exception ex)
            {
                await WriteSlowly($"Errore durante il caricamento: {ex.Message}\n");
            }
        }

        /// <summary>
        /// Ottiene un input intero dal giocatore con validazione.
        /// </summary>
        /// <param name="prompt">Messaggio da mostrare.</param>
        /// <param name="currentValue">Valore attuale.</param>
        /// <param name="min">Valore minimo accettato.</param>
        /// <param name="max">Valore massimo suggerito (opzionale, 0 = nessun limite).</param>
        /// <returns>Il valore inserito o quello attuale se non valido.</returns>
        private async Task<int> GetIntInput(string prompt, int currentValue, int min, int max = 0)
        {
            await WriteSlowly(prompt, 0, false);
            string? input = ReadLine()?.Trim();
            
            if (string.IsNullOrEmpty(input))
                return currentValue;

            if (int.TryParse(input, out int value))
            {
                if (value < min)
                {
                    await WriteSlowly($"Valore troppo basso. Deve essere almeno {min}. Mantengo il valore attuale.\n");
                    return currentValue;
                }
                
                // Se è specificato un massimo e il valore lo supera, chiedi conferma
                if (max > 0 && value > max)
                {
                    await WriteSlowly($"ATTENZIONE: Hai inserito {value}, che è superiore al valore consigliato di {max}.\n");
                    await WriteSlowly("Sei sicuro di voler usare questo valore? (s/n): ", 0, false);
                    string? confirm = ReadLine()?.Trim().ToLower();
                    if (confirm != "s")
                    {
                        await WriteSlowly("Mantengo il valore attuale.\n");
                        return currentValue;
                    }
                }
                
                return value;
            }

            await WriteSlowly("Valore non valido. Deve essere un numero intero. Mantengo il valore attuale.\n");
            return currentValue;
        }

        /// <summary>
        /// Ottiene un input booleano dal giocatore.
        /// </summary>
        /// <param name="prompt">Messaggio da mostrare.</param>
        /// <param name="currentValue">Valore attuale.</param>
        /// <returns>Il valore inserito o quello attuale se non valido.</returns>
        private async Task<bool> GetBoolInput(string prompt, bool currentValue)
        {
            await WriteSlowly(prompt, 0, false);
            await WriteSlowly($"(s/n, INVIO per mantenere {(currentValue ? "attivato" : "disattivato")}): ", 0, false);
            string? input = ReadLine()?.Trim().ToLower();
            
            return input switch
            {
                "s" => true,
                "n" => false,
                _ => currentValue
            };
        }

        /// <summary>
        /// Scrive testo lentamente per simulare un effetto di digitazione usando la velocità configurata.
        /// </summary>
        /// <param name="text">Il testo da scrivere.</param>
        /// <param name="delay">Ritardo tra i caratteri in millisecondi (default: usa typingSpeed).</param>
        /// <param name="newLine">Se aggiungere una nuova riga alla fine (default: true).</param>
        private async Task WriteSlowly(string text, int delay = -1, bool newLine = true)
        {
            // Se delay non è specificato, usa la velocità configurata
            int actualDelay = delay == -1 ? typingSpeed : delay;
            
            foreach (char c in text)
            {
                Write(c);
                if (actualDelay > 0)
                    await Task.Delay(actualDelay);
            }
            if (newLine && !text.EndsWith('\n'))
                WriteLine();
        }

        /// <summary>
        /// Imposta la velocità di digitazione globale per il gioco.
        /// </summary>
        /// <param name="speed">Velocità in millisecondi.</param>
        public void SetGlobalTypingSpeed(int speed)
        {
            GameLoop.GlobalTypingSpeed = speed;
        }

    }
}
