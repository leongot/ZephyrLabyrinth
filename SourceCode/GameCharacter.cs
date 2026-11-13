namespace ZephyrLabyrinth
{
    
    ///<summary> Rappresenta un personaggio generico nel gioco.</summary
    public class GameCharacter
    {
        ///<summary>Nome del personaggio.</summary>
        public string Name { get; }

        /// <summary>Descrizione del personaggio.</summary>
        public string Description { get; }

        /// <summary>Indica se il personaggio è amichevole o meno.</summary>
        public bool IsHelpful { get; }

        
        /// <summary> Costruttore per inizializzare un personaggio.</summary>
        /// <param name="name">Nome del personaggio.</param>
        /// <param name="description">Descrizione del personaggio.</param>
        /// <param name="isHelpful">Se il personaggio è amichevole.</param>
        /// <returns> Un'istanza di GameCharacter </returns>
        public GameCharacter(string name, string description, bool isHelpful)
        {
            Name = name;
            Description = description;
            IsHelpful = isHelpful;
        }

        
        /// <summary> Ritorna il nome del personaggio come stringa./// </summary>
        /// <returns> Il nome del personaggio come stringa.</returns>
        public override string ToString()
        {
            return Name;
        }
    }
    
    /// <summary>
    /// Rappresenta Zephyr, un personaggio specifico con attributi di combattimento, eredita da GameCharacter.
    /// </summary>
    public class Zephyr : GameCharacter
    {
        /// <summary>Salute attuale di Zephyr.</summary>
        public int Health { get; set; }

        /// <summary>Potenza d'attacco di Zephyr.</summary>
        public int AttackPower { get; set; }

        /// <summary>Valore di difesa di Zephyr.</summary>
        public int Defence { get; set; }

        /// <summary> Costruttore di Zephyr con valori predefiniti.</summary>
        /// <returns> Un'istanza di Zephyr </returns>
        public Zephyr() : base("Zephyr", "Lo stregone predone dei mondi.", false)
        {
            Health = 110;
            AttackPower = 20;
            Defence = 11;
        }
    }
}