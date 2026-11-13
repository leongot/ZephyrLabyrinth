namespace ZephyrLabyrinth
{
    public class KeyPuzzle
    {
        /// <summary> Indica se il puzzle è stato risolto (true = risolto).</summary>
        public bool cont = false;
        public GameItem chiaveMagica = new GameItem("Chiave Magica", false, 1, "Una chiave che puo' assorbire vapori magici.");

        /// <summary> Indica se la porta è bloccata (true = bloccata).</summary>
        public bool portboc = true;
        public bool discoveredStudio = false;

        /// <summary> Metodo principale per gestire l'interazione con il puzzle del calderone.</summary>
        /// <remarks>
        /// <para>- Se la porta non è ancora stata sbloccata, il giocatore affronta un puzzle a tentativi.
        /// <para>- Se il puzzle è già risolto, viene notificato che il calderone è esploso.
        /// <para>- Se invece la porta è ancora bloccata da Condri, viene chiamato un messaggio informativo.
        /// </remarks>
        /// <returns> Un Task che rappresenta l'operazione asincrona di risoluzione del puzzle.</returns>
        public async Task Puzzle()
        {
            // Se la porta è sbloccata
            if (portboc == false)
            {
                // Se il puzzle non è risolto
                if (cont == false)
                {
                    // Loop per permettere più tentativi di risoluzione
                    while (true)
                    {
                        await dialogue.IntroKeyPuzzle();
                        bool risolto = await dialogue.KeyPuzzleChoice();

                        // Se il puzzle è risolto esce dal loop
                        if (risolto) break;
                        if (player.Health <= 0) break;
                    }
                }
                else
                {
                    // Se il puzzle è già stato risolto mostra messaggio di stato
                    await WriteSlowly("\nSYSTEM: Il calderone è esploso, non è utilizzabile.");
                    Logger.Log("Puzzle completato", "INFO");
                }
            }
            else
            {
                // Se la porta è ancora bloccata, mostra info che bisogna occuparsi di Condri
                await InfoCondri();
            }
        }

        /// <summary> Metodo per aprire la porta tra Studio Alchemico e Salone.</summary>
        /// <returns> Un Task che rappresenta l'operazione asincrona di apertura della porta.</returns>
        public void OpenDoorStudioAlchemico()
        {
            // Controlla che il giocatore sia nello Studio Alchemico
            if (player.CurrentRoom == studioAlchemico)
            {
                // Connetti le stanze tra di loro
                studioAlchemico.ConnectRoom("ovest", salone);
                salone.ConnectRoom("est", studioAlchemico);

                // Porta non più bloccata
                portboc = false;

                Logger.Log("Lo Studio Alchemico e' stato connesso con il Salone", "INFO");
            }
        }

        /// <summary> Metodo per gestire l'interazione con il tavolino nel puzzle. </summary>
        /// <returns> Un Task che rappresenta l'operazione asincrona di interazione con il tavolino.</returns>
        public async Task TavolinoInteraction()
        {
            // Se la porta è aperta, esegui il dialogo del tavolino
            if (portboc == false)
            {
                await dialogue.KeyPuzzleTavolino();
            }
            else
            {
                // Altrimenti mostra info che bisogna prima occuparsi di Condri
                await InfoCondri();
            }
        }

        /// <summary> Metodo che mostra il messaggio per occuparsi di Condri prima di procedere.</summary>
        /// <returns> Un Task che rappresenta l'operazione asincrona di visualizzazione del messaggio.</returns>
        public async Task InfoCondri()
        {
            await WriteSlowly($"\nSYSTEM: Non è il momento, devi prima occuparti di Condri");
            Logger.Log("Il giocatore deve prima interagire con Condri", "INFO");
        }
    }
}
