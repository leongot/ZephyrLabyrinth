using System.Threading.Tasks;

namespace ZephyrLabyrinth
{
    /// <summary> Gestisce il combattimento tra il giocatore e Zephyr.</summary>
    public class ZephyrBattle
    {
        /// <summary> Tipi di azioni di combattimento possibili per i partecipanti.</summary>
        public enum CombatAction
        {
            
            ///<summary>Azione di attacco.</summary>
            Attacca,
            ///<summary>Azione di parata.</summary>    
            Parata,
            ///<summary>Azione di default.</summary>
            Default
        }
      
        /// <summary> Avvia il combattimento tra il giocatore e Zephyr, gestendo il ciclo di turno fino alla fine della battaglia.</summary>
        /// <remarks>
        /// Il metodo mostra un messaggio di inizio combattimento, quindi esegue un ciclo while che continua finché entrambi i combattenti hanno salute maggiore di zero.
        /// Durante ogni iterazione, viene richiesto al giocatore e a Zephyr di scegliere un'azione (Attacca o Parata).
        /// In base alle azioni scelte, viene chiamato il metodo corrispondente per risolvere lo scontro (AttackAttack, AttackDefence, DefenceAttack, DefenceDefence).
        /// Infine, viene verificato se il combattimento è terminato con CheckBattleEnd; in tal caso il ciclo si interrompe.
        /// </remarks>
        public async Task StartBattle()
        {
            await WriteSlowly("\nSYSTEM: Il combattimento con Zephyr inizia!");
            Logger.Log("Il giocatore ha iniziato il combattimento con zephyr", "INFO");

            // Loop di combattimento finché entrambi sono vivi
            while (player.Health > 0 && zephyr.Health > 0)
            {
                // Chiede all'utente e a Zephyr quale azione scegliere
                var (playerAction, zephyrAction) = await dialogue.BattleOption();

                // Risoluzione degli scontri in base alle azioni scelte
                if (playerAction == CombatAction.Attacca && zephyrAction == CombatAction.Attacca)
                {
                    await AttackAttack(playerAction, zephyrAction);
                }
                else if (playerAction == CombatAction.Attacca && zephyrAction == CombatAction.Parata)
                {
                    await AttackDefence(playerAction, zephyrAction);
                }
                else if (playerAction == CombatAction.Parata && zephyrAction == CombatAction.Attacca)
                {
                    await DefenceAttack(playerAction, zephyrAction);
                }
                else // entrambi parata
                {
                    await DefenceDefence(playerAction, zephyrAction);
                }

                // Controlla se il combattimento è terminato
                if (await CheckBattleEnd()) break;
            }
        }

        /// <summary>
        /// Gestisce il turno in cui sia il giocatore sia Zephyr scelgono di attaccare,
        /// calcolando i danni inflitti da entrambi e aggiornando la salute.
        /// </summary>
        /// <param name="playerAction">Azione del giocatore (deve essere Attacca).</param>
        /// <param name="zephyrAction">Azione di Zephyr (deve essere Attacca).</param>
        /// <remarks>
        /// Il metodo esegue il log delle azioni, calcola il danno inflitto da ciascuno tenendo conto dell'attacco e della difesa,
        /// applica i danni ad entrambi, mostra il danno inflitto da entrambi e aggiorna lo stato della salute.
        /// </remarks>
        public async Task AttackAttack(CombatAction playerAction, CombatAction zephyrAction)
        {
            await LogActions(playerAction, zephyrAction);

            int playerDamage = CalculateDamage(player.AttackPower, zephyr.Defence);
            int zephyrDamage = CalculateDamage(zephyr.AttackPower, player.Defence);

            ApplyBothDamage(playerDamage, zephyrDamage);

            await WriteSlowly($"\nSYSTEM: Hai inflitto {playerDamage} danni a Zephyr.");
            await WriteSlowly($"SYSTEM: Zephyr ti infligge {zephyrDamage} danni.");
            await ShowHealthStatus();
        }

