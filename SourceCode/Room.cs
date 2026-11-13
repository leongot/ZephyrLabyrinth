using System.Threading.Tasks;

namespace ZephyrLabyrinth
{
    /// <summary> Rappresenta una stanza nel gioco con uscite, oggetti, personaggi e oggetti interagibili.</summary>
    public class Room
    {
        /// <summary>Nome della stanza.</summary>
        public string Name { get; }
        /// <summary>Descrizione della stanza.</summary>
        public string Description { get; }
        /// <summary>Contiene le uscite della stanza, mappate da nome a oggetto Room.</summary>
        public Dictionary<string, Room> exits;
        /// <summary>Lista degli oggetti presenti nella stanza.</summary>
        public List<GameItem> items;
        /// <summary>Ritorna una lista di oggettidi sola lettura presenti nella stanza.</summary>
        /// <remarks>Usa AsReadOnly per evitare modifiche esterne alla lista.</remarks>
        /// <returns>Lista di oggetti presenti nella stanza.</returns>
        public IReadOnlyList<GameItem> Items => items.AsReadOnly();
        /// <summary>Lista dei personaggi presenti nella stanza.</summary>
        public List<GameCharacter> characters;
        /// <summary>Lista degli oggetti interagibili nella stanza.</summary>
        public List<Interactable> interactables;
        /// <summary>Indica se la stanza è stata visitata dal giocatore.</summary>
        public bool hasBeenVisited;

        /// <summary> Costruttore della classe Room.</summary>
        /// <param name="name">Nome della stanza.</param>
        /// <param name="description">Descrizione della stanza.</param>
        /// <remarks>
        /// <para>Inizializza le proprietà Name e Description con i valori passati.</para>
        /// <para>Crea un dizionario vuoto per le uscite (exits).</para>
        /// <para>Inizializza le liste di oggetti (items), personaggi (characters) e interagibili (interactables) come vuote.</para>
        /// <para>Imposta hasBeenVisited a false, indicando che la stanza non è stata ancora visitata.</para>
        /// </remarks>
        public Room(string name, string description)
        {
            Name = name;
            Description = description;
            exits = new Dictionary<string, Room>();
            items = new List<GameItem>();
            characters = new List<GameCharacter>();
            interactables = new List<Interactable>();
            hasBeenVisited = false;
        }

        /// <summary>
        /// Collega la stanza corrente a un'altra stanza in una direzione specifica.
        /// </summary>
        /// <param name="direction">La direzione in cui collegare la stanza ("nord", "sud", "est", "ovest").</param>
        /// <param name="room">La stanza da collegare.</param>
        /// <remarks>
        /// Aggiunge o aggiorna l'uscita nella direzione specificata nel dizionario exits, collegandola alla stanza passata come parametro.
        /// </remarks>
        public void ConnectRoom(string direction, Room room)
        {
            exits[direction] = room;
        }

        /// <summary> Verifica se esiste un'uscita nella direzione specificata.</summary>
        /// <param name="direction">La direzione da controllare (es. "nord", "est").</param>
        /// <returns>Restituisce true se esiste un'uscita nella direzione indicata, altrimenti false.</returns>
        /// <remarks>
        /// Controlla la presenza della chiave 'direction' nel dizionario exits, indicando se è possibile uscire in quella direzione dalla stanza corrente.
        /// </remarks>
        public bool HasExit(string direction)
        {
            return exits.ContainsKey(direction);
        }

        /// <summary> Restituisce la stanza collegata nell'uscita specificata.</summary>
        /// <param name="direction">La direzione dell'uscita da cui ottenere la stanza (es. "nord", "est").</param>
        /// <returns>La stanza collegata all'uscita nella direzione indicata.</returns>
        /// <remarks>
        /// Accede al dizionario exits con la chiave 'direction' e ritorna la stanza corrispondente.
        /// Se la direzione non esiste, potrebbe causare un'eccezione.
        /// </remarks>
        public Room GetExit(string direction)
        {
            return exits[direction];
        }

        /// <summary> Aggiunge un oggetto alla lista degli oggetti presenti nella stanza.</summary> 
        /// <param name="item">L'oggetto da aggiungere alla stanza.</param>
        /// <remarks>
        /// Aggiunge l'elemento 'item' alla lista 'items'.
        /// Registra un messaggio di log con il nome dell'oggetto aggiunto.
        /// </remarks>
        public void AddItem(GameItem item)
        {
            items.Add(item);
            Logger.Log($"Aggiunto l'oggetto '{item.Name}' alla stanza", "INFO");
        }

