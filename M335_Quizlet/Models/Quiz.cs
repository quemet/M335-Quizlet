namespace M335_Quizlet.Models;

public class Quiz
{
	// public List<Card> Cards = new();
	public int QuizId { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }

    public override string ToString()
    {
        return Title.ToString();
    }
}