        /// <summary>
        /// Gestisce il turno in cui il giocatore attacca e Zephyr si difende,
        /// infliggendo danni ridotti a Zephyr e aggiornandone la salute.
        /// </summary>
        /// <param name="playerAction">Azione del giocatore (Attacca).</param>
        /// <param name="zephyrAction">Azione di Zephyr (Parata).</param>
        /// <remarks>
        /// Il metodo esegue il log delle azioni, calcola il danno ridotto (metà attacco del giocatore) tenendo conto della difesa di Zephyr,
        /// applica il danno a Zephyr, assicura che la salute resti entro i limiti validi,
        /// registra i danni inflitti e la salute residua di Zephyr, e mostra i messaggi informativi.
        /// </remarks>
        public async Task AttackDefence(CombatAction playerAction, CombatAction zephyrAction)
        {
            await LogActions(playerAction, zephyrAction);

            int playerDamage = CalculateDamage(player.AttackPower / 2, zephyr.Defence);
            zephyr.Health -= playerDamage;
            zephyr.Health = ClampHealth(zephyr.Health);

            Logger.Log($"{player.Name} ha inflitto: {playerDamage} danni a Zephyr", "INFO");
            Logger.Log($"Salute di Zephyr: {zephyr.Health}", "INFO");
            await Task.Delay(1000);
            await WriteSlowly($"\nSYSTEM: Zephyr si difende! Infliggi solo {playerDamage} danni.");
            await Task.Delay(1000);
            await WriteSlowly($"\nSYSTEM: Salute di Zephyr: {zephyr.Health}");
        }

        /// <summary>
        /// Gestisce il turno in cui il giocatore si difende e Zephyr attacca,
        /// infliggendo danni ridotti al giocatore e aggiornandone la salute.
        /// </summary>
        /// <param name="playerAction">Azione del giocatore (Parata).</param>
        /// <param name="zephyrAction">Azione di Zephyr (Attacca).</param>
        /// <remarks>
        /// Il metodo esegue il log delle azioni, calcola il danno ridotto (metà attacco di Zephyr) e sottrae la difesa del giocatore,
        /// applica il danno al giocatore, assicura che la salute resti entro i limiti validi,
        /// registra i danni inflitti e la salute residua del giocatore, e mostra i messaggi informativi.
        /// </remarks>
        public async Task DefenceAttack(CombatAction playerAction, CombatAction zephyrAction)
        {
            await LogActions(playerAction, zephyrAction);

            int zephyrDamage = CalculateDamage(zephyr.AttackPower / 2, player.Defence);
            player.Health -= zephyrDamage;
            player.Health = ClampHealth(player.Health);

            Logger.Log($"Zephyr ha inflitto: {zephyrDamage} danni a {player.Name}", "INFO");
            Logger.Log($"Salute di {player.Name}: {player.Health}", "INFO");
            await Task.Delay(1000);
            await WriteSlowly($"\nSYSTEM: Ti difendi! Zephyr ti infligge solo {zephyrDamage} danni.");
            await Task.Delay(1000);
            await WriteSlowly($"SYSTEM: Salute di {player.Name}: {player.Health}");
        }

        /// <summary>Gestisce il turno in cui sia il giocatore che Zephyr scelgono di difendersi.</summary>
        /// <param name="playerAction">Azione del giocatore (Parata).</param>
        /// <param name="zephyrAction">Azione di Zephyr (Parata).</param>
        /// <remarks>
        /// <para>Il metodo registra le azioni scelte da entrambi i combattenti tramite il log.</para>
        /// <para>Poiché entrambi si difendono, non viene inflitto alcun danno.</para>
        /// <para>Viene visualizzato un messaggio informativo e registrato un log dell'evento.</para>
        /// </remarks>
        public async Task DefenceDefence(CombatAction playerAction, CombatAction zephyrAction)
        {
            await LogActions(playerAction, zephyrAction);
            await Task.Delay(1000);
            await WriteSlowly("\nSYSTEM: Entrambi vi difendete, nessun danno inflitto.");
            Logger.Log("Entrambi i combattenti si sono difesi, nessun danno inflitto", "INFO");
        }

        /// <summary>Visualizza e registra nel log le azioni scelte dal giocatore e da Zephyr durante un turno di combattimento.</summary>
        /// <param name="playerAction">L'azione selezionata dal giocatore.</param>
        /// <param name="zephyrAction">L'azione selezionata da Zephyr.</param>
        /// <remarks>
        /// <para>Le azioni vengono mostrate a schermo tramite <c>WriteSlowly</c>.</para>
        /// <para>Il metodo aiuta a fornire trasparenza sulle decisioni di ciascun partecipante al combattimento.</para>
        /// </remarks>
        private async Task LogActions(CombatAction playerAction, CombatAction zephyrAction)
        {
            await WriteSlowly($"\nSYSTEM: {player.Name} sceglie {playerAction}!");
            Logger.Log($"{player.Name} sceglie {playerAction}!", "INFO");
            await Task.Delay(1000);
            await WriteSlowly($"SYSTEM: Zephyr sceglie {zephyrAction}!");
            Logger.Log($"Zephyr sceglie {zephyrAction}!", "INFO");

        }

