namespace M335_Quizlet.Models;

public class Quiz
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<Question> Questions { get; set; }
    public DateTime Created { get; set; }

    public override string ToString()
    {
        return $"[Quiz {Id}]";
    }
}