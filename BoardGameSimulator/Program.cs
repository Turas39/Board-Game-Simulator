
public class Player
{
    public string Name { get; set; }
    public (int X, int Y) Position { get; set; }
    public int Score { get; private set; }

    public Player(string name)
    {
        Name = name;
        Position = (0, 0); 
        Score = 0; 
    }

    public void Move(int deltaX, int deltaY, int boardWidth, int boardHeight)
    {
        int newX = Math.Clamp(Position.X + deltaX, 0, boardWidth - 1);
        int newY = Math.Clamp(Position.Y + deltaY, 0, boardHeight - 1);
        Position = (newX, newY);
        Console.WriteLine($"{Name} moved to position ({Position.X}, {Position.Y})");
    }

    public void UpdateScore(int points)
    {
        Score += points;
        Console.WriteLine($"{Name} gained {points} points. Total score: {Score}");
    }
}

public interface IPlayer
{
    string Name { get; set; }
    (int X, int Y) Position { get; set; }
    int Score { get; set; }

    void Move(int deltaX, int deltaY, int boardWidth, int boardHeight);
    void PerformSpecialAction();
    void UpdateScore(int points);
}

public class Warrior : Player
{
    public Warrior(string name) : base(name) { }

    public override void PerformSpecialAction()
    {
        Console.WriteLine($"{Name} performs a powerful attack to gain extra points!");
        UpdateScore(10);
    }
}

public class Mage : Player
{
    public Mage(string name) : base(name) { }

    public override void PerformSpecialAction()
    {
        Console.WriteLine($"{Name} casts a spell to influence the game!");
    }
}

public class Healer : Player
{
    public Healer(string name) : base(name) { }

    public override void PerformSpecialAction()
    {
        Console.WriteLine($"{Name} heals another player!");
    }
}

public class Board
{
    public int Width { get; }
    public int Height { get; }
    private Dictionary<(int X, int Y), int> Rewards { get; set; }

    public event Action<IPlayer, int> RewardCollected;

    public Board(int width, int height)
    {
        Width = width;
        Height = height;
        Rewards = new Dictionary<(int X, int Y), int>();
    }

    public void GenerateRewards(int rewardCount, int maxPoints)
    {
        Random rand = new Random();
        for (int i = 0; i < rewardCount; i++)
        {
            int x = rand.Next(Width);
            int y = rand.Next(Height);
            int points = rand.Next(1, maxPoints + 1);
            Rewards[(x, y)] = points;
        }
        Console.WriteLine("Rewards generated on the board.");
    }

    public void CheckReward(IPlayer player)
    {
        if (Rewards.TryGetValue(player.Position, out int points))
        {
            Rewards.Remove(player.Position);
            RewardCollected?.Invoke(player, points);
        }
    }
}

public class Game
{
    private List<IPlayer> Players { get; }
    private Board GameBoard { get; }
    private int CurrentPlayerIndex { get; set; }

    public Game(int boardWidth, int boardHeight, int rewardCount, int maxPoints, List<IPlayer> players)
    {
        GameBoard = new Board(boardWidth, boardHeight);
        GameBoard.RewardCollected += OnRewardCollected;
        Players = players;
        GameBoard.GenerateRewards(rewardCount, maxPoints);
        CurrentPlayerIndex = 0;
    }

    public void StartGame(int turns)
    {
        Console.WriteLine("Game started!");
        for (int turn = 1; turn <= turns; turn++)
        {
            Console.WriteLine($"\nTurn {turn}:");
            PlayTurn();
        }
        DisplayResults();
    }

    private void PlayTurn()
    {
        IPlayer currentPlayer = Players[CurrentPlayerIndex];
        Console.WriteLine($"It's {currentPlayer.Name}'s turn.");
        
        Random rand = new Random();
        int deltaX = rand.Next(-1, 2);
        int deltaY = rand.Next(-1, 2);
        currentPlayer.Move(deltaX, deltaY, GameBoard.Width, GameBoard.Height);
        
        GameBoard.CheckReward(currentPlayer);
        
        currentPlayer.PerformSpecialAction();
        
        CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Count;
    }

    private void DisplayResults()
    {
        Console.WriteLine("\nGame over! Final scores:");
        foreach (var player in Players)
        {
            Console.WriteLine($"{player.Name}: {player.Score} points");
        }

        var winner = Players.OrderByDescending(p => p.Score).First();
        Console.WriteLine($"\nThe winner is {winner.Name} with {winner.Score} points! Congratulations!");
    }

    private void OnRewardCollected(IPlayer player, int points)
    {
        Console.WriteLine($"{player.Name} collected a reward worth {points} points!");
        player.UpdateScore(points);
    }
}

class Program
{
    static void Main()
    {
        int boardWidth = 5;
        int boardHeight = 5;
        int rewardCount = 5;
        int maxPoints = 10;
        int turns = 10;
        
        List<IPlayer> players = new List<IPlayer>
        {
            new Warrior("Alice"),
            new Mage("Bob"),
            new Healer("Charlie")
        };
        
        Game game = new Game(boardWidth, boardHeight, rewardCount, maxPoints, players);
        game.StartGame(turns);
    }
}





