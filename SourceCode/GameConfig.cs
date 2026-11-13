/// <summary>
/// Classe principale che contiene la configurazione di gioco.
/// </summary>
public class GameConfig
{
    /// <summary>
    /// Configurazione del giocatore.
    /// </summary>
    public PlayerConfig player { get; set; }

    /// <summary>
    /// Configurazione di Zephyr (il nemico o personaggio).
    /// </summary>
    public ZephyrConfig zephyr { get; set; }

    /// <summary>
    /// Configurazione degli stati dei puzzle.
    /// </summary>
    public PuzzleConfig puzzle { get; set; }
}

/// <summary>
/// Configurazione del giocatore.
/// </summary>
public class PlayerConfig
{
    /// <summary>
    /// Nome del giocatore.
    /// </summary>
    public string name { get; set; }

    /// <summary>
    /// Peso massimo che il giocatore può trasportare.
    /// </summary>
    public int maxCarryWeight { get; set; }

    /// <summary>
    /// Punti vita massimi del giocatore.
    /// </summary>
    public int health { get; set; }

    /// <summary>
    /// Potenza d'attacco del giocatore.
    /// </summary>
    public int attackPower { get; set; }

    /// <summary>
    /// Valore di difesa del giocatore.
    /// </summary>
    public int defence { get; set; }

    /// <summary>
    /// Stanza di partenza del giocatore.
    /// </summary>
    public string startRoom { get; set; }
}

/// <summary>
/// Configurazione del personaggio Zephyr.
/// </summary>
public class ZephyrConfig
{
    /// <summary>
    /// Punti vita di Zephyr.
    /// </summary>
    public int health { get; set; }

    /// <summary>
    /// Potenza d'attacco di Zephyr.
    /// </summary>
    public int attackPower { get; set; }

    /// <summary>
    /// Valore di difesa di Zephyr.
    /// </summary>
    public int defence { get; set; }
}

/// <summary>
/// Configurazione dello stato dei puzzle nel gioco.
/// </summary>
public class PuzzleConfig
{
    /// <summary>
    /// Stato del puzzle della serratura.
    /// </summary>
    public bool LockPuzzle { get; set; }

    /// <summary>
    /// Stato del puzzle del guardiano.
    /// </summary>
    public bool KeeperPuzzle { get; set; }

    /// <summary>
    /// Indica se la pergamena magica è raccoglibile.
    /// </summary>
    public bool MagicScrollPickable { get; set; }

    /// <summary>
    /// Stato del puzzle della chiave.
    /// </summary>
    public bool KeyPuzzle { get; set; }

    /// <summary>
    /// Blocca il portale o porta.
    /// </summary>
    public bool PortBloc { get; set; }

    /// <summary>
    /// Stato del puzzle della luce.
    /// </summary>
    public bool altarpuzzle { get; set; }

    /// <summary>
    /// Stato attacco del teletrasporto.
    /// </summary>
    public bool TelAtt { get; set; }

    /// <summary>
    /// Indica se il cancello è chiuso.
    /// </summary>
    public bool CancelloChiuso { get; set; }
}

/// <summary>
/// Classe statica per caricare la configurazione da file JSON.
/// </summary>
public static class ConfigLoader
{
    /// <summary>
    /// Carica la configurazione dal file JSON specificato (default "config.json").
    /// Restituisce null se il file non esiste.
    /// </summary>
    /// <param name="path">Percorso del file JSON di configurazione.</param>
    /// <returns> Un oggetto GameConfig con la configurazione caricata, oppure null se il file non esiste.</returns>
    public static GameConfig Load(string path = "config.json")
    {
        // Ottiene il percorso completo del file di configurazione.
        string fullPath = Path.GetFullPath(path);
        WriteLine($"DEBUG: Cerco il file di configurazione in {fullPath}");

        // Controlla se il file esiste.
        if (!File.Exists(path))
        {
            WriteLine("DEBUG: Il file non esiste!");
            return null;
        }

        // Legge il contenuto JSON dal file.
        string json = File.ReadAllText(path);

        // Impostazioni per la deserializzazione con case insensitive.
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // Deserializza il contenuto JSON in un oggetto GameConfig.
        return JsonSerializer.Deserialize<GameConfig>(json, options);
    }
}