        /// <summary>Calcola il danno inflitto in base all'attacco e alla difesa.</summary>
        /// <param name="attack">Il valore dell'attacco dell'aggressore.</param>
        /// <param name="defence">Il valore della difesa del difensore.</param>
        /// <returns>Il danno effettivo inflitto, che non può essere inferiore a zero.</returns>
        /// <remarks>
        /// <para>Se la difesa è maggiore o uguale all'attacco, il danno restituito sarà zero.</para>
        /// <para>Utilizza <c>Math.Max</c> per garantire che il danno non sia negativo.</para>
        /// </remarks>
        private int CalculateDamage(int attack, int defence)
        {
            return Math.Max(attack - defence, 0);
        }

        /// <summary>Applica il danno calcolato sia al giocatore che a Zephyr.</summary>
        /// <param name="playerDamage">Il danno inflitto dal giocatore a Zephyr.</param>
        /// <param name="zephyrDamage">Il danno inflitto da Zephyr al giocatore.</param>
        /// <remarks>
        /// <para>Entrambi i valori di salute vengono aggiornati sottraendo il rispettivo danno subito.</para>
        /// <para>La salute viene limitata ai valori validi tramite <c>ClampHealth</c>.</para>
        /// <para>I nuovi valori di salute vengono registrati nel log per il debug.</para>
        /// </remarks>
        private void ApplyBothDamage(int playerDamage, int zephyrDamage)
        {
            zephyr.Health -= playerDamage;
            player.Health -= zephyrDamage;

            zephyr.Health = ClampHealth(zephyr.Health);
            player.Health = ClampHealth(player.Health);

            Logger.Log($"Salute di Zephyr: {zephyr.Health}", "INFO");
            Logger.Log($"Salute di {player.Name}: {player.Health}", "INFO");
        }

        /// <summary>Limita il valore della salute a un minimo di zero.</summary>
        /// <param name="health">Il valore corrente della salute da controllare.</param>
        /// <returns>Il valore della salute garantito essere maggiore o uguale a zero.</returns>
        /// <remarks>
        /// <para>Evita che la salute scenda sotto zero durante il combattimento.</para>
        /// </remarks>
        private int ClampHealth(int health)
        {
            return Math.Max(health, 0);
        }

        /// <summary>Mostra lo stato attuale della salute del giocatore e di Zephyr.</summary>
        /// <returns>Un'attività asincrona che stampa le informazioni sulla salute.</returns>
        /// <remarks>
        /// <para>Visualizza la salute residua dei due partecipanti al combattimento.</para>
        /// <para>Utile per tenere traccia dell'andamento dello scontro.</para>
        /// </remarks>
        private async Task ShowHealthStatus()
        {
            await Task.Delay(1000);
            await WriteSlowly($"\nSYSTEM: Salute di {player.Name}: {player.Health}");
            await WriteSlowly($"SYSTEM: Salute di Zephyr: {zephyr.Health}");
        }

        /// <summary>Restituisce un'azione casuale tra attacco e parata.</summary>
        /// <returns>Un valore casuale di tipo <see cref="CombatAction"/> corrispondente ad <c>Attacca</c> o <c>Parata</c>.</returns>
        /// <remarks>
        /// <para>Utilizzato per determinare casualmente l'azione di un personaggio controllato dal computer.</para>
        /// <para>Esclude l'opzione <c>Default</c> e considera solo le prime due azioni dell'enumerazione.</para>
        /// </remarks>
        public static CombatAction ZephyrRandomAction()
        {
            int actionChoice = random.Next(0, 2); // 0 o 1 (2 escluso)
            return (CombatAction)actionChoice;
        }

        /// <summary>Controlla se il combattimento è terminato verificando la salute di Zephyr e del giocatore.</summary>
        /// <returns>Un valore booleano che indica se il combattimento è terminato (true) o deve continuare (false).</returns>
        private async Task<bool> CheckBattleEnd()
        {
            if (zephyr.Health <= 0)
            {
                await dialogue.ZephyrVictoryDialogue(); 
                return true;
            }
            if (player.Health <= 0)
            {
                await dialogue.ZephyrDefeatDialogue();
                return true;
            }
            return false;
        }
    }
}