        /// <summary> Rimuove un oggetto dalla lista degli oggetti presenti nella stanza.</summary>
        /// <param name="item">L'oggetto da rimuovere dalla stanza.</param>
        /// <returns>Task asincrono che completa la rimozione e la notifica all'utente.</returns>
        /// <remarks>
        /// <para>- Cerca nella lista 'items' un oggetto con nome uguale a quello specificato (case insensitive).</para>
        /// <para>- Se l'oggetto viene trovato, lo rimuove dalla lista.</para>
        /// <para>- Se il giocatore non ha l'oggetto in inventario, mostra un messaggio di sistema.</para>
        /// <para>- Registra un messaggio di log di tipo INFO per la rimozione.</para>
        /// <para>- Se l'oggetto non viene trovato nella stanza, mostra un messaggio di sistema di errore.</para>
        /// <para>- Registra un messaggio di log di tipo WARN in caso di mancato ritrovamento.</para>
        /// </remarks>
        public async Task RemoveItem(GameItem item)
        {
            var itemToRemove = items.FirstOrDefault(i => i.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase));
            if (itemToRemove != null)
            {
                items.Remove(itemToRemove);
                if (!player.Inventory.HasItem(itemToRemove.Name))
                {
                    await WriteSlowly($"\nSYSTEM: L'oggetto '{itemToRemove.Name}' e' stato rimosso dalla stanza.");
                }
                Logger.Log($"Rimosso l'oggetto '{itemToRemove.Name}' dalla stanza", "INFO");
            }
            else
            {
                await WriteSlowly($"\nSYSTEM: Oggetto '{item.Name}' non trovato nella stanza.");
                Logger.Log($"Oggetto '{item.Name}' non trovato nella stanza", "WARN");
            }
        }

        /// <summary> Restituisce il primo oggetto presente nella stanza il cui nome corrisponde a quello fornito, ignorando maiuscole/minuscole.</summary>
        /// <param name="itemName">Il nome dell'oggetto da cercare.</param>
        /// <returns>Il GameItem corrispondente se trovato; altrimenti, null.</returns>
        /// <remarks>
        /// <para>Utilizza una ricerca lineare nella lista degli oggetti della stanza.</para>
        /// <para>Confronta i nomi degli oggetti ignorando differenze tra maiuscole e minuscole.</para>
        /// <para>Se nessun oggetto corrisponde, restituisce null.</para>
        /// </remarks>
        public GameItem GetItem(string itemName)
        {
            return items.FirstOrDefault(item => item.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary> Aggiunge un personaggio alla lista dei personaggi presenti nella stanza.</summary>
        /// <param name="character">Il personaggio da aggiungere.</param>
        /// <remarks>
        /// <para>Il personaggio viene aggiunto direttamente alla lista interna characters.</para>
        /// <para>Viene registrata un'entrata di log per tracciare l'aggiunta del personaggio.</para>
        /// </remarks>
        public void AddCharacter(GameCharacter character)
        {
            characters.Add(character);
            Logger.Log($"Aggiunto il personaggio '{character.Name}' alla stanza", "INFO");
        }

        /// <summary> Restituisce il personaggio presente nella stanza con il nome specificato.</summary>
        /// <param name="name">Il nome del personaggio da cercare.</param>
        /// <returns>Il personaggio corrispondente al nome, oppure null se non trovato.</returns>
        /// <remarks>
        /// <para>La ricerca ignora la differenza tra maiuscole e minuscole.</para>
        /// <para>Se più personaggi hanno lo stesso nome, viene restituito il primo trovato.</para>
        /// </remarks>
        public GameCharacter GetCharacter(string name)
        {
            return characters.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary> Aggiunge un oggetto interagibile alla stanza.</summary>
        /// <param name="interactable">L'oggetto interagibile da aggiungere.</param>
        /// <remarks>
        /// <para>L'oggetto viene aggiunto alla lista interna degli interagibili della stanza.</para>
        /// <para>Viene registrata una voce di log con il nome dell'oggetto e il nome della stanza di appartenenza.</para>
        /// </remarks>
        public void AddInteractable(Interactable interactable)
        {
            interactables.Add(interactable);
            Logger.Log($"Aggiunto l'oggetto '{interactable.Name}' a {interactable.RoomName}", "INFO");
        }

        /// <summary> Restituisce un oggetto interagibile presente nella stanza tramite il suo nome.</summary>
        /// <param name="name">Il nome dell'oggetto interagibile da cercare.</param>
        /// <returns>L'oggetto interagibile corrispondente al nome, oppure null se non trovato.</returns>
        /// <remarks>
        /// <para>La ricerca è case-insensitive.</para>
        /// <para>Se non esiste un interagibile con il nome specificato, la funzione restituisce null.</para>
        /// </remarks>
        public Interactable GetInteractable(string name)
        {
            return interactables.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>Mostra la descrizione della stanza al giocatore.</summary>
        /// <param name="forceDisplay">Se true, forza la visualizzazione della descrizione anche se la stanza è già stata visitata.</param>
        /// <returns>Task asincrono.</returns>
        /// <remarks>
        /// <para>La descrizione viene mostrata solo la prima volta che la stanza viene visitata, a meno che forceDisplay sia true.</para>
        /// <para>Se la stanza non è mai stata visitata, viene registrato un log e mostrato un messaggio di scoperta.</para>
        /// </remarks>
        public async Task DisplayRoomDescription(bool forceDisplay = false)
        {
            if (!hasBeenVisited || forceDisplay)
            {
                await WriteSlowly(Description);
                hasBeenVisited = true;
            }
            if (!hasBeenVisited)
            {
                Logger.Log($"{player.Name} ha sbloccato la stanza {Name}", "INFO");
                await WriteSlowly($"\nSYSTEM: Hai scoperto la stanza '{Name}'!");
            }
        }

        /// <summary>Restituisce il nome della stanza come rappresentazione testuale.</summary>
        /// <returns>Il nome della stanza.</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
