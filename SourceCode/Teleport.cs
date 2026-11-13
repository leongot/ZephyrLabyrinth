namespace ZephyrLabyrinth
{
    /// <summary> Gestisce la logica del teletrasporto del giocatore tra le stanze del labirinto.</summary>
    class Teleport
    {
        /// <summary> Indica se il teletrasporto è attivo.</summary>
        public bool telAtt = false;

        /// <summary> Indica se il primo teletrasporto è stato già effettuato.</summary>
        public bool firstTeleportDone = false;

        /// <summary> Indica se la Stanza Segreta è stata scoperta.</summary>
        public bool discoveredSecretRoom = false;

        /// <summary> Stanza di destinazione del teletrasporto.</summary>
        Room destinationRoom;

        /// <summary> Esegue il teletrasporto del giocatore a seconda delle condizioni del gioco.</summary>
        /// <remarks>
        /// <para>- Se non attivato, stampa messaggio di errore.</para>
        /// <para>- Il primo teletrasporto porta sempre allo "Studio Alchemico".</para>
        /// <para>- I successivi teletrasporti portano casualmente in stanze già visitate, con una probabilità del 15% di entrare nella "Stanza Segreta".</para>
        /// <para>- Il teletrasporto consuma 5 HP dal giocatore.</para>
        /// </remarks>
        /// <param name="player">Il giocatore che esegue il teletrasporto.</param>
        /// <param name="rooms">Dizionario delle stanze del labirinto.</param>
        /// <param name="dialogue">Il dialogo da utilizzare per le interazioni.</param>
        /// <returns> Un Task che rappresenta l'operazione asincrona di teletrasporto.</returns>
        public async Task Teleportation()
        {
            if (!telAtt)
            {
                await WriteSlowly("\nSYSTEM: Non puoi teletrasportarti ora.");
                Logger.Log("TELETRASPORTO non attivato", "WARN");
                return;
            }
            else if (rooms == null || rooms.Count <= 1)
            {
                await WriteSlowly("\nSYSTEM: Teletrasporto fallito. Nessuna stanza disponibile.");
                Logger.Log("TELETRASPORTO fallito: lista stanze vuota o nulla", "ERROR");
                return;
            }
            else if (!firstTeleportDone)
            {
                if (!rooms.TryGetValue("Studio Alchemico", out destinationRoom))
                {
                    destinationRoom = rooms.Values.FirstOrDefault(r => r.Name.Equals("Studio Alchemico", StringComparison.OrdinalIgnoreCase));
                }
                else if (destinationRoom == null)
                {
                    await WriteSlowly("\nSYSTEM: Errore, 'Studio Alchemico' non trovato.");
                    Logger.Log("TELETRASPORTO fallito: 'Studio Alchemico' non presente tra le stanze", "ERROR");
                    return;
                }

                firstTeleportDone = true;
                player.CurrentRoom = destinationRoom;
            }
            else
            {
                Room secretRoom = rooms.Values.FirstOrDefault(r => r.Name.Equals("Stanza Segreta", StringComparison.OrdinalIgnoreCase));
                var unlockedRooms = rooms.Values.Where(r => r.hasBeenVisited).ToList();

                if (unlockedRooms.Count == 0)
                {
                    await WriteSlowly("\nSYSTEM: Nessuna stanza sbloccata disponibile per il teletrasporto.");
                    Logger.Log("TELETRASPORTO fallito: nessuna stanza sbloccata", "ERROR");
                    return;
                }

                int diceRoll = random.Next(100);
                Logger.Log($"Il giocatore ha tirato un dado e ha ottenuto: {diceRoll}", "INFO");
                bool teleportToSecret = secretRoom != null && diceRoll < 15; // 15% di probabilità di entrare nella stanza segreta
                Logger.Log($"teleportToSecret: {teleportToSecret}", "INFO");

                if (teleportToSecret)
                {
                    destinationRoom = secretRoom;
                    Logger.Log("Il giocatore è entrato nella stanza segreta", "INFO");
                    await WriteSlowly("\nNARRATORE: Un'energia misteriosa ti avvolge...");
                }
                else
                {
                    var nonSecretRooms = unlockedRooms.Where(r => r != secretRoom).ToList();
                    destinationRoom = nonSecretRooms[random.Next(nonSecretRooms.Count)];
                }

                // Sottrai 5 HP al giocatore per il teletrasporto
                player.Health = Math.Max(player.Health - 5, 0);
                await WriteSlowly($"\nSYSTEM: {player.Name} ha perso 5 HP usando il teletrasporto. HP attuali: {player.Health}");
                Logger.Log($"{player.Name} ha perso 5HP usando il teletrasporto. HP attuali: {player.Health}", "INFO");

                if (player.Health <= 0)
                {
                    await WriteSlowly($"\nSYSTEM: Il teletrasporto ha consumato completamente l'energia vitale, {player.Name} e' morto e non lasciera' mai il labirinto.");
                    Logger.Log($"{player.Name} è morto a causa del teletrasporto", "FATAL");
                    await WriteSlowly("\nSYSTEM: Premi un qualsiasi tasto per tornare al menu principale.");
                    ReadKey(true);
                    dialogue.EndBattle = true;
                    return;
                }
            }
            player.CurrentRoom = destinationRoom;
            await WriteSlowly($"\nSYSTEM: Sei stato teletrasportato in: {destinationRoom.Name}.");
            await player.CurrentRoom.DisplayRoomDescription(true); // Mostra la descrizione della stanza forzatamente
            await dialogue.CheckDialogueCondition();
            Logger.Log($"{player.Name} si è teletrasportato in {destinationRoom.Name}", "INFO");
        }

        /// <summary> Attiva il teletrasporto se il giocatore possiede una "Pergamena Magica".</summary>
        /// <remarks> Rimuove la pergamena dall'inventario del giocatore e abilita il teletrasporto.</remarks>
        /// <param name="player">Il giocatore che attiva il teletrasporto.</param>
        /// <returns> Un Task che rappresenta l'operazione asincrona di attivazione del teletrasporto.</returns>
        public async Task TeleportActivation()
        {
            if (player.Inventory.Peek().Name.Equals("Pergamena Magica", StringComparison.OrdinalIgnoreCase))
            {
                telAtt = true;
                GameItem item = player.Inventory.Pop();
                await WriteSlowly($"\nSYSTEM: Hai lasciato: {item.Name}.");
                Logger.Log($"{player.Name} ha rimosso {item.Name} dal suo inventario", "INFO");
                player.currentWeight -= item.Weight;
                Logger.Log($"{player.Name} ha attivato il TELETRASPORTO", "INFO");
            }
            else
            {
                Logger.Log("L'attivazione del TELETRASPORTO non è andata a buon fine...", "WARN");
            }
        }
    }
}
