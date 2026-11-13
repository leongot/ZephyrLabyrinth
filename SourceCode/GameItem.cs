
///<summary> Rappresenta un oggetto di gioco generico.</summary>
public class GameItem
{
    ///<summary> Nome dell'oggetto. </summary>
    public string Name { get; }

    /// <summary> Indica se l'oggetto può essere raccolto.</summary>
    public bool CanBePickedUp { get; set; }

    /// <summary> Peso dell'oggetto.</summary>
    public int Weight { get; }

    ///<summary>Descrizione dell'oggetto.</summary>
    public string Description { get; set; }

    /// <summary> Statistiche null per GameItem per</summary>
    public int? AttackPower { get; set; }
    public int? Defence { get; set; }
    public int? Health { get; set; }

    public static GameItem? mappa;

    /// <summary>
    /// Determina se l'oggetto è considerato un equipaggiamento.
    /// Un oggetto è considerato un equipaggiamento se ha valori definiti per AttackPower, Defence o Health.
    /// </summary> 
    public virtual bool IsEquipment => AttackPower.HasValue || Defence.HasValue || Health.HasValue;

    ///<summary> Indica se l'oggetto e' un Game Item o un Equipment. /// </summary>
    public string Type { get; set; }

    ///<summary> Costruttore per creare un oggetto di gioco. /// </summary>
    /// <param name="name">Nome dell'oggetto.</param>
    /// <param name="canBePickedUp">Se l'oggetto può essere raccolto.</param>
    /// <param name="weight">Peso dell'oggetto.</param>
    /// <param name="description">Descrizione dell'oggetto.</param>
    public GameItem(string name, bool canBePickedUp, int weight, string description)
    {
        Name = name;
        CanBePickedUp = canBePickedUp;
        Weight = weight;
        Description = description;
        Type = "Game Item";
    }

    /// <summary> Ritorna la rappresentazione testuale dell'oggetto (il nome).</summary>
    /// <returns> Nome dell'oggetto.</returns>
    public override string ToString()
    {
        return Name;
    }
}

/// <summary> Rappresenta un equipaggiamento, eredita da GameItem, con proprietà di attacco, difesa e salute non null.</summary>
public class Equipment : GameItem
{
    /// <summary> Riferimenti statici agli equipaggiamenti principali del gioco. /// </summary>
    public static Equipment? specchio;
    public static Equipment? palantir;
    public static Equipment? teschioCristallo;
    public static Equipment? candela;
    public static Equipment? frammentiOssidiana;
    public static Equipment? anelloArgento;
    public static Equipment? necronomicon;
    public static Equipment? mela;
    public static Equipment? pietraFilosofale;
    public static Equipment? fiammaPrometeo;
    public static Equipment? behelit;
    public static Equipment? santoGraal;
    public static Equipment? bastone;
    public static Equipment? torcia;
    public static Equipment? osso;

    ///<summary> Determina se l'oggetto è considerato un equipaggiamento.</summary>
    public override bool IsEquipment => true;

    /// <summary> Valore di attacco dell'equipaggiamento. </summary>
    public int AttackPower { get; set; }

    /// <summary> Valore di difesa dell'equipaggiamento.</summary>
    public int Defence { get; set; }

    /// <summary>Valore di salute aggiuntiva fornita dall'equipaggiamento.</summary>
    public int Health { get; set; }

    /// <summary> Costruttore per creare un equipaggiamento.</summary>
    /// <param name="name">Nome dell'equipaggiamento.</param>
    /// <param name="canBePickedUp">Se può essere raccolto.</param>
    /// <param name="weight">Peso dell'equipaggiamento.</param>
    /// <param name="attackPower">Valore di attacco.</param>
    /// <param name="defence">Valore di difesa.</param>
    /// <param name="health">Valore di salute.</param>
    /// <param name="description">Descrizione dell'equipaggiamento.</param>
    public Equipment(string name, bool canBePickedUp, int weight, int attackPower, int defence, int health, string description)
        : base(name, canBePickedUp, weight, description)
    {
        AttackPower = attackPower;
        Defence = defence;
        Health = health;
        Type = "Equipment";
    }

    /// <summary>Ritorna la rappresentazione testuale dell'equipaggiamento, inclusi attacco e difesa. </summary>
    /// <returns> Stringa con statistiche dell'equipaggiamento.</returns>
    public override string ToString()
    {
        return $"{Name} (Attack: {AttackPower}, Defence: {Defence}, HP: {Health})";
    }
}