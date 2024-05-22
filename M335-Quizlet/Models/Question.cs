namespace M335_Quizlet.Models;

public class Question
{
    public int Id { get; set; }
    public string Answer { get; set; }
    public string Response { get; set; }
    public int Quiz_Id { get; set; }
    public DateTime Created { get; set; }

    public override string ToString()
    {
        return $"[Card {Id}]";
    }
}