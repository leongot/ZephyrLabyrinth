namespace ZephyrLabyrinth
{
    public class KeeperPuzzle
    {
        
        /// <summary> Flag che indica se il puzzle è stato risolto (true = risolto).</summary>
        public bool cont = false;

        /// <summary> Flag che indica se il giocatore ha già ascoltato l'indovinello.</summary>
        public bool asc = false;

        /// <summary> Oggetto che rappresenta la pergamena magica.</summary>     
        /// <remarks> La pergamena magica è un oggetto che il giocatore può utilizzare per attivare il teletrasporto.</remarks>
        public GameItem pergamenaMagica = new GameItem("Pergamena Magica", false, 2, "Una pergamena antica dal grande potere.");

        /// <summary>Variabile per far comparire l'enigma.</summary>
        /// <remarks> True per mostrare l'enigma.</remarks>
        bool testo = false;

        /// <summary> Variabile per chiedere se riascoltare l'enigma.</summary> 
        /// /// <remarks> True per riascoltarlo.</remarks>

        bool testo2 = false;        
        /// <summary>Gestisce l'interazione del giocatore con il puzzle della statua.</summary>
        /// <remarks>
        /// <para>- Se il puzzle non è stato risolto, il giocatore può ascoltare o rileggere l'indovinello e provare a risolverlo.</para>
        /// <para>- Il puzzle accetta risposte numeriche da 1 a 6.</para>
        /// <para>- La risposta corretta è il numero 4: risolvendo il puzzle, il giocatore può raccogliere la pergamena magica.</para>
        /// <para>- Risposte errate infliggono danno al giocatore (-10 HP).</para>
        /// <para>- Il giocatore può scegliere di abbandonare l'enigma selezionando l'opzione 6.</para>
        /// <para>- Se il puzzle è già stato risolto, la statua diventa inattiva e non è più interagibile.</para>
        /// </remarks>
        public async Task Puzzle()
        {
            // Se il puzzle non è stato risolto
            if (cont == false)
            {
                // Loop finché l'indovinello non viene mostrato
                while (testo == false && (player.Health > 0))
                {
                    // Se il giocatore non ha ancora ascoltato l'indovinello
                    if (asc == false)
                    {
                        // Mostra l'introduzione del puzzle statua
                        await dialogue.IntroKeeper();
                        testo = true; // Indovinello mostrato
                    }
                    else
                    {
                        // Se il giocatore ha già ascoltato, mostra di nuovo l'introduzione
                        await dialogue.IntroKeeper();

                        // Loop per chiedere se vuole riascoltare l'indovinello
                        while (testo2 == false && player.Health > 0)
                        {
                            await WriteSlowly("\nSYSTEM: Scegli un'opzione: ", 40, false);
                            string? playerInput2 = ReadLine();

                            // Controlla che l'input sia un numero valido tra 1 e 2
                            if (!int.TryParse(playerInput2, out int inputNumber) || inputNumber > 2 || inputNumber <= 0)
                            {
                                await player.PlayerInputWarn();
                                continue; // Chiede di nuovo l'input
                            }

                            if (playerInput2 == "1")
                            {
                                // Il giocatore vuole riascoltare l'indovinello
                                asc = false;
                                testo2 = true;
                                Logger.Log("Il giocatore ha deciso di rileggere l'indovinello", "INFO");
                            }

                            else if (playerInput2 == "2")
                            {
                                // Il giocatore prova a risolvere il puzzle
                                await dialogue.PlayerTryToSolveKeeper();
                                testo = true;
                                testo2 = true;
                            }
                        }
                    }
                }

                // Loop per leggere la risposta del giocatore
                while (true)
                {
                    string? playerInput = ReadLine();
                    // Controlla che l'input sia un numero valido tra 1 e 6
                    if (!int.TryParse(playerInput, out int inputNumber) || inputNumber > 6 || inputNumber <= 0)
                    {
                        await player.PlayerInputWarn();
                        await WriteSlowly("\nSYSTEM: Scegli un'opzione: ", 40, false);
                        continue; // Chiede di nuovo l'input
                    }

                    if (playerInput == "4")
                    {
                        // Risposta corretta
                        await dialogue.RightAnswerKeeper();
                        pergamenaMagica.CanBePickedUp = true; // Permette di raccogliere la pergamena magica
                        Logger.Log("Pergamena puo' essere raccolta", "INFO");
                        cont = true; // Puzzle risolto
                        break;
                    }
                    else if (playerInput == "6")
                    {
                        // Il giocatore esce dal puzzle
                        await player.PlayerGoesAwayFrom(player.CurrentRoom.GetInteractable("Statua"));
                        asc = true;  // Segna che ha ascoltato l'indovinello
                        testo = false;
                        testo2 = false;
                        break;
                    }
                    else
                    {
                        // Risposta sbagliata, mostra messaggio di errore
                        await player.TakeDamage(10); // Il giocatore perde 10 HP
                        if (player.Health <= 0) break;
                        await dialogue.WrongAnswerKeeper();
                    }
                }
            }
            else
            {
                // Se il puzzle è già stato risolto, la statua non può essere più interagita
                await WriteSlowly("SYSTEM: La statua è dormiente, non è possibile interagirci.");
                Logger.Log("Il giocatore ha già risolto l'enigma", "INFO");
            }
        }
    }
}
