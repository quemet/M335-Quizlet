namespace M335_QuizletLib.Models;

public class Card
{
    public int Id { get; set; }
    public string? Question { get; set; }
    public string? Response { get; set; }
    public DateTime Created { get; set; }

    public override string ToString()
    {
        return $"[Card {Id}]";
    }
}