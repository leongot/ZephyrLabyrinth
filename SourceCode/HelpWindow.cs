namespace ZephyrLabyrinth
{
    /// <summary> Classe che gestisce la visualizzazione della finestra di aiuto con i comandi di gioco.</summary>
    class HelpW
    {
        /// <summary> Mostra la finestra di aiuto in una nuova thread STA con i comandi di gioco.</summary>
        /// <returns> Nessuno. La finestra viene mostrata in un thread separato.</returns>
        public static void ShowHelpWindow()
        {
            // Crea un nuovo thread per l'interfaccia grafica con modello STA (Single Threaded Apartment)
            Thread t = new Thread(() =>
            {
                // Abilita gli stili visuali di Windows per la finestra
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // Crea una nuova finestra di tipo Form
                Form helpForm = new Form
                {
                    Text = "Aiuto di gioco",     // Titolo finestra
                    Width = 400,                // Larghezza iniziale finestra
                    Height = 200,               // Altezza iniziale finestra
                    MinimumSize = new Size(1000, 450), // Dimensione minima finestra ridimensionabile
                };

                // Crea una TextBox multilinea e in sola lettura per mostrare il testo di aiuto
                TextBox helpText = new TextBox
                {
                    Multiline = true,            // Supporta più righe
                    Dock = DockStyle.Fill,       // Occupa tutto lo spazio della finestra
                    ReadOnly = true,             // Solo lettura, nessuna modifica dall'utente
                    WordWrap = true,             // Va a capo automaticamente
                    ScrollBars = ScrollBars.Vertical, // Barra di scorrimento verticale
                    BackColor = Color.Black,     // Sfondo nero
                    Font = new Font("Consolas", 15, FontStyle.Bold), // Font monospaziato e grassetto
                    ForeColor = Color.White,     // Testo bianco
                    Text =                       // Testo con i comandi e descrizioni
@"
    COMANDO                             DESCRIZIONE
    ----------------------------------------------------------------------------------
    move [direzione]               |    Spostati in un altra stanza.
    take [nome oggetto]            |    Prendi un oggetto.
    drop [nome oggetto]            |    Lascia un oggetto.
    inventory                      |    Mostra il tuo inventario.
    stats                          |    Mostra le statistiche del personaggio.
    interact [interagibile]        |    Interagisci con un oggetto interagibile.
    observe room                   |    Osserva la stanza attuale.
    observe items                  |    Osserva gli oggetti nella stanza.
    map                            |    Mostra la mappa di gioco.
    talk [nome personaggio]        |    Parla con un personaggio.
    teleport                       |    Teletrasportati in una stanza a caso.
    save                           |    Salva il gioco.
    load                           |    Carica una partita salvata.
    help                           |    Mostra la finestra con i comandi possibili.
    quit                           |    Esci dal gioco.
    ----------------------------------------------------------------------------------"
                };

                // Aggiunge la TextBox alla finestra
                helpForm.Controls.Add(helpText);

                // Avvia il loop dell'applicazione per la finestra
                Application.Run(helpForm);
            });
            
            // Imposta il thread come STA (necessario per Windows Forms)
            t.SetApartmentState(ApartmentState.STA);
            // Imposta il thread come background (si chiude quando il processo termina)
            t.IsBackground = true;
            // Avvia il thread che mostra la finestra di aiuto
            t.Start();
        }
    }
}
