
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