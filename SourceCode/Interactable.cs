namespace ZephyrLabyrinth
{
    /// <summary> Rappresenta un oggetto con cui il giocatore può interagire all'interno di una stanza.</summary>
    public class Interactable
    {
        /// <summary> Nome dell'oggetto interagibile.</summary>
        public string Name { get; }

        /// <summary> Descrizione dell'oggetto interagibile.</summary>
        public string Description { get; }

        /// <summary> Nome della stanza in cui si trova l'oggetto.</summary>
        public string RoomName { get; }

        /// <summary> Costruttore che inizializza un oggetto interagibile con nome, descrizione e stanza.</summary>
        /// <param name="name">Nome dell'oggetto.</param>
        /// <param name="description">Descrizione dell'oggetto.</param>
        /// <param name="roomName">Nome della stanza contenente l'oggetto.</param>
        /// <returns> Un'istanza di Interactable.</returns>
        public Interactable(string name, string description, string roomName)
        {
            Name = name;
            Description = description;
            RoomName = roomName;
        }

        /// <summary> Restituisce il nome dell'oggetto come rappresentazione testuale.</summary>
        /// <returns> Nome dell'oggetto.</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
