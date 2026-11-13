namespace ZephyrLabyrinth
{
    class Dialogue
    {
        /// <summary> Flag interno per gestire una scelta logica del giocatore.</summary>
        bool scelta;

        /// <summary> Indica se il sistema di dialogo è attivo o in corso.</summary>
        bool dialogue = true;

        /// <summary> Indica se il dialogo introduttivo con Zephyr è già stato mostrato.</summary>
        public bool introZephyr = false;

        /// <summary> Indica se la battaglia con Zephyr è terminata.</summary>
        public bool EndBattle = false;

        /// <summary> Segna se Condri è già stato generato nel mondo di gioco.</summary>
        public bool condriSpawned = false;

        //-----------------------------------------------------------------------------
        //                                 Menu Iniziale
        //-----------------------------------------------------------------------------

        /// <summary> Mostra il menu principale del gioco "Il Labirinto di Zephyr" con le opzioni disponibili.</summary>
        /// <returns> Task asincrono che mostra il menu all'utente.</returns>
        public async Task MenuOption()
        {
            await WriteSlowly("=== IL LABIRINTO DI ZEPHYR ===\n");
            await Task.Delay(1000);
            await WriteSlowly("1. Nuova partita\n");
            await WriteSlowly("2. Carica partita\n");
            await WriteSlowly("3. Esci dal gioco\n");
            await WriteSlowly("4. Configurazione interattiva\n");
        }
        
        //-----------------------------------------------------------------------------
        //                                 Dialoghi Storia
        //-----------------------------------------------------------------------------

        /// <summary> Metodo asincrono che introduce il giocatore al mondo di gioco "Labirinto di Zephyr".</summary>
        /// <remarks>
        /// Simula una narrazione iniziale tramite output testuale progressivo (WriteSlowly),
        /// costruendo il contesto della storia, dei personaggi e dell'incidente iniziale.
        /// Alla fine, introduce la co-protagonista Elara e richiede il nome del giocatore.
        /// </remarks>
        /// <returns> Un Task rappresentante l'operazione asincrona di introduzione narrativa.</returns>
        public async Task Introduzione()
        {
            await WriteSlowly("\n------------------------------------------------ Benvenuto nel Labirinto di Zephyr ------------------------------------------------");
            await Task.Delay(1500);
            await WriteSlowly("\nSei un avventuriero in viaggio su una carrozza diretta verso Winterhold, la capitale delle terre del nord.");
            await WriteSlowly("Oltre a te, sul carro si trovano il guidatore e una ragazza misteriosa che afferma di essere una maga.");
            await WriteSlowly("Siete stati incaricati dal vostro Re di trovare il leggendario labirinto di Zephyr e recuperare i tesori che sono custoditi al suo interno.");
            await WriteSlowly("Il vostro paese e' in rovina e forse riuscirete a salvarlo se porterete a termine la vostra missione.");
            await WriteSlowly("Il guidatore è un vecchio mercante a cui sono stati dati parecchi soldi per accompagnarvi.");
            await WriteSlowly("La ragazza è molto taciturna e tiene sempre coperto il suo volto, come te, ha chiesto un passaggio all'anziano.");
            await WriteSlowly("Mentre viaggiate nella notte piu' buia, attraversando la foresta che precede la capitale, sentite degli strani rumori.");
            await WriteSlowly("Il cavallo che traina la carrozza inizia a sbizzarrirsi e il terreno sotto di voi inizia a crollare.");
            await WriteSlowly("Tempestivamente spingi con forza l'anziano sul terreno che non ha ceduto.");
            await WriteSlowly("La ragazza nel mentre mette in salvo il cavallo con un incantesimo.");
            await WriteSlowly("Entrambi vi ritrovate a cadere nella voragine formatasi dal crollo.");
            await WriteSlowly("Perdete conoscenza per colpa della caduta.");
            await WriteSlowly("Dopo un tempo non precisato riprendi i sensi...");
            await WriteSlowly("\n-----------------------------------------------------------------------------------------------------------------------------------");
            await Task.Delay(1500);
            await WriteSlowly("\nSYSTEM: Sei nella Grotta.");
            await grotta.DisplayRoomDescription();
            await Task.Delay(1500);
            await WriteSlowly("\nNARRATORE: Osservi la zona e vedi la ragazza che esplora l'area.");
            await Task.Delay(1500);
            await WriteSlowly("\n???: Per quanto cerco, non sembra esserci nessun' altra via di fuga.");
            await Task.Delay(1500);
            await WriteSlowly("\nNARRATORE: Mentre la ragazza cerca un modo per uscire dalla grotta si gira verso la tua direzione e nota che hai ripreso i sensi.");
            await Task.Delay(1500);
            await WriteSlowly("\n???: Tutto bene? Riesci a camminare?");
            await Task.Delay(1500);
            await WriteSlowly("\nNARRATORE: La ragazza si avvicina e ti aiuta ad alzarti.");
            await Task.Delay(1500);
            await WriteSlowly("\n???: Sembri tutto intero... menomale! Permettimi di presentarmi, io sono Elara... Tu come ti chiami?");
            await Task.Delay(1000);
            await WriteSlowly("\nSYSTEM: Inserisci il tuo nome: ", 40, false);
            Logger.Log("Esposta introduzione di gioco e di Elara al giocatore (1)", "INFO");
        }

        /// <summary> Seconda parte dell'introduzione narrativa del gioco "Labirinto di Zephyr".</summary>
        /// <returns> Un Task che rappresenta l'operazione asincrona di narrazione e introduzione al gameplay.</returns>
        public async Task Introduzione2()
        {
            await Task.Delay(1500);
            await WriteSlowly($"\n{player.Name}: Io sono {player.Name}.");
            await WriteSlowly($"{player.Name}: Prima stavi controllando la zona, hai trovato una via di uscita?");
            await WriteSlowly("Elara: Solo una.");
            await Task.Delay(1500);
            await WriteSlowly("\nNARRATORE: Elara procede ad indicarti un portone nascosto dalle macerie.");
            await Task.Delay(1500);
            await WriteSlowly($"\n{player.Name}: Non sembra una opzione così brutta... ");
            await WriteSlowly("Elara: Credo sia l'entrata del labirinto... Sembra che finalmente lo abbiamo trovato.");
            await WriteSlowly($"{player.Name}: Sei sicura che sia proprio quel labirinto?");
            await WriteSlowly("Elara: Si, il logo nell'arco del portone ne da conferma.");
            await WriteSlowly($"{player.Name}: Avrei voluto prepararmi meglio prima di entrare, ho perso tutto il mio equipaggiamento durante la caduta ma sembra che non abbiamo altre alternative...");
            await WriteSlowly("Elara: Dobbiamo fare molta attenzione, nessuno sa quali pericoli si celino al suo interno, ora dirigiamoci verso nord.");
            await Task.Delay(1500);
            await WriteSlowly("\nSUGGERIMENTO: Usa il comando 'move [Direzione]' per spostarti.");
            await WriteSlowly("              Usa il comando 'interact [Oggetto]' per interagirci.");
            await WriteSlowly("              Usa il comando 'observe room' per osservare la stanza.");
            await WriteSlowly("              Usa il comando 'observe items' per osservare gli oggetti presenti nella stanza.");
            await WriteSlowly("              Usa il comando 'take [Oggetto]' per raccoglierlo.");
            await WriteSlowly("              Usa il comando 'drop [Oggetto]' per lasciarlo.");
            await WriteSlowly("\nSUGGERIMENTO: Le direzioni sono: nord, sud, est, ovest.");
            await WriteSlowly("              Alcuni oggetti che trovi nelle stanze possono aumentrare le tue statistiche, come la salute, la potenza d'attacco e la difesa.");
            await Task.Delay(1500);
            await WriteSlowly($"\nSYSTEM: Le tue statistiche iniziali sono: {player.AttackPower} ATK | {player.Defence} DEF | {player.Health} HP");
            await Task.Delay(1500);
            await WriteSlowly("\nSUGGERIMENTO: I nomi menzionati nei dialoghi che sono tra 'esempio' sono oggetti o persone con cui puoi interagire (interact), parlare (talk) o essere raccolti (take [nome oggetto]).");
            await Task.Delay(1500);
            await WriteSlowly("\nSYSTEM: Se i tuoi HP scendono a 0 hai perso.");
            Logger.Log("Esposta introduzione (2)", "INFO");
        }

        /// <summary> Terza parte della narrativa del gioco "Labirinto di Zephyr".</summary>
        /// <remarks>
        /// <para> Il giocatore e Elara attraversano una nuova stanza del labirinto e incontrano Condri.</para>
        /// <para> Questo evento introduce un nuovo nemico, una situazione di pericolo e un obiettivo narrativo: trovare un modo per riaprire la porta bloccata magicamente.</para>
        /// <para> Il metodo mostra un dialogo interattivo e fornisce un suggerimento per proseguire nel gioco.</para>
        /// <para> Questa sezione è eseguita solo una volta, grazie al flag `condriSpawned`.</para>
        /// </remarks>
        /// <returns> Un Task che rappresenta l’operazione asincrona di narrazione e introduzione dell’incontro con Condri.</returns>
        public async Task IntroduzioneCondri()
        {
            if (!condriSpawned)
            {
                await Task.Delay(1500);
                await WriteSlowly("\nNARRATORE: Sbloccata la porta la attraversi insieme a Elara.");
                await Task.Delay(1500);
                await player.CurrentRoom.DisplayRoomDescription();
                await Task.Delay(1500);
                await WriteSlowly($"\n{player.Name}: Finalmente abbiamo superato il portone...");
                await WriteSlowly("Elara: Questo luogo sembra che funga da collegamento per arrivare alle altre stanze del labirinto.");
                await Task.Delay(1500);
                await WriteSlowly("\nNARRATORE: Mentre parli con la ragazza noti una piccola figura volante vicino la porta alla vostra destra.");
                await Task.Delay(1500);
                await WriteSlowly("\n???: Speriamo che il mio piccolo esperimento non abbia fatto arrabbiare il padrone...");
                await Task.Delay(1500);
                await WriteSlowly("\nNARRATORE: Cerchi di avvicinarti di soppiatto per capire meglio chi sia quell'essere, nel farlo attiri inavvertitamente la sua attenzione.");
                await Task.Delay(1500);
                await WriteSlowly("\n???: Uh? E voi chi siete? Come siete entrati?");
                await WriteSlowly("Elara: Una frana ci ha fatto crollare in una grotta di fronte all'entrata del labirinto e stiamo cer...");
                await WriteSlowly("???: NO! NO! NO! nessuno può entrare a meno che non lo voglia il Grande Zep...");
                await Task.Delay(1500);
                await WriteSlowly("\nNARRATORE: Una potente voce riecheggia nella stanza, è cosi imponente che il solo udirla vi blocca, come se il vostro corpo fosse diventato di pietra.");
                await Task.Delay(1500);
                await WriteSlowly("\n???: Mi hai deluso Condri, hai lasciato che qualcuno entrasse...");
                await WriteSlowly("Condri: Non è come sembra padro...");
                await WriteSlowly("???: SILENZIO! Sbarazzati degli intrusi e non deludermi, altrimenti...");
                await WriteSlowly("Condri: Ce...rto pa...dr...drone.");
                await Task.Delay(1500);
                await WriteSlowly("\nNARRATORE: Il silenzio cala nella stanza e Condri si gira verso di voi, infuriato.");
                await Task.Delay(1500);
                await WriteSlowly("\nCondri: PER COLPA VOSTRA SONO STATO RIMPROVERATO DAL PADRONE! Pagherete con la vostra vita per questo!");
                await Task.Delay(1500);
                await WriteSlowly("\nNARRATORE: Condri inizia a pronunciare una formula magica, sorpresi dalla situazione cercate di reagire.");
                await WriteSlowly("           Elara prova a contrastarlo con una delle sue magie.");
                await WriteSlowly("           E' tutto inutile, con la sua agilità l'essere evita il colpo.");
                await WriteSlowly("           Condri completa il suo incantesimo sigillando la porta a est della stanza.");
                await WriteSlowly("           Al completamento Condri svanisce ed inizia ad urlare dall'altro lato della porta.");
                await Task.Delay(1500);
                await WriteSlowly("\nCondri: Con questa stanza bloccata non potrete scappare da qui, morirete di fame!");
                await WriteSlowly("        Userò i vostri corpi per delle pozioni magiche da dare in dono al padrone per farmi perdonare... Muhahahahha.");
                await WriteSlowly("Elara: Maledizione...");
                await WriteSlowly($"{player.Name}: Non puoi spezzare l'incantesimo che blocca la porta?");
                await WriteSlowly("Elara: No... e' troppo potente, può essere interrotto solo ferendo chi ha pronunciato la formula.");
                await WriteSlowly($"{player.Name}: Dobbiamo trovare un altro modo per entrare in quella stanza.");
                await Task.Delay(1500);
                await WriteSlowly("\nSUGGERIMENTO: Usa il comando 'talk Elara' per parlare con la giovane maga.");
                Logger.Log("Esposta introduzione del Salone e di Condri al giocatore (3)", "INFO");
                condriSpawned = true;
            }
        }

        /// <summary> Gestisce l'interazione narrativa e la conversazione con il personaggio Condri nello studio alchemico.</summary>
        /// <remarks>
        /// <para> Se la porta dello studio è ancora chiusa, il giocatore insegue Condri, lo stordisce accidentalmente con un'ampolla,
        /// sblocca l'accesso allo studio e assiste alla reazione di Condri una volta catturato.</para>
        /// <para> Se la porta è già stata sbloccata, descrive Condri ancora intrappolato e la decisione del giocatore di allontanarsi.</para>
        /// </remarks>
        /// <returns> Un Task che rappresenta l’operazione asincrona di esecuzione del dialogo e della narrazione.</returns>
        public async Task CondriNelloStudio()
        {
            Logger.Log($"{player.Name} ha avviato una conversazione con Condri", "INFO");
            if (keypuzzle.portboc)
            {
                await Task.Delay(1500);
                await WriteSlowly($"\nNARRATORE: Condri si gira e si ritrova con {player.Name} alle sue spalle.");
                await WriteSlowly("\nCondri: Ahhhhhhhhhh!!!");
                await Task.Delay(1500);
                await WriteSlowly("\nNARRATORE: Condri cade nel panico e inizia a volare alla rinfusa nella stanza.");
                await Task.Delay(1500);
                await WriteSlowly("\nCondri: COME SEI ARRIVATO?!?!?");
                await WriteSlowly("        STAMMI LONTANO!!!");
                await Task.Delay(1500);
                await WriteSlowly("\nNARRATORE: Insegui Condri per tutta la stanza, nella speranza di colpirlo.");
                await WriteSlowly("           Nella fuga Condri sbatte contro una ampolla posta su un ripiano.");
                await WriteSlowly("           L'impatto è cosi forte da far perdere i sensi a Condri, che cade sulla superficie sottostante.");
                await WriteSlowly("           Per pura casualità l'ampolla su cui si era schiantato Condri perde l'equilibrio.");
                await WriteSlowly("           Nel cadere si rigira su se stessa atterrando in modo perfetto sopra Condri, bloccandolo.");
                await WriteSlowly($"\n{player.Name}: ...");
                await Task.Delay(1500);
                await WriteSlowly("\nNARRATORE: Con Condri svenuto vedi il suo l'incantesimo dissolversi.");
                await Task.Delay(1500);
                await WriteSlowly("\nSYSTEM: Hai sbloccato l'accesso allo Studio Alchemico!");
                await Task.Delay(1500);
                await WriteSlowly("\nNARRATORE: Apri la porta e avvisi Elara che sei riuscito nell'impresa.");
                await WriteSlowly("           Elara esulta dell'impresa e procede ad entrare nella stanza.");
                await WriteSlowly("           Dopo un po' che tu e Elara esplorate la stanza Condri riprende i sensi.");
                await Task.Delay(1500);
                await WriteSlowly("\nCondri: Dove sono? Ah, ora ricordo!");
                await Task.Delay(1500);
                await WriteSlowly("\nNARRATORE: Condri nota la situazione e cerca di usare la magia per liberarsi, senza riuscirci.");
                await Task.Delay(1500);
                await WriteSlowly("\nElara: È inutile, ho incantato l'ampolla in modo che tu non possa uscirci.");
                await WriteSlowly("Condri: Liberatemi!!!");
                await WriteSlowly("Elara: Perché mai dovremmo farlo?");
                await WriteSlowly("Condri: Se lo farete potrei 'risparmiarvi'.");
                await WriteSlowly($"{player.Name}: Non credo lo faremo.");
                await Task.Delay(1500);
                await WriteSlowly("\nNARRATORE: Condri continua ad insultarvi e minacciarvi, Elara utilizza un altra magia in modo che non lo possiate più sentire.");
                await Task.Delay(1500);
                await WriteSlowly("\nElara: Bene, lo abbiamo sistemato. Ora torno nel salone a cercare oggetti che potrebbero tornarci utili.");
                await WriteSlowly($"{player.Name}: Ricevuto, io continuo ad esplorare.");
                await Task.Delay(1500);
                await WriteSlowly("\nNARRATORE: Elara annuisce e ritorna nel Salone.");
            }
            else
            {
                await Task.Delay(1500);
                await WriteSlowly("\nNARRATORE: Condri ancora silenziato dalla magia di Elara continua a dimenarsi dentro l'ampolla.");
                await WriteSlowly("           Lo stuzzichi un po' per poi allontanarti.");
            }
        }

        //-----------------------------------------------------------------------------
        //                            Dialoghi Personaggi
        //-----------------------------------------------------------------------------

        /// <summary> Gestisce la conversazione interattiva tra il giocatore e Elara.</summary>
        /// <remarks>
        /// <para> Presenta un menu di dialogo con più opzioni, permettendo al giocatore di scegliere di approfondire diversi argomenti o sbloccare funzioni del gioco.</para>
        /// <para> Include controlli dinamici basati sullo stato del gioco (es. sblocco della porta, possesso di oggetti, stato del teletrasporto).</para>
        /// <para> La conversazione continua finché il giocatore non sceglie di allontanarsi da Elara.</para>
        /// </remarks>
        /// <returns> Un Task che rappresenta l’operazione asincrona di gestione del dialogo con Elara.</returns>
        public async Task ElaraDialogue()
        {
            Logger.Log($"{player.Name} ha avviato una conversazione con Elara", "INFO");
            dialogue = true;
            while (dialogue == true)
            {
                await WriteSlowly($"\nElara: {player.Name}, cosa vuoi dirmi?");
                await WriteSlowly("\n1. Raccontami di te.");
                await WriteSlowly("2. Puoi Raccontarmi qualcosa di più su dove siamo?");
                if (keypuzzle.portboc)
                {
                    await WriteSlowly("3. Cosa facciamo per la porta?");
                }
                else
                {
                    await WriteSlowly("3. [Hai gia' sbloccato la porta dello studio di Condri]");
                }
                bool hasPergamena = player.Inventory.Count > 0 && player.Inventory.Peek().Name.Equals("Pergamena Magica", StringComparison.OrdinalIgnoreCase);
                if (hasPergamena)
                {
                    await WriteSlowly("4. Dai 'Pergamena Magica'");
                }
                else if (teleport.telAtt == false && !hasPergamena)
                {
                    await WriteSlowly("4. [Manca qualcosa per sbloccare questa opzione]");
                }
                else
                {
                    await WriteSlowly("4. Come funziona il teletrasporto?");
                }
                await WriteSlowly("5. Quanto e' grande il Labirinto?");
                await WriteSlowly("\n6. Allontanati da Elara");
                await WriteSlowly("\nSYSTEM: Scegli un'opzione: ", 40, false);

                string? playerInput = ReadLine();
                if (!int.TryParse(playerInput, out int inputNumber) || inputNumber > 6 || inputNumber <= 0)
                {
                    await Task.Delay(1500);
                    await WriteSlowly("\nSYSTEM: Opzione non valida. Inserisci un numero da 1 a 6.\n", 40, false);
                }
                else
                {
                    switch (playerInput)
                    {
                        case "1":
                            // Opzione: Elara si presenta
                            await WriteSlowly("\nElara: Sono una giovane maga, ho studiato per secoli i segreti dell'arcano e ho viaggiato per il mondo per imparare nuove magie.");
                            await WriteSlowly("       Tutto per mantenere alto l'onore della mia famiglia, casato nobile del Regno degli Elfi.");
                            await WriteSlowly("       Sono qui per aiutarti a superare le sfide del labirinto e trovare i tesori che sono custoditi al suo interno.");
                            Logger.Log("Il giocatore ha scelto l'opzione 1", "INFO");
                            break;

                        case "2":
                            // Opzione: Informazioni sul labirinto
                            await WriteSlowly("\nElara: Siamo nel labirinto di Zephyr, un luogo pieno di pericoli e trappole.");
                            await WriteSlowly("       Il mago che lo ha creato, Zephyr, è un potente stregone che ha partecipato alla guerra contro i demoni ed è in grado di viaggiare tra diverse dimensioni.");
                            await WriteSlowly("       Dopo la guerra, Zephyr, ormai corrotto dal potere, ha deciso di creare questo labirinto per isolarsi dal mondo con lo scopo di sviluppare le sue abilità magiche.");
                            await WriteSlowly("       Nessuno sa cosa voglia ottenere, ma si pensa che voglia conquistare il mondo con la sua magia attraverso gli oggetti rubati nei viaggi ultradimensionali.");
                            Logger.Log($"Il giocatore ha scelto l'opzione 2", "INFO");
                            break;

                        case "3":
                            // Opzione: Discussione sulla porta di Condri
                            Logger.Log($"Il giocatore ha scelto l'opzione 3", "INFO");
                            if (keypuzzle.portboc == true)
                            {
                                await WriteSlowly("\nElara: Dobbiamo trovare un modo per aprire la porta...");
                                await WriteSlowly("       Forse cercando in altre stanze potremmo trovare qualcosa che ci aiuti.");
                                await WriteSlowly("       Inoltre dobbiamo trovare il modo di colpire Condri così da poter interrompere l'incantesimo che blocca la porta.\n");
                            }
                            else
                            {
                                await WriteSlowly("\nSYSTEM: La porta è stata già aperta.\n");
                            }
                            break;

                        case "4":
                            // Opzione: Teletrasporto
                            Logger.Log("Il giocatore ha scelto l'opzione 4", "INFO");
                            if (keeperpuzzle.cont == true && hasPergamena)
                            {
                                await WriteSlowly("\nElara: Non ho mai visto una pergamena così, e' in grado di conferire l'abilità del teletrasporto all'utilizzatore.");
                                await WriteSlowly("       È scritta correttamente ma sembra un po' instabile... Penso di poterla stabilizzare, vuoi che lo faccia?");
                                scelta = false;
                                while (scelta == false)
                                {
                                    await WriteSlowly("\n1. Si, stabilizzala!");
                                    await WriteSlowly("2. Ci, devo pensare");
                                    await WriteSlowly("\nSYSTEM: Scegli un'opzione: ", 40, false);
                                    string? playerInput2 = ReadLine();
                                    if (!int.TryParse(playerInput2, out int inputNumber2) || inputNumber2 > 2 || inputNumber2 <= 0)
                                    {
                                        await WriteSlowly("\nSYSTEM: Opzione non valida. Inserisci 1 o 2.", 40, false);
                                    }
                                    if (playerInput2 == "1")
                                    {
                                        await WriteSlowly("\nElara: Va bene, ci provo");
                                        await Task.Delay(1500);
                                        await WriteSlowly("\nNARRATORE: Elara incanala la sua magia nella pergamena, attivandola.");
                                        await Task.Delay(1500);
                                        await teleport.TeleportActivation();
                                        await WriteSlowly("\nSYSTEM: Complimenti, hai sbloccato il teletrasporto!");
                                        Logger.Log("Il giocatore ha attivato adesso il teletrasporto.", "INFO");
                                        await Task.Delay(1500);
                                        await WriteSlowly("\nSUGGERIMENTO: Digita 'teleport' per andare in una stanza casuale.");
                                        await Task.Delay(1500);
                                        await WriteSlowly("\nElara: Con questa potremmo raggiungere Condri senza dover aprire la porta!\n");
                                        scelta = true;
                                    }
                                    else if (playerInput2 == "2")
                                    {
                                        await WriteSlowly("\nElara: Capito, quando te la senti torna pure da me.\n");
                                        Logger.Log("Il giocatore ha scelto di non attivare adesso il teletrasporto.", "INFO");
                                        scelta = true;
                                    }
                                }
                            }
                            else if (keeperpuzzle.cont == false && !hasPergamena)
                            {
                                await WriteSlowly("\nSYSTEM: Opzione non utilizzabile.");
                            }
                            else
                            {
                                await WriteSlowly("\nElara: Il teletrasporto ti permette di andare immediatamente in una stanza casuale tra quelle che gia' conosci.");
                                await WriteSlowly("       Devi pero' fare molta attenzione, questo oscuro potere ha un costo e questo ti togliera' 5 HP ad ogni utilizzo.");
                                await WriteSlowly("       Tuttavia sono riuscita a fare una piccola modifica e la prima volta che lo utilizzerai non subirai alcun danno collaterale.");
                                await WriteSlowly("       Inoltre potrai raggiungere la stanza dove si nasconde Condri al primo tentativo.");
                            }
                            break;

                        case "5":
                            // Opzione: Informazioni sulla grandezza del labirinto
                            await WriteSlowly("\nElara: Il labirinto secondo la leggenda dovrebbe essere composto da cinque stanze, quattro per ogni punto cardinale e con un grande salone al centro.");
                            await WriteSlowly("       Inoltre qualcuno sostiene che ci sia una stanza nascosta e non raggiungibile senza un particolare potere.");
                            await WriteSlowly("       In quella stanza Zephyr ha occultato i tesori e gli oggetti che ha rubato nel corso dei secoli dalle altre dimensioni.");
                            await WriteSlowly("       Tuttavia nessuno ha la certezza della sua esistenza, ma se dovessi trovare il modo per raggiungerla ti aspettano grandi ricompense.");
                            Logger.Log("Il giocatore ha scelto l'opzione 5", "INFO");
                            break;

                        case "6":
                            // Opzione: Terminare il dialogo
                            Logger.Log("Il giocatore ha scelto l'opzione 6", "INFO");
                            await WriteSlowly("\nNARRATORE: Ti allontani da Elara.");
                            dialogue = false;
                            break;
                    }
                }
            }
        }

        /// <summary> Mostra a schermo il dialogo di Elara riguardo al portone chiuso e la necessità di trovare un indizio per aprirlo.</summary>
        /// <returns> Task che rappresenta l'operazione asincrona di scrittura a schermo.</returns>
        public async Task IngressoDoorLocked()
        {
            await Task.Delay(1500);
            await WriteSlowly("\nElara: Questo portone e' chiuso da una strana serratura, dobbiamo trovare il modo per aprirla... Deve pur esserci un indizio da qualche parte...");
        }

        /// <summary> Mostra un dialogo in cui Elara segnala la necessità di un oggetto magico per aprire una porta chiusa.</summary>
        /// <returns> Un Task asincrono che rappresenta il completamento del dialogo.</returns>
        public async Task PlayerNeedSomethingToOpenDoor()
        {
            await Task.Delay(500);
            await WriteSlowly("\nElara: Hmm... La porta è chiusa e non ho idea di come fare per aprirla.");
            await WriteSlowly("       Forse un oggetto magico potrebbe aiutarci a proseguire.");
            await WriteSlowly($"       {player.Name} hai trovato qualcosa di utile nella Cripta?");
        }

        /// <summary> Elara esprime interesse per la pergamena magica, suggerendo al giocatore di portargliela per sfruttarne il potere.</summary>
        /// <returns> Task asincrono che mostra il dialogo di Elara.</returns>
        public async Task ElaraWantsMagicScroll()
        {
            await Task.Delay(500);
            await WriteSlowly("\nElara: Quella deve essere una pergamena magica... Percepisco un grande potere, portamela e forse potremo utilizzarla a nostro vantaggio.");
        }
        

        /// <summary> Gestisce il dialogo con la Dea, che racconta la storia della Master Sword. </summary>
        /// <returns> Un Task che rappresenta l’operazione asincrona di visualizzazione del testo narrativo.</returns>
        public async Task DeaTalk()
        {
            await WriteSlowly("\nDea: Sono secoli che custodivo la 'Master Sword' per evitare che cadesse nelle mani sbagliate.");
            await WriteSlowly("     Molti anni anni fa un potente mago ha celato questo Santuario dentro al suo labirinto.");
            await WriteSlowly("     Da subito mi era chiaro che volesse il potere della spada e er tutto questo tempo ho impedito che potesse ottenere ciò che desiderava.");
            await WriteSlowly("     Ma si e' rivelato un nemico troppo potente e sarebbe stata solo questione di tempo prima che fosse riuscito a sottrarmela.");
            await WriteSlowly("     Mi stavo per rassegnare ma per fortuna sei arrivato tu.");
            await WriteSlowly("     Ricorda, solo chi possiede le tre virtù potrà impugnarla senza subirne le conseguenze.");
        }

        /// <summary> Gestisce l'introduzione narrativa dell'incontro con Zephyr.</summary>
        /// <remarks>
        /// <para> Se l'introduzione non è ancora stata mostrata, presenta il dialogo che introduce Zephyr nel Salone.</para>
        /// <para> Una volta mostrata, segnala che Zephyr attende la risposta del giocatore senza ripetere l'introduzione.</para>
        /// </remarks>
        /// <returns> Un Task che rappresenta l’operazione asincrona di visualizzazione del testo narrativo.</returns>
        public async Task ZephyrIntroduction()
        {
            if (introZephyr == false)
            {
                Logger.Log($"{player.Name} ha avviato l'intro di Zephyr", "INFO");
                await Task.Delay(1500);
                await WriteSlowly("\nNARRATORE: Entri nel Salone e noti una figura imponente al centro della stanza.");
                await WriteSlowly("           Non riconosci il volto dell'essere ma percepsci dalla sua presenza chi sia.");
                await WriteSlowly("           Il creatore del labirinto 'Zephyr' si è palesato.");
                await WriteSlowly("           Noti anche Elara ferita a terra, di fronte a Zephyr.");
                await Task.Delay(1500);
                await WriteSlowly($"\nZephyr: Sono sorpreso {player.Name}, hai superato tutti gli ostacoli che ho lasciato.");
                await WriteSlowly("        Ma poco importa, il vostro destino è già segnato. Io sono Zephyr, il più grande mago della storia.");
                await WriteSlowly("        NON ESISTE NESSUN MODO PER USCIRNE VIVI, E NON POTETE SCONFIGGERMI.");
                await WriteSlowly("        Gli anni che ho passato a studiare la magia mi hanno reso invincibile.");
                await WriteSlowly("        E non mi fermerò finché non avrò conquistato il mondo con la mia magia.");
                await Task.Delay(1500);
                await WriteSlowly("\nNARRATORE: Sia tu che Elara siete terrorizati al cospetto di Zephyr, ormai più simile ad un mostro che ad un umano.");
                await Task.Delay(1500);
                await WriteSlowly("\nZephyr: Avevo già intenzione di uscire da qui per attuare la mia ascesa a sovrano del mondo.");
                await WriteSlowly("        Ma dato l'intrattenimento che mi avete offerto, ho deciso di farvi morire in modo lento e doloroso.");
                await WriteSlowly("        DOVRAI COMBATTERE CONTRO DI ME!");
                await WriteSlowly("        Preparati pure e sfidami quando pensi di essere pronto...");
                await WriteSlowly("        ... MA SAPPI CHE SARA' TUTTO INUTILE!");
                await Task.Delay(1500);
                await WriteSlowly("\nNARRATORE: Zephyr sogghigna di fronte ai vostri sguardi impauriti, aspettando una tua risposta.");
                await WriteSlowly($"\nElara: {player.Name} non puoi permettere che Zephyr conquisti il mondo!");
                introZephyr = true;
            }
            else
            {
                await WriteSlowly("SYSTEM: Zephyr è in attesa di una tua risposta.");
                Logger.Log($"{player.Name} ha già avviato l'intro di Zephyr", "INFO");
                return;
            }

        }

        /// <summary> Gestisce il dialogo con Zephyr prima dell'inizio del combattimento finale.</summary>
        /// <remarks>
        /// <para> Presenta al giocatore due opzioni: iniziare subito il combattimento oppure prendersi più tempo per prepararsi.</para>
        /// <para> Se il giocatore sceglie di combattere, avvia la battaglia con Zephyr.</para>
        /// <para> Se il giocatore non è pronto, Zephyr deride il giocatore e il dialogo termina.</para>
        /// <para> Gestisce anche input non valido richiedendo una scelta corretta.</para>
        /// </remarks>
        /// <returns>Un Task che rappresenta l’operazione asincrona di gestione del dialogo e potenziale avvio del combattimento.</returns>
        public async Task ZephyrFightDialogue()
        {
            Logger.Log($"{player.Name} ha avviato una conversazione con Zephyr", "INFO");
            await WriteSlowly("\nZephyr: Sei pronto a combattere?");
            await WriteSlowly("\n1. Si, sono pronto!");
            await WriteSlowly("2. No, devo prepararmi.");
            await WriteSlowly("\nSYSTEM: Scegli un'opzione: ", 40, false);
            string? playerInput = ReadLine();
            if (playerInput == "1")
            {
                Logger.Log("Il giocatore ha deciso iniziare il combattimento", "INFO");
                await WriteSlowly("\nZephyr: Finalmente, quasi stavo per decidere di uccidervi subito... Vediamo di cosa sei capace!");
                zephyr = (Zephyr)salone.characters.First(c => c is Zephyr);
                Logger.Log($"DEBUG: player.Health={player.Health}, zephyr.Health={zephyr.Health}, zephyr ref={zephyr.GetHashCode()}", "DEBUG");
                if (battle == null)
                {
                    battle = new ZephyrBattle();
                }
                Logger.Log($"DEBUG: Zephyr in stanza ref={salone.characters.First(c => c is Zephyr).GetHashCode()}", "DEBUG");
                await battle.StartBattle();
            }
            else if (playerInput == "2")
            {
                await WriteSlowly("\nZephyr: Bene Verme, prenditi pure il tuo tempo. Ma la paura che provi non passera' con qualche oggetto magico.");
                await Task.Delay(1500);
                await WriteSlowly("\nNARRATORE: Zephyr ride di te, con un sorriso inquietante.");
                await Task.Delay(1500);
                await WriteSlowly("\nSYSTEM: Ti allontani da Zephyr.");
                Logger.Log("Il giocatore ha deciso che non e' pronto a combattere", "INFO");
            }
            else
            {
                await WriteSlowly("\nSYSTEM: Opzione non valida. Inserisci 1 o 2.", 40, false);
                Logger.Log("Il giocatore ha inserito una opzione non valida", "WARN");
            }
        }

        /// <summary> Gestisce la scelta dell'azione di combattimento da parte del giocatore, attendendo un input valido tra 'attacca' e 'parata', e genera casualmente l'azione corrispondente di Zephyr.</summary>
        /// <returns> Una tupla contenente l'azione scelta dal giocatore e quella generata per Zephyr, entrambe di tipo <see cref="ZephyrBattle.CombatAction"/>.</returns>
        public async Task<(ZephyrBattle.CombatAction playerAction, ZephyrBattle.CombatAction zephyrAction)> BattleOption()
        {
            ZephyrBattle.CombatAction playerAction = ZephyrBattle.CombatAction.Default;
            bool correctInput = false;
            while (!correctInput)
            {
                Logger.Log("Dialogo per scelta di azione di combattimento", "INFO");
                await WriteSlowly("\nSYSTEM: Digita 'attacca' per attaccare Zephyr, 'parata' per proteggerti e subire meno danni dal suo attacco.");
                await WriteSlowly("\nSYSTEM: Scegli un'opzione: ", 40, false);
                string? action = ReadLine()?.Trim().ToLower();
                if (action == "parata")
                {
                    playerAction = ZephyrBattle.CombatAction.Parata;
                    Logger.Log($"Il giocatore ha scelto: {playerAction}.", "INFO");
                    correctInput = true;
                }
                else if (action == "attacca")
                {
                    playerAction = ZephyrBattle.CombatAction.Attacca;
                    Logger.Log($"Il giocatore ha scelto: {playerAction}.", "INFO");
                    correctInput = true;
                }
                else
                {
                    await player.PlayerInputWarn();
                    continue;
                }
            }
            ZephyrBattle.CombatAction zephyrAction = ZephyrBattle.ZephyrRandomAction();
            Logger.Log($"Zephyr ha scelto: {zephyrAction}.", "INFO");
            return (playerAction, zephyrAction);
        }

        /// <summary> Gestisce la sequenza narrativa e il dialogo in caso di vittoria del giocatore nel combattimento finale contro Zephyr.</summary>
        /// <remarks>
        /// <para> Mostra il discorso di Zephyr sconfitto, la trasformazione del salone, il salvataggio da parte delle guardie reali e il finale del gioco.</para>
        /// <para> Attende la pressione di un tasto per tornare al menu principale e imposta lo stato di fine battaglia.</para>
        /// </remarks>
        /// <returns> Un Task che rappresenta l’operazione asincrona di visualizzazione del dialogo di vittoria.</returns>
        public async Task ZephyrVictoryDialogue()
        {
            Logger.Log($"{player.Name} ha vinto il combattimento con Zephyr", "INFO");
            await Task.Delay(1500);
            await WriteSlowly("\nZephyr: NO, NON PUO ESSERE ...");
            await WriteSlowly("        SONO ... STATO SCONFITTO?");
            await WriteSlowly("        NON E POSSIBILE, i...o so...no in...vin...ci...bile...");
            await Task.Delay(1500);
            await WriteSlowly("\nNARRATORE: Zephyr cade a terra, sconfitto.");
            await WriteSlowly("           Il potere magico all'interno del suo corpo collasa.");
            await WriteSlowly("           E piano piano il suo corpo si tramuta in pura magia, che vola via.");
            await Task.Delay(1500);
            await WriteSlowly($"\nElara: {player.Name}, sei riuscito a sconfiggere Zephyr!");
            await Task.Delay(1500);
            await WriteSlowly("\nNARRATORE: La cupola del salone si apre, mostrandovi il cielo che da tanto non vedete.");
            await WriteSlowly("           Anche il terreno si trasforma, creando una scala che porta all'esterno.");
            await WriteSlowly("           Dopo aver ammirato la trasformazione del salone aiuti Elara ad alzarsi.");
            await WriteSlowly("           Entrambi salite le scale e finalmente uscite dal labirinto.");
            await WriteSlowly("           All'esterno uno squadrone di guardie reali che vi stavano cercando.");
            await WriteSlowly("           Appena vi notano vengono in vostro soccorso, guarendo le vostre ferite.");
            await WriteSlowly("           Fianlmente questo vostro incubo è finito.-");
            await WriteSlowly("           Arrivati nella capitale spiegerete cosa è accaduto al re, il quale premiera i vostri sforzi.");
            await WriteSlowly("           Una volta risolto il tutto sia tu che Elara deciderete di continaure a viaggiare per il mondo, insieme.");
            await WriteSlowly("           Che nuove avventure vi attendano!");
            await Task.Delay(1500);
            await WriteSlowly($"\nSYSTEM: Complimenti {player.Name}, hai vinto!");
            await Task.Delay(1500);
            await WriteSlowly("\nSYSTEM: Grazie per aver giocato a 'Labirinto di Zephyr'!");
            await WriteSlowly("\nSYSTEM: Premi un qualsiasi tasto per tornare al menu principale.");
            ReadKey(true);
            EndBattle = true;
            Logger.Log("Il giocatore ha vinto il combattimento con Zephyr e ha completato il gioco", "INFO");
        }

        /// <summary> Gestisce la sequenza narrativa e il dialogo in caso di sconfitta del giocatore nel combattimento finale contro Zephyr.</summary>
        /// <remarks>
        /// <para> Mostra il discorso di Zephyr trionfante, la caduta del giocatore e il finale oscuro dove Zephyr domina il mondo.</para>
        /// <para> Attende la pressione di un tasto per tornare al menu principale e imposta lo stato di fine battaglia.</para>
        /// </remarks>
        /// <returns> Un Task che rappresenta l’operazione asincrona di visualizzazione del dialogo di sconfitta.</returns>
        public async Task ZephyrDefeatDialogue()
        {
            Logger.Log($"{player.Name} ha perso il combattimento con Zephyr", "INFO");
            await WriteSlowly("\nZephyr: Come vi avevo detto...TUTTO QUELLO CHE AVETE FATTO é STATO INUTILE.");
            await Task.Delay(1500);
            await WriteSlowly("\nNARRATORE: Zephyr ti colpisce con un potente incantesimo, uccidendoti sul colpo.");
            await Task.Delay(1500);
            await WriteSlowly("\nZephyr: ORA TOCCA A TE!");
            await WriteSlowly("\nElara: NOOOOO!");
            await Task.Delay(1500);
            await WriteSlowly("\nNARRATORE: Zephyr, dopo avervi sconfitti, apre la cupola del salone, la quale porta all'esterno.");
            await WriteSlowly("           Una volta uscito Zephyr portera la distruzione nel mondo, diventando il nuovo re dei demoni.");
            await WriteSlowly("           Ormai nessuno può opporsi a lui...");
            await Task.Delay(1500);
            await WriteSlowly($"\nSYSTEM: {player.Name} Hai perso!");
            await Task.Delay(1500);
            await WriteSlowly("\nSYSTEM: Premi un qualsiasi tasto per tornare al menu principale.");
            ReadKey(true);
            EndBattle = true;
            Logger.Log("Il giocatore ha perso il combattimento con Zephyr e ha completato il gioco", "INFO");
        }

        //-----------------------------------------------------------------------------
        //                                 Intro Stanze
        //-----------------------------------------------------------------------------

        /// <summary> Introduce il giocatore all'ambiente della Cripta, descrivendone l'atmosfera e i dettagli visivi e sensoriali.</summary>
        /// <returns> Task asincrono che mostra il testo introduttivo della Cripta.</returns>
        public async Task IntroCripta()
        {
            await Task.Delay(1500);
            await WriteSlowly("\nNARRATORE: La Cripta è avvolta da un silenzio tombale, rotto solo dal suono dei tuoi passi sull'antico pavimento di pietra. ");
            await WriteSlowly("           Al centro della sala si erge una colossale statua scolpita in un marmo oscuro che sembra assorbire la luce.");
            await WriteSlowly("           Gli occhi della statua, incastonati con gemme lucenti, sembrano seguire ogni tuo movimento con inquietante consapevolezza.");
            await WriteSlowly("           Le pareti sono adornate da tappeti polverosi e candelabri spenti, pendenti da catene arrugginite che oscillano lievemente nonostante l'assenza di vento.");
            await WriteSlowly("           L'aria è densa di un odore di muffa e umidità, e un senso di oppressione ti avvolge mentre ti addentri in questo luogo dimenticato.");
        }

        /// <summary> Fornisce l'introduzione narrativa alla stanza dello studio dove il personaggio Condri si è rinchiuso.</summary>
        /// <remarks> Descrive l'arrivo tramite teletrasporto e introduce il dialogo interiore di Condri, creando suspense sulla situazione.</remarks>
        /// <returns> Un Task che rappresenta l’operazione asincrona di visualizzazione del testo narrativo.</returns>
        public async Task IntroStudio()
        {
            await Task.Delay(1500);
            await WriteSlowly("\nNARRATORE: Grazie al teletrasporto ti ritrovi nella stanza in cui 'Condri' si era rinchiuso.");
            await Task.Delay(1500);
            await WriteSlowly("           Senti Condri parlare con se stesso mentre e' intento a mischiare liquidi nel calderone.");
            await WriteSlowly("\nCondri: Quanto ci impiegano a morire? Sono li da un bel po'... Dovrebbero essere nel panico più totale...");
            await WriteSlowly("        Beh...Poco importa, non ci vorra' molto... ");
            keypuzzle.discoveredStudio = true;
        }

        /// <summary> Dialogo che spiega al giocatore dover poter usare la 'Chiave Magica'.</summary>
        /// <remarks>
        /// <para> Se il cancello è chiuso e il giocatore possiede la "Chiave magica", Elara fornisce un dialogo che invita il giocatore ad avanzare.</para>
        /// <para> Se il cancello è chiuso e il giocatore non ha la chiave, Elara informa l'impossibilità di procedere e la necessità di trovare la chiave.</para>
        /// </remarks>
        /// <returns> Un Task che rappresenta l’operazione asincrona di narrazione e dialogo relativo al santuario.</returns>
        public async Task IntroSantuario()
        {
            if (altarpuzzle.cancelloChiuso && player.Inventory.HasItem("Chiave magica"))
            {
                await Task.Delay(500);
                await WriteSlowly("\nElara: Sai, nel mentre che tu cercavi la chiave, io ho continuato ad analizzare il 'cancello'.");
                await WriteSlowly("       Nel mentre che lo facevo mi sono chiesta perchè Zephyr avesse messo un cancello così imponente per quest'area.");
                await WriteSlowly("       Le sue dimensioni e il suo livello di sicurezza sono decisamente inusuali, come se fosse stato creato per impedire l'accesso a qualcosa di molto importante.");
                await WriteSlowly("       Posso solo ipotizzare che il labirinto sia stato costruito intorno ad esso, come per nasconderlo.");
                await WriteSlowly("       Mi chiedo cosa si celi al suo interno.");
                await WriteSlowly($"{player.Name}: L'unico modo per scoprirlo è aprire il cancello e continuare a esplorare.");
                await WriteSlowly("Elara: Hai ragione, tu vai avanti, per il momento restero' qui. Ci sono altre questioni che vorrei comprendere meglio.");
            }
            else if (altarpuzzle.cancelloChiuso && !player.Inventory.HasItem("Chiave magica"))
            {
                await Task.Delay(500);
                await WriteSlowly("\nElara: Sfortunatamente, senza la chiave magica non possiamo aprire il cancello.");
                await WriteSlowly("       Dobbiamo trovare un modo per ottenerla.");
            }
        }

        /// <summary> Gestisce l'introduzione narrativa quando il giocatore scopre la stanza segreta.</summary>
        /// <returns> Un Task asincrono che rappresenta la scoperta della stanza segreta.</returns>
        public async Task IntroSecretRoom()
        {
            await Task.Delay(1500);
            await WriteSlowly("\nNARRATORE: Hai scoperto una stanza segreta, un luogo nascosto ormai dimenticato...");
            await Task.Delay(1500);
            await WriteSlowly($"\n{player.Name}: Che sia questa la stanza di cui parlava Elara? Forse e' qui che Zephyr ha nascosto gli oggetti piu' potenti...");
            teleport.discoveredSecretRoom = true; // Reset per evitare ripetizioni
        }

        //-----------------------------------------------------------------------------
        //                                   Observe
        //-----------------------------------------------------------------------------

        /// <summary> Mostra una descrizione atmosferica della grotta in cui si trova il giocatore, usando una narrazione testuale che appare lentamente. </summary>
        /// <returns> Un Task che rappresenta l’operazione asincrona di narrazione descrittiva della grotta.</returns>
        public async Task ObserveGrotta()
        {
            await Task.Delay(1500);
            await WriteSlowly("\nNARRATORE: All'interno della grotta in cui ti ritrovi l'aria è fresca e umida.");
            await WriteSlowly("           Le pareti sono ricoperte di muschio e le stalattiti pendono da quello che rimane del soffitto.");
            await WriteSlowly("           Riesci a sentire il suono dell'acqua che gocciola dall'alto.");
            await WriteSlowly("           La luce della luna illumina la voragine creatasi dal terremoto.");
            await WriteSlowly("           Sembra un luogo dimenticato dal tempo...");
        }

        /// <summary> Descrive narrativamente l'ingresso principale del labirinto di Zephyr.</summary>
        /// <remarks>
        /// <para>Fornisce un'introduzione atmosferica alla scena del portone antico che funge da barriera iniziale.</para>
        /// <para>Questo passaggio prepara il giocatore all'enigma o all'interazione necessaria per aprirlo.</para>
        /// </remarks>
        /// <returns>Un Task che rappresenta l’operazione asincrona di narrazione descrittiva dell’ingresso.</returns>
        public async Task ObserveIngresso()
        {
            await Task.Delay(1500);
            await WriteSlowly("\nNARRATORE: Un enorme portone di pietra, annerito dal tempo, si erge davanti a te come un guardiano silenzioso.");
            await WriteSlowly("           Al centro, un’ incisione irregolare pulsa lievemente, come se celasse un messaggio o un incantesimo.");
            await WriteSlowly("           Una serratura antica, contornata da simboli consumati, attende una combinazione che pochi riescono a decifrare.");
        }

        /// <summary> Fornisce una descrizione narrativa dettagliata del salone all’interno del labirinto.</summary>
        /// <remarks>
        /// <para> Introduce l’ambiente circostante, gli ostacoli presenti (cancello a nord) e le possibili vie di esplorazione.</para>
        /// <para> Questo metodo prepara il giocatore a esplorare e interagire con l’ambiente circostante.</para>
        /// </remarks>
        /// <returns> Un Task che rappresenta l’operazione asincrona di narrazione del salone.</returns>
        public async Task ObserveSalone()
        {
            await Task.Delay(1500);
            await WriteSlowly("\nNARRATORE: Il salone e' ben illuminato e spazioso, a Nord un 'cancello' imponente impedisce di proseguire in quella direzione.");
            await Task.Delay(1500);
            if (keypuzzle.portboc == true) await WriteSlowly("           Alla tua destra la porta è chiusa.");
            else await WriteSlowly("           Alla tua destra la porta conduce al Laboratorio di Condri.");

            await WriteSlowly("           Alla tua sinistra un arco di pietra e' l'ingresso per delle scale che proseguono verso il basso, oscurate dal buio.");
            await WriteSlowly("           Intorno a te noti oggetti di ogni tipo, sulle pareti vedi dei quadri raffiguranti paesaggi differenti.");

        }

        /// <summary> Descrive l'ambiente della Cripta, fornendo dettagli visivi e sensoriali.</summary>
        /// <returns> Un Task asincrono che rappresenta la narrazione completata della grotta.</returns>
        public async Task ObserveCripta()
        {
            await Task.Delay(1500);
            await WriteSlowly("\nNARRATORE: Osservi la Cripta con attenzione, cercando di cogliere ogni dettaglio che possa rivelarsi utile.");
            await WriteSlowly("           La statua al centro della stanza sembra emanare un'energia maligna, i suoi occhi brillano di una luce misteriosa.");
            await WriteSlowly("           Le pareti sono decorate con antichi affreschi che raccontano storie dimenticate, e il pavimento è coperto da uno spesso strato di polvere.");
            await WriteSlowly("           Alcuni oggetti attirano la tua attenzione.");
        }

        /// <summary> Descrive l'ambiente dello studio alchemico.</summary>
        /// <returns> Un Task che rappresenta l’operazione asincrona di narrazione dell'ambiente.</returns>
        public async Task ObserveStudioAlchemico()
        {
            await Task.Delay(1500);
            await WriteSlowly("\nNARRATORE: Lo studio alchemico è un caos ordinato di pozioni ribollenti, pergamene ingiallite e strumenti arcani sparsi ovunque.");
            await WriteSlowly("           L'aria è impregnata di odori pungenti: zolfo, metalli, erbe sconosciute.");
            await WriteSlowly("           Le pareti sono coperte da formule tracciate con una calligrafia disturbante.");
        }

        /// <summary> Fornisce una descrizione narrativa del santuario.</summary>
        /// <returns> Un Task che rappresenta l’operazione asincrona di narrazione del santuario.</returns>
        public async Task ObserveSanctuary()
        {
            await Task.Delay(1500);
            await WriteSlowly("\nNARRATORE: Il Santuario è un luogo avvolto da un silenzio solenne e quasi irreale.");
            await WriteSlowly("           Al centro della stanza si erge un altare di pietra antica, illuminato da un fascio di luce che scende dall’alto, come un giudizio divino.");
            await WriteSlowly("           Tre piedistalli emergono dalla superficie dell’altare, ciascuno custodisce un oggetto carico di significato:");
            await WriteSlowly("           una moneta d’oro, una clessidra finemente incisa e un cuore scolpito nella pietra scura.");
            await WriteSlowly("           L’aria è intrisa di un senso di attesa, come se la stanza stessa vegliasse su queste reliquie.");
        }

        /// <summary> Introduce la stanza segreta con una descrizione narrativa.</summary>
        /// <returns> Task asincrono che esegue la scrittura del testo narrativo.</returns>
        public async Task ObserveSecretRoom()
        {
            await Task.Delay(1500);
            await WriteSlowly("\nNARRATORE: Hai trovato una stanza segreta, che non era accessibile in altro modo.");
            await WriteSlowly("           La stanza è piccola e buia, al centro c'è una luce che brilla intensamente illuminando una vetrina che espone degli oggetti insoliti.");
            await WriteSlowly("           Sembrano essere di grande valore, forse un reliquiario dimenticato.");
        }

        //-----------------------------------------------------------------------------
        //                                Observe Items
        //-----------------------------------------------------------------------------

        /// <summary> Descrive gli oggetti presenti nella grotta se ancora disponibile.</summary>
        /// <remarks>
        /// <para> Se il giocatore non possiede ancora il bastone, viene mostrata una descrizione narrativa dell'oggetto.</para>
        /// <para> In caso contrario, viene indicato che non ci sono oggetti interessanti.</para>
        /// </remarks>
        /// <returns> Un Task asincrono che rappresenta la narrazione completata degli oggetti nella stanza.</returns>
        public async Task ObserveItemsGrotta()
        {
            await Task.Delay(1500);
            await WriteSlowly("\nNARRATORE: Osservi attentamente la grotta.");
            await Task.Delay(1500);
            if (!player.Inventory.HasItem(Equipment.bastone.Name))
            {
                await WriteSlowly("\n           Sul pavimento della grotta noti uno strano oggetto.");
                await WriteSlowly("           Sembra essere un bastone di legno molto antico e usurato.");
            }
            else
            {
                await WriteSlowly("\n           Non ci sono oggetti che attirano la tua attenzione in questa stanza.");
            }
        }

        /// <summary> Descrive gli oggetti visibili all'ingresso del labirinto, se presenti.</summary>
        /// <remarks>
        /// <para>Questo metodo controlla se nella stanza di ingresso sono presenti la torcia o l'osso.</para>
        /// <para>Se uno dei due oggetti è disponibile e non è già nell'inventario del giocatore, viene descritto.</para>
        /// <para>Se nessuno dei due è presente, il giocatore viene informato che non ci sono oggetti notevoli.</para>
        /// </remarks>
        /// <returns> Un Task asincrono che rappresenta la narrazione completata degli oggetti nella stanza.</returns>
        public async Task ObserveItemsIngresso()
        {
            await Task.Delay(1500);
            await WriteSlowly("\nNARRATORE: Osservi attentamente l'ingresso del labirinto.");
            await Task.Delay(1500);
            if (ingresso.GetItem(Equipment.torcia.Name) != null || ingresso.GetItem(Equipment.osso.Name) != null)
            {
                if (!player.Inventory.HasItem(Equipment.torcia.Name))
                {
                    await WriteSlowly("\n           Attaccata alla parete sulla tua destra noti una torcia spenta.");
                }
                if (!player.Inventory.HasItem(Equipment.osso.Name))
                {
                    await WriteSlowly("\n           Ai piedi del portone noti un piccolo cumulo di resti organici da cui spunta fuori un osso di modeste dimensioni.");
                }
            }
            else
            {
                await Task.Delay(1500);
                await WriteSlowly("\n           Non ci sono oggetti che attirano la tua attenzione in questa stanza.");
            }
        }

        /// <summary> Descrive gli oggetti presenti nel salone attualmente visitato dal giocatore.</summary>
        /// <remarks>
        /// <para> Controlla se specifici oggetti (Specchio, Palantir, Necronomicon) sono ancora disponibili nella stanza e non sono già stati raccolti.</para>
        /// <para> Fornisce una descrizione narrativa per ogni oggetto recuperabile.</para>
        /// <para> Se il giocatore ha raccolto tutti gli oggetti, informa che la stanza è stata svuotata.</para>
        /// <para> Se invece non ci sono più oggetti nella stanza, comunica che non ci sono elementi interessanti da esplorare.</para>
        /// </remarks>
        /// <returns>Un Task che rappresenta l'operazione asincrona di narrazione e descrizione degli oggetti nella stanza.</returns>
        public async Task ObserveItemsSalone()
        {
            await Task.Delay(1500);
            await WriteSlowly($"\nNARRATORE: Osservi attentamente il {player.CurrentRoom.Name} in cerca di oggetti che possano esserti utili.");
            await Task.Delay(1500);
            if (!player.Inventory.HasItem(Equipment.specchio.Name) && salone?.GetItem(Equipment.specchio.Name) != null)
            {
                await WriteSlowly("\n           Uno specchio è appeso al muro, sembra che possa essere utilizzato per osservare la tua immagine.");
            }

            if (!player.Inventory.HasItem(Equipment.palantir.Name) && salone?.GetItem(Equipment.palantir.Name) != null)
            {
                await WriteSlowly("\n           Una sfera nera lucente è posata sopra a un piedistallo, può soggiogare la mente di chi lo utilizza, questi oggetti sono i 'Palantir'.");
            }

            if (!player.Inventory.HasItem(Equipment.necronomicon.Name) && salone?.GetItem(Equipment.necronomicon.Name) != null)
            {
                await WriteSlowly("\n           Un antico libro dal titolo 'Necronomicon' è aperto su un leggio, sembra contenere incantesimi dimenticati.");
            }

            if (player.Inventory.HasItem(Equipment.specchio.Name) && player.Inventory.HasItem(Equipment.palantir.Name) && player.Inventory.HasItem(Equipment.necronomicon.Name))
            {
                await WriteSlowly("\nSYSTEM: Hai raccolto tutti gli oggetti che erano presenti nella stanza.");
            }
            else if (salone?.GetItem(Equipment.specchio.Name) == null && salone?.GetItem(Equipment.palantir.Name) == null && salone?.GetItem(Equipment.necronomicon.Name) == null)
            {
                await WriteSlowly("\n           Non ci sono più oggetti che attirano la tua curiosità.");
            }
        }

        /// <summary> Introduce al giocatore gli oggetti presenti nella Cripta.</summary>
        /// <remarks> Segnala quelli ancora disponibili per il prelievo e indica se sono già stati raccolti tutti.</remarks>
        /// <returns> Task asincrono che mostra la descrizione degli oggetti presenti nella stanza corrente.</returns>
        public async Task ObserveItemsCripta()
        {
            await Task.Delay(1500);
            await WriteSlowly($"\nNARRATORE: Osservi attentamente la {player.CurrentRoom.Name} in cerca di oggetti che possano esserti utili.");
            await Task.Delay(1500);
            if (!player.Inventory.HasItem(Equipment.candela.Name) && cripta?.GetItem(Equipment.candela.Name) != null)
            {
                await WriteSlowly("\n           Ai piedi della statua una candela nera intatta poggia su un supporto di ferro battuto.");
            }
            if (!player.Inventory.HasItem(Equipment.frammentiOssidiana.Name) && cripta?.GetItem(Equipment.frammentiOssidiana.Name) != null)
            {
                await WriteSlowly("\n           Piccoli frammenti di ossidiana incisi sono sparsi in modo ordinato lungo un cerchio disegnato sul pavimento.");
            }
            if (!player.Inventory.HasItem(Equipment.anelloArgento.Name) && cripta?.GetItem(Equipment.anelloArgento.Name) != null)
            {
                await WriteSlowly("\n           Un anello d'argento è rimasto incastrato tra due lastre di pietra, come dimenticato da chi è passato prima di te.");
            }
            if (player.Inventory.HasItem(Equipment.candela.Name) && player.Inventory.HasItem(Equipment.frammentiOssidiana.Name) && player.Inventory.HasItem(Equipment.anelloArgento.Name))
            {
                await WriteSlowly("\nSYSTEM: Hai gia' raccolto tutti gli oggetti che erano presenti nella stanza.");
            }
            else if (cripta?.GetItem(Equipment.candela.Name) == null && cripta?.GetItem(Equipment.frammentiOssidiana.Name) == null && cripta?.GetItem(Equipment.anelloArgento.Name) == null)
            {
                await WriteSlowly("\n           Non ci sono più oggetti che attirano la tua curiosità.");
            }
        }

        /// <summary> Descrive gli oggetti presenti nello studio alchemico attualmente visitato dal giocatore.</summary>
        /// <remarks>
        /// <para> Verifica se specifici oggetti (Pietra filosofale, Teschio di cristallo, Mela d'oro) sono disponibili nella stanza e non ancora raccolti.</para>
        /// <para> Fornisce una descrizione narrativa per ogni oggetto recuperabile.</para>
        /// <para> Se il giocatore ha raccolto tutti gli oggetti, o se non ce ne sono più disponibili, comunica che non ci sono elementi di interesse nella stanza.</para>
        /// </remarks>
        /// <returns> Un Task che rappresenta l’operazione asincrona di narrazione e descrizione degli oggetti nello studio alchemico.</returns>
        public async Task ObserveItemsStudioAlchemico()
        {
            await Task.Delay(1500);
            await WriteSlowly($"\nNARRATORE: Osservi attentamente lo {player.CurrentRoom.Name} in cerca di oggetti che possano esserti utili.");
            await Task.Delay(1500);
            if (!player.Inventory.HasItem(Equipment.pietraFilosofale.Name) && studioAlchemico?.GetItem(Equipment.pietraFilosofale.Name) != null)
            {
                await WriteSlowly("\n           Una pietra filosofale è posata su un tavolo, sembra essere in grado di trasformare i metalli in oro.");
            }
            if (!player.Inventory.HasItem(Equipment.teschioCristallo.Name) && studioAlchemico?.GetItem(Equipment.teschioCristallo.Name) != null)
            {
                await WriteSlowly("\n           Un teschio di cristallo è posto su uno scaffale, sembra essere in grado di comunicare con gli spiriti.");
            }
            if (!player.Inventory.HasItem(Equipment.mela.Name) && studioAlchemico?.GetItem(Equipment.mela.Name) != null)
            {
                await WriteSlowly("\n           Una mela d'oro rinchiusa in una teca, sembra essere in grado di conferire grandi poteri a chi la mangia.");
            }
            if (player.Inventory.HasItem(Equipment.pietraFilosofale.Name) && player.Inventory.HasItem(Equipment.teschioCristallo.Name) && player.Inventory.HasItem(Equipment.mela.Name))
            {
                await WriteSlowly("\n           Non ci sono più oggetti che attirano il tuo sguardo.");
            }
            else if (studioAlchemico?.GetItem(Equipment.pietraFilosofale.Name) == null || studioAlchemico?.GetItem(Equipment.teschioCristallo.Name) == null || studioAlchemico?.GetItem(Equipment.mela.Name) == null)
            {
                await WriteSlowly("\n           Non ci sono più oggetti che attirano la tua curiosità.");
            }
        }
        
        /// <summary> Introduce la descrizione degli oggetti presenti nella stanza segreta, mostrando solo quelli che il giocatore non ha ancora nell'inventario.</summary>
        /// <returns> Task asincrono che esegue la scrittura della descrizione degli oggetti nella stanza.</returns>
        public async Task ObserveItemsSecretRoom()
        {
            await Task.Delay(1500);
            await WriteSlowly($"\nNARRATORE: Osservi attentamente la {player.CurrentRoom.Name} in cerca di oggetti che possano esserti utili.");
            await Task.Delay(1500);
            if (!player.Inventory.HasItem(Equipment.behelit.Name) && stanzaSegreta?.GetItem(Equipment.behelit.Name) != null)
            {
                await WriteSlowly("\n           Un uovo demoniaco dall'aspetto organico di colore rosso e' appoggiato su una targhetta recante la seguente descrizione:");
                await WriteSlowly("           Behelit rosso del conquistatore, attivato dal dolore estremo e dal sacrificio della cosa a te più cara, apre un portale verso la Mano di Dio.");
            }

            if (!player.Inventory.HasItem(Equipment.santoGraal.Name) && stanzaSegreta?.GetItem(Equipment.santoGraal.Name) != null)
            {
                await WriteSlowly("\n           Un calice d'oro, antico e consumato dal tempo, reca la seguente descrizione:");
                await WriteSlowly("           Santo Graal, simbolo di purezza e redenzione.");
            }

            if (!player.Inventory.HasItem(Equipment.fiammaPrometeo.Name) && stanzaSegreta?.GetItem(Equipment.fiammaPrometeo.Name) != null)
            {
                await WriteSlowly("\n           Una fiamma blu, eterna e inestinguibile, reca la seguente descrizione:");
                await WriteSlowly("           Fiamma di Prometeo, simbolo di conoscenza e sacrificio.");
            }

            if (player.Inventory.HasItem(Equipment.behelit.Name) && player.Inventory.HasItem(Equipment.santoGraal.Name) && player.Inventory.HasItem(Equipment.fiammaPrometeo.Name))
            {
                await WriteSlowly("           La stanza è vuota, hai raccolto tutto ciò che si trovava nella vetrina.");
            }
            else if (stanzaSegreta?.GetItem(Equipment.behelit.Name) == null && stanzaSegreta?.GetItem(Equipment.santoGraal.Name) == null && stanzaSegreta?.GetItem(Equipment.fiammaPrometeo.Name) == null)
            {
                await WriteSlowly("           La stanza è vuota, non c'è più nulla da raccogliere.");
            }

        }

        //-----------------------------------------------------------------------------
        //                               Testi LockPuzzle
        //-----------------------------------------------------------------------------

        /// <summary> Mostra il menu di scelta relativo al puzzle della serratura.</summary>
        /// <returns> Task che rappresenta l'operazione asincrona di scrittura a schermo.</returns>
        public async Task IntroLockPuzzle()
        {
            await WriteSlowly("\n1. Tenta di sbloccare il portone.");
            await WriteSlowly("2. Rileggi i numeri.");
            await WriteSlowly("3. Esci dal puzzle.");
            await WriteSlowly("\nSYSTEM: Scegli un'opzione: ", 40, false);
            Logger.Log("Menu a scelta del lock puzzle", "INFO");
        }

        /// <summary> Mostra i numeri binari presenti sulla serratura.</summary>
        /// <returns> Task che rappresenta l'operazione asincrona di scrittura a schermo.</returns>
        public async Task LockPuzzleShowNumbers()
        {
            await Task.Delay(1500);
            await WriteSlowly("\nNARRATORE: Analizzi il lucchetto e noti quattro numeri.");
            await Task.Delay(1500);
            await WriteSlowly($"\n                      {lockpuzzle.binaryNumber1}");
            await WriteSlowly($"\n                      {lockpuzzle.binaryNumber2}");
            await WriteSlowly($"\n                      {lockpuzzle.binaryNumber3}");
            await WriteSlowly($"\n                      {lockpuzzle.binaryNumber4}");
            Logger.Log("Mostrati i numeri del lucchetto", "INFO");
        }

        /// <summary> Gestisce la scelta del giocatore all'interno del puzzle della serratura.</summary>
        /// <param name="lockpuzzle"> L'istanza del puzzle della serratura con dati e stato.</param>
        /// <returns> True se il giocatore esce dal puzzle (porta aperta o esce), false altrimenti.</returns>
        public async Task<bool> LockPuzzleChoice(LockPuzzle lockpuzzle)
        {
            string? playerInput = ReadLine();

            if (playerInput == "1")
            {
                Logger.Log("Il giocatore vuole risolvere l'indovinello", "INFO");
                await WriteSlowly("\nSYSTEM: Inserisci la risposta: ", 40, false);
                string? playerAnswer = ReadLine();

                if (int.TryParse(playerAnswer, out int answer) && answer == lockpuzzle.sum)
                {
                    await Task.Delay(1500);
                    await WriteSlowly("\nNARRATORE: La combinazione è quella corretta e il portone si apre.");
                    lockpuzzle.cont = true;

                    // Connette l'ingresso al salone
                    ingresso.ConnectRoom("nord", salone);
                    salone.ConnectRoom("sud", ingresso);
                    await Task.Delay(1500);
                    await WriteSlowly("\nSYSTEM: Hai sbloccato l'accesso al Salone!");
                    await Task.Delay(1500);
                    await WriteSlowly($"\nELARA: Complimenti {player.Name}, non credevo fossi cosi' intelligente! Ora possiamo proseguire verso il salone davanti a noi.");

                    Logger.Log("Il giocatore ha risolto con successo l'enigma", "INFO");
                    return true; // esci dal ciclo/while nel chiamante
                }
                else
                {
                    await WriteSlowly("\nSYSTEM: Risposta sbagliata.");
                    Logger.Log("La risposta fornita dal giocatore non e' corretta", "INFO");
                    return false;
                }
            }
            else if (playerInput == "2")
            {
                Logger.Log("Il giocatore ha chiesto di rileggere i numeri", "INFO");
                await WriteSlowly("\nSYSTEM: I Numeri sono i seguenti:");
                await Task.Delay(1500);
                await WriteSlowly($"\n             {lockpuzzle.binaryNumber1}");
                await WriteSlowly($"\n             {lockpuzzle.binaryNumber2}");
                await WriteSlowly($"\n             {lockpuzzle.binaryNumber3}");
                await WriteSlowly($"\n             {lockpuzzle.binaryNumber4}");
                return false;
            }
            else if (playerInput == "3")
            {
                await player.PlayerGoesAwayFrom(player.CurrentRoom.GetInteractable("Serratura"));
                return true; // esci dal ciclo/while nel chiamante
            }
            else
            {
                await player.PlayerInputWarn();
                return false;
            }
        }
        
        /// <summary> Mostra la traduzione fornita da Elara riguardo incisione misteriosa sulla serratura.</summary>
        /// <returns> Task che rappresenta l'operazione asincrona di scrittura a schermo.</returns>
        public async Task LockPuzzleIncisione()
        {
            await Task.Delay(1500);
            await WriteSlowly("\nNARRATORE: Provi a leggere l'incisione ma non riesci a comprenderla.");
            await WriteSlowly("           Chiedi aiuto a Elara, la quale sembra conoscere quei simboli, traducendoti cosa c'è scritto.");
            await Task.Delay(1500);
            await WriteSlowly("\nElara: Sembra spiegare come funziona la serratura:");
            await WriteSlowly("\n       'Da il loro mondo i numeri devono venire al nostro'");
            await WriteSlowly("       'Per fare ciò devi moltiplicare ogni cifra per la relativa potenza di 2 (inclusi gli 0)'");
            await WriteSlowly("       'Da destra verso sinistra'");
            await WriteSlowly("       'Successivamente sommare il risultato.'");
        }
        
        //-----------------------------------------------------------------------------
        //                             Testi KeeperPuzzle
        //-----------------------------------------------------------------------------

        /// <summary> Introduce l'indovinello del 'Keeper Puzzle' al giocatore, mostrando la storia e le opzioni.</summary>
        /// <remarks> Se l'indovinello è già stato letto, chiede se vuole riascoltarlo.</remarks>
        /// <returns>Task asincrono per la scrittura del testo a schermo.</returns>
        public async Task IntroKeeper()
        {
            if (keeperpuzzle.asc == false)
            {
                Logger.Log("Introduzione dell'indovinello 'Keeper Puzzle' al giocatore", "INFO");
                await WriteSlowly("\nStatua: Benvenuto Avventuriero, sono il guardiano della Cripta del potente Zephyr.");
                await WriteSlowly("        Risolvi il mio quesito e riceverai in dono uno dei segreti del mio Padrone.");
                await WriteSlowly("        Ascolta con attenzione, perchè se sbaglierai una punizione ti attendera'.");
                await Task.Delay(1500);
                await WriteSlowly("\n        Hai di fronte a te 5 mercanti, numerati da 1 a 5.");
                await WriteSlowly("        Ognuno di essi prova a convincerti di essere quello onesto, in ordine dicono:");
                await WriteSlowly("\n        1. Il mercante 2 è onesto.");
                await WriteSlowly("        2. Il mercante 5 mente.");
                await WriteSlowly("        3. Io sono quello onesto.");
                await WriteSlowly("        4. Il mercante 1 mente.");
                await WriteSlowly("        5. Il mercante 3 mente.");
                await WriteSlowly("\n        6. Esci dall'enigma.");
                await Task.Delay(1500);
                await WriteSlowly("\nStatua: Solamente uno di loro ti permettera' di ricevere la ricompensa...");
                await WriteSlowly($"        Solo chi sa ascoltare le bugie troverà la verità.");
                await WriteSlowly($"        Quindi, {player.Name}, qual e' il mercante onesto?");
                await WriteSlowly("\nSYSTEM: Scegli un'opzione: ", 40, false);
            }
            else if (keeperpuzzle.asc == true)
            {
                await WriteSlowly("\nSYSTEM: Vuoi sentire di nuovo l'indovinello?");
                await WriteSlowly("\n1. Si");
                await WriteSlowly("2. No");
            }
        }

        /// <summary> Avvia il dialogo con la statua per far scegliere al giocatore quale mercante è onesto.</summary>
        /// <returns> Task asincrono di scrittura del dialogo quando si deve risolvere il puzzle statua.</returns>
        public async Task PlayerTryToSolveKeeper()
        {
            await WriteSlowly($"\nStatua: Dimmi {player.Name}, qual e' il mercante onesto?");
            await Task.Delay(1500);
            await WriteSlowly("SYSTEM: Digita un numero da 1 a 5 per rispondere.");
            await WriteSlowly("SYSTEM: Digita 6 per uscire dall'enigma");
            await Task.Delay(1500);
            await WriteSlowly("\nSYSTEM: Scegli un'opzione: ", 40, false);
            Logger.Log($"Il giocatore ha deciso di risolvere l'indovinello", "INFO");
        }

        /// <summary> Gestisce la sequenza di dialogo e suggerimenti quando il giocatore risolve correttamente l'enigma della statua.</summary>
        /// <returns> Task asincrono che completa la scrittura del dialogo in caso di risposta corretta nell'indovinello.</returns>
        public async Task RightAnswerKeeper()
        {
            await Task.Delay(1500);
            await WriteSlowly($"\nStatua: Complimenti {player.Name}, hai giudicato correttamente.");
            await Task.Delay(1500);
            await WriteSlowly("\nNARRATORE: La statua chiude i suoi occhi mostrandoti uno scompartimento segreto che si apre lentamente, al suo interno noti una pergamena.");
            await Task.Delay(1500);
            await WriteSlowly($"\n{player.Name}: Questa pergamena rilascia un forte potere... e' sicuramente una 'Pergamena Magica'! Forse dovrei prenderla e portarla a Elara...");
            await Task.Delay(1500);
            await WriteSlowly("\nSUGGERIMENTO: Fai in modo che sia l'ultimo oggetto del tuo inventario.");
            Logger.Log("Il giocatore ha risolto l'enigma", "INFO");
            Logger.Log("Oggetto pergamena ora può essere raccolto", "INFO");
        }

        /// <summary> Gestisce la risposta errata del giocatore all'enigma della statua.</summary>
        /// <remarks> Infligge danni al giocatore e ripropone l'indovinello.</remarks>
        /// <returns> Task asincrono che rappresenta la scrittura del dialogo in caso di risposta sbagliata.</returns>
        public async Task WrongAnswerKeeper()
        {
            await Task.Delay(1500);
            await WriteSlowly($"\nStatua: Ragiona meglio e ripova... ma fai attenzione perche' la mia ira si scatenera' su di te ogni volta che sbaglierai.");
            await WriteSlowly($"        Solo chi sa ascoltare le bugie troverà la verità, qual e' il mercante onesto, {player.Name}?");
            await Task.Delay(1500);
             await WriteSlowly("\nSYSTEM: Digita un numero da 1 a 5 per rispondere.");
            await WriteSlowly("SYSTEM: Digita 6 per uscire dall' enigma.");
            await WriteSlowly("\nSYSTEM: Scegli un'opzione: ", 40, false);
            Logger.Log("Il giocatore ha inserito la risposta sbagliata", "INFO");
        }

        //-----------------------------------------------------------------------------
        //                              Testi KeyPuzzle
        //-----------------------------------------------------------------------------

        /// <summary> Mostra al giocatore l'introduzione al puzzle delle chiavi ("Key Puzzle"), elencando gli elementi disponibili da aggiungere e fornendo le opzioni di scelta.</summary>
        /// <returns> Task che rappresenta l'operazione asincrona di scrittura del testo.</returns>
        public async Task IntroKeyPuzzle()
        {
            Logger.Log("Introduzione dell'indovinello 'Key Puzzle' al giocatore", "INFO");
            await Task.Delay(1500);
            await WriteSlowly("\nNARRATORE: Sei davanti al tavolo da lavoro, sugli scaffali vedi vari elementi.");
            await Task.Delay(1500);
            await WriteSlowly("\nSYSTEM: Quale elemento vuoi aggiungere?");
            await Task.Delay(1500);
            await WriteSlowly("\n1. Europio (E)");
            await WriteSlowly("2. Oro (Au)");
            await WriteSlowly("3. Rame (Cu)");
            await WriteSlowly("4. Potassio (K)");
            await WriteSlowly("5. Ferro (Fe)");
            await WriteSlowly("6. Ittrio (Y)");
            await WriteSlowly("\n7. Esci dal puzzle.");
            await WriteSlowly("\nSYSTEM: Scegli un'opzione: ", 40, false);
        }

        /// <summary> Mostra il testo del puzzle della Chiave Magica in cui chiede di aggiungere un elemento.</summary>
        /// <returns> Task che rappresenta l'operazione asincrona di scrittura del testo.</returns>
        public async Task KeyPuzzleMenu()
        {
            await Task.Delay(1500);
            await WriteSlowly("\nSYSTEM: L'intruglio nel calderone sta reagendo, quale altro elemento vuoi aggiungere?");
            await WriteSlowly("\nSYSTEM: Scegli un'opzione: ", 40, false);
        }

        /// <summary> Gestisce l'interazione con il puzzle degli elementi da aggiungere all'intruglio nel calderone.</summary>
        /// <remarks> Controlla passo passo l'inserimento degli elementi corretti nella sequenza prevista.</remarks>
        /// <returns> Task che indica se il puzzle è stato completato (true) o se l'input è errato o il giocatore ha scelto di uscire (false).</returns>
        public async Task<bool> KeyPuzzleChoice()
        {

            string? playerInput = ReadLine()?.Trim();

            if (!int.TryParse(playerInput, out int inputNumber) || inputNumber < 1 || inputNumber > 7)
            {
                await player.PlayerInputWarn();
                return false;
            }

            if (playerInput == "4")
            {
                Logger.Log("Il giocatore ha inserito il primo elemento della soluzione correttamente", "INFO");
                await KeyPuzzleMenu();
                string? playerInput2 = ReadLine()?.Trim();

                if (!int.TryParse(playerInput2, out int inputNumber2) || inputNumber2 < 1 || inputNumber2 > 7)
                {
                    await player.PlayerInputWarn();
                    return false;
                }

                if (playerInput2 == "1")
                {
                    Logger.Log("Il giocatore ha inserito il secondo elemento della soluzione correttamente", "INFO");
                    await KeyPuzzleMenu();
                    string? playerInput3 = ReadLine()?.Trim();

                    if (!int.TryParse(playerInput3, out int inputNumber3) || inputNumber3 < 1 || inputNumber3 > 7)
                    {
                        await player.PlayerInputWarn();
                        return false;
                    }

                    if (playerInput3 == "6")
                    {
                        await KeyPuzzleCompleted();
                        keypuzzle.cont = true;
                        var magicKey = player.CurrentRoom.GetItem("Chiave Magica");
                        if (magicKey != null)
                            magicKey.CanBePickedUp = true;
                        Logger.Log("Il giocatore ha risolto l'enigma della chiave", "INFO");
                        return true;
                    }
                    else
                    {
                        await player.TakeDamage(5);
                        Logger.Log("Il terzo elemento inserito è errato", "INFO");
                        return false;
                    }
                }
                else
                {
                    await player.TakeDamage(5);
                    Logger.Log("Il secondo elemento inserito è errato", "INFO");
                    return false;
                }
            }
            else if (playerInput == "7")
            {
                await player.PlayerGoesAwayFrom(player.CurrentRoom.GetInteractable("Calderone"));
                return true; // esce dal puzzle
            }
            else
            {
                await player.TakeDamage(5);
                Logger.Log("Il primo elemento inserito è errato", "INFO");
                return false;
            }
        }

        /// <summary> Gestisce la situazione quando il giocatore completa correttamente il puzzle delle chiavi.</summary>
        /// <remarks> Mostra una descrizione narrativa dell'effetto dell'intruglio e del rilascio della chiave magica, e fornisce un suggerimento utile al giocatore riguardo l'inventario.</remarks>
        /// <returns> Task che rappresenta l'operazione asincrona di scrittura del testo.</returns>
        public async Task KeyPuzzleCompleted()
        {
            Logger.Log("Il giocatore ha inserito l'ultimo elemento della soluzione correttamente", "INFO");
            await Task.Delay(1500);
            await WriteSlowly("\nNARRATORE: L'intruglio si illumina facendo esplodere il calderone.");
            await WriteSlowly("           Il vapore scaturito viene assorbito da una chiave appesa alla parete sulla sinistra del calderone, la quale si stacca dal muro e cade a terra.");
            await Task.Delay(1500);
            await WriteSlowly($"\n{player.Name}: Che strano... sembra una 'Chiave Magica'... Forse con questa posso aprire il 'cancello' del Salone...");
            await Task.Delay(1500);
            await WriteSlowly("\nSUGGERIMENTO: Fai in modo che sia l'ultimo oggetto del tuo inventario.");
        }

        /// <summary> Introduce l'osservazione del tavolino e descrive il foglio trovato.</summary>
        /// <returns> Task asincrono che rappresenta l'operazione di scrittura del testo a schermo.</returns>
        public async Task KeyPuzzleTavolino()
        {
            await Task.Delay(1500);
            await WriteSlowly("\nNARRATORE: Osservi il tavolino e tra i vari oggetti su di esso noti un foglio.");
            await WriteSlowly("           Sul fronte è presente una strana mappa, nella parte superiore c'è scritto: Tavola Periodica.");
            await WriteSlowly("           Sul retro invece c'è scritto: Da usare come una KEY.");
            await WriteSlowly("           Mentre osservi il foglio noti 'Condri' palesemente agitato.");
        }

        //-----------------------------------------------------------------------------
        //                             Testi AperturaCancello
        //-----------------------------------------------------------------------------

        /// <summary> Mostra il menu di scelta per il giocatore relativo all'apertura del cancello con la Chiave Magica.</summary>
        /// <returns> Task asincrono che esegue la scrittura del menu e registra il log dell'evento.</returns>
        public async Task CancelloMenu()
        {
            await WriteSlowly("\nSYSTEM: Hai la Chiave Magica, vuoi aprire il cancello?.");
            await WriteSlowly("\n1. Si");
            await WriteSlowly("2. No");
            await WriteSlowly("\nSYSTEM: Scegli un'opzione: ", 40, false);
            Logger.Log("Menu di scelta per aprire il cancello", "INFO");
        }

        /// <summary> Gestisce la scelta del giocatore riguardo all'apertura del cancello con la Chiave Magica.</summary>
        /// <returns> Task asincrono che esegue la logica in base alla scelta del giocatore.</returns>
        public async Task CancelloMenuChoice()
        {
            string? playerInput = ReadLine()?.Trim();
            if (playerInput == "1")
            {
                altarpuzzle.cancelloChiuso = false;
                salone.ConnectRoom("nord", santuario);
                santuario.ConnectRoom("sud", salone);
                await Task.Delay(1500);
                await WriteSlowly("\nNARRATORE: Il cancello si apre lentamente, rivelando un corridoio che conduce al Santuario.");
                await Task.Delay(1500);
                await WriteSlowly("\nSYSTEM: Hai sbloccato l'accesso al Santuario.");
                await Task.Delay(1500);
                await WriteSlowly("\nElara: Finalmente, il cancello è aperto! Possiamo proseguire verso nord!");
                Logger.Log("Il giocatore ha aperto il cancello verso il santuario", "INFO");
            }
            else if (playerInput == "2")
            {
                await WriteSlowly("\nNARRATORE: Hai deciso di non aprire il cancello.");
                Logger.Log("Il giocatore ha scelto di non aprire il cancello", "INFO");
            }
            else if (!player.Inventory.HasItem("Chiave Magica"))
            {
                await WriteSlowly("\nSYSTEM: Non hai la chiave per aprire il cancello.");
                Logger.Log("Il giocatore ha tentato di aprire il cancello senza la chiave", "WARN");
            }
            else
            {
                await player.PlayerInputWarn();
            }
        }

        //-----------------------------------------------------------------------------
        //                              Testi AltarPuzzle
        //-----------------------------------------------------------------------------

        /// <summary> Introduce l'enigma dell'altare nel Santuario.</summary>
        /// <remarks> 
        /// <para> Se il puzzle è già stato completato, mostra un messaggio di conferma e termina.</para>
        /// <para> Altrimenti, descrive la scena e l'enigma al giocatore.</para>
        /// </remarks>
        /// <returns> Task asincrono che rappresenta l'introduzione del puzzle dell'altare.</returns>
        public async Task IntroAltar()
        {
            if (altarpuzzle.cont == true)
            {
                await WriteSlowly("\nSYSTEM: Hai già dimostrato di essere degno di proseguire. Continua il tuo cammino.");
                Logger.Log("Il giocatore ha già completato il puzzle dell'altare.", "INFO");
                return;
            }
            else
            {
                await Task.Delay(1500);
                await WriteSlowly("\nNARRATORE: Sopra l'altare tre piedistalli dorati reggono oggetti consumati dal tempo: ");
                if (!player.Inventory.HasItem(altarpuzzle.moneta.Name))
                {
                    await WriteSlowly("\n           Una moneta raffigurante una donna bendata che sorregge una bilancia.");
                }
                if (!player.Inventory.HasItem(altarpuzzle.clessidra.Name))
                {
                    await WriteSlowly("\n           Una clessidra la cui sabbia scandisce lo scorrere del tempo.");
                }
                if (!player.Inventory.HasItem(altarpuzzle.cuore.Name))
                {
                    await WriteSlowly("\n           Un cuore scolpito su una pietra nera avvolto da fiamme ardenti di colore blu.");
                }
                await WriteSlowly("\n           Rune incise sui piedistalli pulsano lievemente, come in attesa di una risposta. Una stele marmorea, incastonata ai piedi dell'altare, reca parole enigmatiche:");
                await WriteSlowly("           Solo colui che cammina nella via della ..., apprende il silenzio che guida la vera ..., e arde nel fuoco puro del ..., potrà proseguire il suo cammino.");
                await WriteSlowly("           Tre luci dormono nell'oscurità. Solo la virtù potrà risvegliarle.");
                Logger.Log("Introduzione dell'enigma dell'altare", "INFO");
            }
        }

        /// <summary> Mostra al giocatore le opzioni disponibili nell'enigma dell'altare, in base agli oggetti ancora da raccogliere o già in possesso.</summary>
        /// <returns> Task asincrono che rappresenta il completamento della visualizzazione del menu di scelta nel puzzle altare.</returns>
        public async Task AltarTakeItemOption()
        {
            await WriteSlowly("\nSYSTEM: Cosa vuoi fare?");
            WriteLine("");
            if (!player.Inventory.HasItem(altarpuzzle.moneta.Name) && santuario.Items.Contains(altarpuzzle.moneta))
            {
                await WriteSlowly("1. Raccogli la moneta con donna bendata.");
            }
            if (!player.Inventory.HasItem(altarpuzzle.clessidra.Name) && santuario.Items.Contains(altarpuzzle.clessidra))
            {
                await WriteSlowly("2. Raccogli la clessidra.");
            }
            if (!player.Inventory.HasItem(altarpuzzle.cuore.Name) && santuario.Items.Contains(altarpuzzle.cuore))
            {
                await WriteSlowly("3. Raccogli il cuore di pietra ardente");
            }
            await WriteSlowly("4. Credo di conoscere la risposta.");
            await WriteSlowly("\n5. Esci dall'enigma.");
            if (player.Inventory.HasItem(altarpuzzle.clessidra.Name))
            {
                await WriteSlowly("6. Interagisci con la clessidra");
            }
            await WriteSlowly("\nSYSTEM: Scegli un'opzione: ", 40, false);
            Logger.Log("Elenco di scelta multipla dell'enigma.", "INFO");
        }

        /// <summary> Gestisce la scelta dell'utente nell'enigma del santuario.</summary>
        /// <remarks> Permette di raccogliere oggetti, interagire con essi, tentare di risolvere il puzzle o abbandonare la sfida.</remarks>
        /// <returns> Ritorna <c>false</c> in tutti i casi per mantenere il ciclo di interazione attivo.</returns>
        public async Task<bool> AltarTakeItemChoice()
        {
            string? choice = ReadLine()?.Trim();
            if (choice == "1" && !player.Inventory.HasItem(altarpuzzle.moneta.Name) && santuario.Items.Contains(altarpuzzle.moneta))
            {
                Logger.Log("Il giocatore prova a raccogliere la moneta con donna bendata", "INFO");
                await player.Inventory.TakeItem(altarpuzzle.moneta.Name);
                if (player.Inventory.HasItem(altarpuzzle.moneta.Name))
                {
                await Task.Delay(1500);
                await WriteSlowly("\nNARRATORE: Raccogli la moneta con donna bendata. La luce della moneta sembra riflettere un misterioso potere.");
                await WriteSlowly("           Una voce fiebile riecheggia nella stanza: 'Chi porta la moneta con la donna bendata porta con sé il peso della vera giustizia.'");
                }
                return false;
            }
            else if (choice == "2" && !player.Inventory.HasItem(altarpuzzle.clessidra.Name) && santuario.Items.Contains(altarpuzzle.clessidra))
            {
                Logger.Log("Il giocatore prova a raccogliere la clessidra", "INFO");
                await player.Inventory.TakeItem(altarpuzzle.clessidra.Name);
                if (player.Inventory.HasItem(altarpuzzle.clessidra.Name))
                {
                    await Task.Delay(1500);
                    await WriteSlowly("\nNARRATORE: Raccogli la clessidra e la sabbia al suo interno smette di scorrere, il tempo sembra fermarsi.");
                    await WriteSlowly("           Una voce fiebile riecheggia nella stanza: 'Non toccare ciò che scorre nel silenzio del tempo. La vera saggezza è nell'attesa, non nell' impazienza.'");
                    await WriteSlowly("           Hai come la sensazione che la clessidra sia un monito a non affrettare le decisioni, forse dovresti lasciarla al suo posto.");
                }
                await altarpuzzle.ClessidraInteraction();
                return false;
            }
            else if (choice == "3" && !player.Inventory.HasItem(altarpuzzle.cuore.Name) && santuario.Items.Contains(altarpuzzle.cuore))
            {
                Logger.Log("Il giocatore prova a raccogliere il cuore di pietra ardente", "INFO");
                await player.Inventory.TakeItem(altarpuzzle.cuore.Name);
                if (player.Inventory.HasItem(altarpuzzle.cuore.Name))
                {
                    await Task.Delay(1500);
                    await WriteSlowly("\nNARRATORE: Raccogli il cuore di pietra ardente. Il suo calore ti avvolge, come se contenesse un fuoco eterno.");
                    await WriteSlowly("           Una voce fiebile riecheggia nella stanza: 'Nel cuore di pietra e fiamma dimora la forza di chi ha il coraggio di sfidare il destino.'");
                }
                return false;

            }
            else if (choice == "4")
            {
                await WriteSlowly("\nSYSTEM: Hai scelto di rispondere all'enigma. Sei pronto a dimostrare di possedere le virtù necessarie per proseguire il tuo cammino.");
                Logger.Log("Il giocatore tenta di risolvere l'enigma", "INFO");
                await altarpuzzle.SolveAltarPuzzleWithInteraction();
                return false;
            }
            else if (choice == "5")
            {
                await WriteSlowly("\nSYSTEM: Hai deciso di non tentare la prova. Tornerai quando sarai pronto.");
                Logger.Log("Il giocatore ha scelto di uscire dal puzzle senza completarlo", "INFO");
                await player.PlayerGoesAwayFrom(player.CurrentRoom.GetInteractable("Altare"));
                return true; //uscire da ciclo
            }
            else if (choice == "6" && player.Inventory.HasItem("Clessidra"))
            {
                await altarpuzzle.ClessidraInteraction();
                Logger.Log("Il giocatore interagisce con la clessidra", "INFO");
                return false;
            }
            else
            {
                await player.PlayerInputWarn();
                return false;
            }
        }

        /// <summary> Mostra il menu di interazione con la clessidra.</summary>
        /// <returns> Non restituisce alcun valore (Task), ma attende l'interazione dell'utente.</returns>
        public async Task ClessidraMenuOption()
        {
            await Task.Delay(1500);
            await WriteSlowly("\nNARRATORE: Osservi la clessidra, la cui sabbia ha smesso di scandire lo scorrere del tempo.");
            await WriteSlowly("\n1. Tieni la clessidra");
            await WriteSlowly("2. Riponi la clessidra");
            await WriteSlowly("\nSYSTEM: Scegli un'opzione: ", 40, false);
            Logger.Log("Menu scelta multipla della clessidra", "INFO");
        }

        /// <summary> Gestisce la scelta del giocatore riguardo la clessidra nell'enigma del santuario.</summary>
        /// <remarks> Il giocatore può decidere se tenerla o riporla, con conseguenze narrative e logiche.</remarks>
        /// <returns> Restituisce <c>true</c> se l'azione è andata a buon fine (tenere o riporre la clessidra correttamente), <c>false</c> se l'input non è valido o la clessidra non può essere riposta perché non è l'ultimo oggetto raccolto.</returns>
        public async Task<bool> ClessidraOptionChoice()
        {
            string? secondchoice = ReadLine()?.Trim();
            if (secondchoice == "1")
            {
                await WriteSlowly("\nNARRATORE: Hai deciso di tenere la clessidra. La sua presenza ti ricorderà l'importanza della pazienza.");
                Logger.Log("Il giocatore ha scelto di tenere la clessidra", "INFO");
                return true;
            }
            else if (secondchoice == "2")
            {
                await player.Inventory.DropItem(altarpuzzle.clessidra.Name);
                if (!player.Inventory.HasItem(altarpuzzle.clessidra.Name))
                {
                    await WriteSlowly("\nNARRATORE: Hai riposto la clessidra nel suo piedistallo, il tempo torna scorrere.");
                    await WriteSlowly("           Il tempo svela ciò che la fretta nasconde. La saggezza cresce nel silenzio dell'attesa.");
                    return true;
                }
                else if (player.Inventory.HasItem(altarpuzzle.clessidra.Name))
                {
                    await WriteSlowly("\nSYSTEM: La clessidra non è l'ultimo oggetto che hai raccolto, scelta non utilizzabile");
                    Logger.Log("Il giocatore ha tentato di riporre la clessidra ma non era l'ultimo oggetto raccolto", "WARN");
                    return false;
                }

                return false; //non dovrebbe essere mai raggiungibile ma se non si mette da errore
            }
            else
            {
                await player.PlayerInputWarn();
                return false;
            }
        }

        /// <summary> Presenta l'enigma finale del santuario al giocatore.</summary>
        /// <remarks> Una voce misteriosa descrive tre virtù, e il giocatore deve rispondere indicando la sequenza corretta.</remarks>
        /// <returns> Task asincrono che visualizza il testo dell'enigma e richiede la risposta dell'utente.</returns>
        public async Task AltarGiveAnswer()
        {
            await Task.Delay(1500);
            await WriteSlowly("\nNARRATORE: Una voce misteriosa risuona nella stanza: 'Risolvi l'enigma per completare la tua prova'.");
            await WriteSlowly("           Tre virtu' celano la soglia dell'anima: la prima dà equilibrio al mondo, la seconda conosce ciò che il tempo rivela, la terza non indietreggia davanti al fuoco.");
            await Task.Delay(1500);
            await WriteSlowly("\nSYSTEM: Rispondi con la sequenza corretta delle parole: ", 40, false);
            Logger.Log("Presentazione enigma", "INFO");
        }

        /// <summary> Mostra la sequenza narrativa e gli effetti in gioco dopo che il giocatore ha risposto correttamente all'enigma del santuario.</summary>
        /// <remarks> Attiva il flag di completamento dell'enigma e introduce un importante evento narrativo legato alla spada e alla figura della dea.</remarks>
        /// <returns> Task asincrono che narra l'evento successivo alla risoluzione dell'enigma. Non restituisce un valore, ma modifica lo stato interno (`altarpuzzle.cont = true`) e aggiorna il contesto narrativo.</returns>
        public async Task AltarCorrectAnswer()
        {
            await WriteSlowly($"\nSYSTEM: Hai superato la prova dell'anima, {player.Name}. Le virtu' fanno di te un vero eroe, prosegui il tuo cammino ora niente potra' fermarti.");
            await Task.Delay(1500);
            await WriteSlowly("\nNARRATORE: La stele marmorea si illumina e le rune sui piedistalli iniziano a brillare intensamente.");
            altarpuzzle.cont = true;
            Logger.Log("Il giocatore ha gli oggetti giusti nell'inventario e ha risposto correttamente", "INFO");
            await WriteSlowly("           L'altare si sgretola e un intenso bagliore illumina la stanza, accecandoti.");
            await WriteSlowly("           Una figura eterea e' davanti a te, inginocchiandosi e con le braccia rivolte verso il cielo ti porge una spada.");
            await WriteSlowly("           Attirata dal rumore Elara entra nel Santuario e rimane senza parole.");
            await WriteSlowly("\nElara: Non credo ai miei occhi! E' la 'Dea' del santuario perduto, custode della spada leggendaria, in grado di ferire anche l'essere piu' potente!");
            await WriteSlowly("       Tutti erano convinti che avesse lasciato il nostro mondo centinaia di anni fa e invece era stato Zephyr a imprigionarla!");
            await WriteSlowly("\nNARRATORE: La spada sembra pulsare di energia, come se fosse stata forgiata con le stesse virtù che hai dimostrato di possedere.");
            await WriteSlowly($"\nDea: Non temere {player.Name}, sono qui per aiutarti.");
            await WriteSlowly("\nElara: Questo spiega tutto, e forse con questa spada potremo scappare di qui, torno nel salone per cercare indizi su come uscire da questo labirinto.");
            await WriteSlowly("\nNARRATORE: Elara corre nel salone per indagare, non appena varca la soglia del cancello un urlo di terrore arriva dal Salone e comprendi che qualcosa di terribile sta accadendo.");
        }

        /// <summary> Gestisce il caso in cui il giocatore fornisce una risposta errata all'enigma del santuario.</summary>
        /// <remarks> Il comportamento varia in base agli oggetti attualmente presenti nell'inventario del giocatore.</remarks>
        /// <returns>
        /// <para>Task asincrono che esegue la logica narrativa e interattiva legata a una risposta errata:</para>
        /// <para>- Se il giocatore ha la clessidra, viene invocata la sua funzione.</para>
        /// <para>- Se ha tutti gli oggetti tranne la clessidra, gli viene concesso di riprovare.</para>
        /// <para>- Se mancano altri oggetti, viene avvisato di non essere pronto.</para>
        /// </returns>
        public async Task AltarCorrectWrongAnswer()
        {
            await WriteSlowly("\nNARRATORE: Non hai superato la prova.");
            if (player.Inventory.HasItem(altarpuzzle.clessidra.Name))
            {
                await Task.Delay(1500);
                await WriteSlowly("\nNARRATORE: La clessidra sembra pulsare lievemente, come se volesse tornare al suo posto.");
                Logger.Log("Il giocatore ha la clessidra nell'inventario, prova non superata", "INFO");
                await altarpuzzle.ClessidraInteraction();
            }
            else if (!player.Inventory.HasItem(altarpuzzle.clessidra.Name) && santuario.Items.Contains(altarpuzzle.clessidra) && player.Inventory.HasItem(altarpuzzle.moneta.Name) && player.Inventory.HasItem(altarpuzzle.cuore.Name))
            {
                await WriteSlowly("\nNARRATORE: Rifletti sulle parole e sul loro ordine.");
                Logger.Log("Il giocatore ha risposto in modo errato ma ha gli oggetti giusti nell'inventario", "INFO");
                await WriteSlowly("SYSTEM: Scegli un'opzione:");
                await WriteSlowly("\n1. Riprova a risolvere l'enigma");
                await WriteSlowly("2. Esci dall'enigma");
                string? choice = ReadLine()?.Trim();
                // Permetti al giocatore di riprovare a risolvere l'enigma
                if (choice == "1")
                {
                    await altarpuzzle.SolveAltarPuzzleWithInteraction();
                }
                else if (choice == "2")
                {
                    await player.PlayerGoesAwayFrom(player.CurrentRoom.GetInteractable("Altare"));
                    return;
                }
                else
                {
                    await player.PlayerInputWarn();
                }
            }
            else if (!player.Inventory.HasItem(altarpuzzle.clessidra.Name) && santuario.Items.Contains(altarpuzzle.clessidra) && (!player.Inventory.HasItem(altarpuzzle.moneta.Name) || !player.Inventory.HasItem(altarpuzzle.cuore.Name)))
            {
                await WriteSlowly("\nNARRATORE: Non hai ancora raccolto gli oggetti necessari per risolvere l'enigma.");
                Logger.Log("Il giocatore non ha tutti gli oggetti necessari nell'inventario", "INFO");
            }
            else if (!santuario.Items.Contains(altarpuzzle.clessidra))
            {
                await WriteSlowly("NARRATORE: La clessidra deve stare al suo posto per risolvere l'enigma.");
                Logger.Log("Il giocatore ha lasciato la clessidra in una stanza diversa dall'altare", "INFO");
            }
        }

        //-----------------------------------------------------------------------------
        //                               Controllo Testi Interattivi
        //-----------------------------------------------------------------------------

        /// <summary> Controlla la stanza corrente del giocatore e lo stato degli enigmi, per far partire il dialogo o evento appropriato.</summary>
        /// <returns> Task asincrono che esegue la funzione di dialogo corrispondente.</returns>
        public async Task CheckDialogueCondition()
        {
            if (player.CurrentRoom == ingresso && lockpuzzle.cont == false && altarpuzzle.cont == false && keypuzzle.cont == false && keeperpuzzle.cont == false)
            {
                await IngressoDoorLocked();
            }
            else if (player.CurrentRoom == salone && lockpuzzle.cont == true && altarpuzzle.cont == false && keypuzzle.cont == false && keeperpuzzle.cont == false)
            {
                await IntroduzioneCondri();
            }
            else if (player.CurrentRoom == salone && lockpuzzle.cont == true && keeperpuzzle.cont == true && player.Inventory.HasItem(keeperpuzzle.pergamenaMagica.Name))
            {
                await ElaraWantsMagicScroll();
            }
            else if (player.CurrentRoom == studioAlchemico && lockpuzzle.cont == true && keeperpuzzle.cont == true && keypuzzle.cont == false && keypuzzle.portboc == true && !keypuzzle.discoveredStudio)
            {
                await IntroStudio();
            }
            else if (player.CurrentRoom == salone && altarpuzzle.cont == true)
            {
                await ZephyrIntroduction();
            }
            else if (player.CurrentRoom == salone && altarpuzzle.cont == false && keypuzzle.cont == true)
            {
                await IntroSantuario();
            }
            else if (player.CurrentRoom == salone && altarpuzzle.cont == false && keypuzzle.cont == false && keeperpuzzle.cont == true && !player.Inventory.HasItem(keeperpuzzle.pergamenaMagica.Name) && teleport.telAtt == false)
            {
                await PlayerNeedSomethingToOpenDoor();
            }
            else if (player.CurrentRoom == santuario && altarpuzzle.cont == false && keypuzzle.cont == false && keeperpuzzle.cont == false)
            {
                await PlayerNeedSomethingToOpenDoor();
            }
            else if (player.CurrentRoom == cripta && lockpuzzle.cont == true && keeperpuzzle.cont == false)
            {
                await IntroCripta();
            }
            else if (player.CurrentRoom == stanzaSegreta && !teleport.discoveredSecretRoom)
            {
                await IntroSecretRoom();
            }
        }
    }
}