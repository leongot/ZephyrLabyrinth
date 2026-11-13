namespace ZephyrLabyrinth
{
    /// <summary> Gestisce l'inventario del giocatore utilizzando uno stack di GameItem.</summary>
    public class Inventory
    {
        /// <summary> Stack che contiene gli oggetti dell'inventario.</summary>
        public Stack<GameItem> inventory = new Stack<GameItem>();

        bool addedEquipment = false;
        bool removedEquipment = false;

        /// <summary> Numero di oggetti presenti nell'inventario.</summary>
        public int Count => inventory.Count;

        /// <summary> Restituisce l'ultimo oggetto aggiunto senza rimuoverlo.</summary>
        public GameItem Peek() => inventory.Peek();

        /// <summary> Rimuove e restituisce l'ultimo oggetto aggiunto.</summary>
        public GameItem Pop() => inventory.Pop();

        /// <summary> Aggiunge un oggetto all'inventario.</summary>
        public void Push(GameItem item) => inventory.Push(item);

        /// <summary> Permette di prendere un oggetto dalla stanza corrente, aggiungendolo all'inventario se possibile.</summary>
        /// <param name="itemName"> Nome dell'oggetto da prendere.</param>
        /// <returns> Task che rappresenta l'operazione asincrona di presa dell'oggetto.</returns>
        public async Task TakeItem(string itemName)
        {
            addedEquipment = true;
            GameItem item = player.CurrentRoom.GetItem(itemName);
            Equipment? equipment = item as Equipment;
            bool existsInInventory = inventory.Any(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));

            // Controlla se l'oggetto esiste in qualche stanza diversa da quella corrente
            bool existsInOtherRoom = false;
            if (item == null)
            {
                // Qui suppongo tu abbia una lista di tutte le stanze chiamata ad esempio "allRooms"
                // Se non ce l'hai, devi adattare la cosa in base alla tua struttura dati
                existsInOtherRoom = rooms.Where(kvp => kvp.Value != player.CurrentRoom).Any(kvp => kvp.Value.GetItem(itemName) != null);
            }

            if (item != null && item.CanBePickedUp)
            {
                if (player.currentWeight + item.Weight <= player.MaxCarryWeight)
                {
                    if (equipment != null)
                    {
                        inventory.Push(equipment);
                    }
                    else
                    {
                        inventory.Push(item);
                    }

                    player.currentWeight += item.Weight;
                    await WriteSlowly($"\nSYSTEM: Hai raccolto {item.Name}.");
                    Logger.Log($"Il giocatore ha raccolto {item.Name}.", "INFO");
                    await StatsModifier(item);
                    player.CurrentRoom.RemoveItem(item);
                }
                else
                {
                    await WriteSlowly("\nSYSTEM: Hai raggiunto il peso massimo trasportabile!");
                    Logger.Log("Il giocatore ha tentato di raccogliere un oggetto nonostante inventario pieno", "WARN");
                }
            }
            else if (existsInInventory)
            {
                await WriteSlowly("\nSYSTEM: L'oggetto e' gia' presente nel tuo inventario.");
                Logger.Log($"Il giocatore ha tentato di raccogliere un oggetto gia' presente nel suo inventario: {itemName}.", "WARN");
                return;
            }
            else if (existsInOtherRoom)
            {
                await WriteSlowly("\nSYSTEM: L'oggetto specificato non è presente in questa stanza.");
                Logger.Log($"Il giocatore ha tentato di raccogliere {itemName} che esiste in un'altra stanza.", "WARN");
            }
            else if (item == null)
            {
                await WriteSlowly("\nSYSTEM: Non esiste un oggetto con quel nome.");
                Logger.Log("Il giocatore ha tentato di raccogliere un oggetto inesistente", "WARN");
            }
            else
            {
                await WriteSlowly($"\nSYSTEM: {item.Name} non può essere raccolto.");
                Logger.Log("Il giocatore ha tentato di raccogliere un oggetto non raccoglibile", "WARN");
            }
            addedEquipment = false;
        }

        /// <summary> Permette di lasciare l'ultimo oggetto raccolto nell'inventario nella stanza corrente.</summary>
        /// <param name="itemName"> Nome dell'oggetto da lasciare.</param>
        /// <returns> Task che rappresenta l'operazione asincrona di rilascio dell'oggetto.</returns>
        public async Task DropItem(string itemName)
        {
            if (string.IsNullOrWhiteSpace(itemName))
            {
                await WriteSlowly("\nSYSTEM: Non hai specificato alcun nome.");
                Logger.Log("Il giocatore ha tentato di lasciare un oggetto senza specificarne il nome.", "WARN");
                return;
            }

            if (inventory.Count == 0)
            {
                await WriteSlowly("\nSYSTEM: Non hai oggetti da lasciare.");
                Logger.Log("Il giocatore ha tentato di lasciare un oggetto ma l'inventario è vuoto.", "WARN");
                return;
            }

            // Controlla se l'oggetto esiste nell'inventario
            bool exists = inventory.Any(item => item.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));

            if (!exists)
            {
                await WriteSlowly("\nSYSTEM: Non esiste un oggetto con quel nome nel tuo inventario.");
                Logger.Log($"Il giocatore ha tentato di lasciare un oggetto inesistente: {itemName}.", "WARN");
                return;
            }

            // Ora controlla se è l'ultimo oggetto raccolto (in cima allo stack)
            GameItem topItem = inventory.Peek();
            if (topItem.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase))
            {
                removedEquipment = true;
                GameItem item = inventory.Pop();

                await WriteSlowly($"\nSYSTEM: Hai lasciato {item.Name}.");
                await WriteSlowly($"SYSTEM: Il suo peso: {item.Weight} è stato rimosso dal tuo inventario.");
                await StatsModifier(item);

                player.currentWeight -= item.Weight;
                await WriteSlowly($"\nSYSTEM: Stai trasportando il seguente peso: {player?.currentWeight}, puoi trasportare fino a {player?.MaxCarryWeight}.");

                item.CanBePickedUp = true;
                player.CurrentRoom.AddItem(item);

                Logger.Log($"Il giocatore ha lasciato {item.Name}.", "INFO");
            }
            else
            {
                await WriteSlowly("\nSYSTEM: Puoi lasciare solo l'ultimo oggetto che hai raccolto.");
                Logger.Log("Il giocatore ha tentato di lasciare un oggetto che non è in cima allo stack.", "WARN");
            }

            removedEquipment = false;
        }

        /// <summary> Mostra all'utente il contenuto dell'inventario con nome e peso degli oggetti.</summary>
        /// <returns> Task che rappresenta l'operazione asincrona di visualizzazione dell'inventario.</returns>
        public async Task ShowInventory()
        {
            if (inventory.Count == 0)
            {
                await WriteSlowly("\nSYSTEM: Il tuo inventario è vuoto.");
                return;
            }
            await WriteSlowly("\nSYSTEM: Il tuo inventario contiene:");
            foreach (var item in inventory)
            {
                Logger.Log($"\nDEBUG: {item.Name} è di tipo {item.GetType().Name}", "INFO");
                await WriteSlowly($"\n        - {item.Name}: {item.Description}");
                await WriteSlowly($"          Peso: {item.Weight}");
                if (item.IsEquipment)
                {
                    Equipment? equipment = item as Equipment;
                    if (equipment != null)
                    {
                        await WriteSlowly($"          Stats: {equipment.AttackPower} ATK | {equipment.Defence} DEF | {equipment.Health} HP");
                    }
                }
            }

            Logger.Log("Il giocatore ha richiesto di visualizzare il suo completo inventario.", "INFO");
            await WriteSlowly($"\nSYSTEM: Stai trasportando il seguente peso: {player?.currentWeight}, puoi trasportare fino a {player?.MaxCarryWeight}.");
        }

        /// <summary> Verifica se l'inventario contiene un oggetto con il nome specificato.</summary>
        /// <param name="itemName"> Nome dell'oggetto da cercare.</param>
        /// <returns> True se l'oggetto è presente, altrimenti false.</returns>
        public bool HasItem(string itemName)
        {
            return inventory.Any(item => item.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary> Modifica le statistiche del giocatore in base all'oggetto aggiunto o rimosso.</summary>
        /// <param name="item"> Oggetto che modifica le statistiche.</param>
        /// <returns> Task che rappresenta l'operazione asincrona di modifica delle statistiche.</returns>
        public async Task StatsModifier(GameItem item)
        {
            if (addedEquipment == true)
            {
                if (item is Equipment equipment)
                {
                    player.AttackPower += equipment.AttackPower;
                    player.Defence += equipment.Defence;
                    player.Health += equipment.Health;
                    await WriteSlowly($"\nSYSTEM: Le tue statistiche sono state incrementate dei seguenti valori: {equipment.AttackPower} ATK | {equipment.Defence} DEF | {equipment.Health} HP.");
                    Logger.Log($"Statistiche incrementate dei seguenti valori {equipment.AttackPower} ATK, {equipment.Defence} DEF, {equipment.Health} HP.", "INFO");
                    await WriteSlowly($"SYSTEM: Le tue statistiche correnti: {player.AttackPower} ATK | {player.Defence} DEF | {player.Health} HP.");
                    Logger.Log($"Statistiche correnti del giocatore {player.AttackPower} ATK, {player.Defence} DEF, {player.Health} HP.", "INFO");
                }
            }

            if (removedEquipment == true)
            {
                if (item is Equipment equipment)
                {
                    player.AttackPower -= equipment.AttackPower;
                    player.Defence -= equipment.Defence;
                    player.Health -= equipment.Health;
                    await WriteSlowly($"\nSYSTEM: Le tue statistiche sono state decrementate dei seguenti valori: {equipment.AttackPower} ATK | {equipment.Defence} DEF | {equipment.Health} HP.");
                    Logger.Log($"Statistiche decrementate dei seguenti valori {equipment.AttackPower} ATK, {equipment.Defence} DEF, {equipment.Health} HP.", "INFO");
                    await WriteSlowly($"SYSTEM: Le tue statistiche correnti: {player.AttackPower} ATK | {player.Defence} DEF | {player.Health} HP.");
                    Logger.Log($"Statistiche correnti del giocatore {player.AttackPower} ATK, {player.Defence} DEF, {player.Health} HP.", "INFO");
                }
            }
        }
        
        /// <summary>
        /// Cerca un oggetto ovunque nel gioco, prima nell'inventario del giocatore, poi in tutte le stanze.
        /// </summary>
        /// <param name="itemName">Nome dell'oggetto da cercare (case-insensitive).</param>
        /// <returns>Ritorna il GameItem se trovato, altrimenti null.</returns>
        public static GameItem GetItemAnywhere(string itemName)
        {
            // Cerca nell'inventario
            foreach (var item in player.Inventory.inventory)
            {
                if (item.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase))
                    return item;
            }

            // Cerca in tutte le stanze: usa solo GetItem()
            foreach (var room in rooms.Values)
            {
                var item = room.GetItem(itemName);
                if (item != null)
                    return item;
            }

            return null; // Oggetto non trovato
        }
    }
}
