using CommunityToolkit.Mvvm.ComponentModel;
using M335_Quizlet.Services;
using System.Collections.ObjectModel;
using M335_Quizlet.Models;
using CommunityToolkit.Mvvm.Input;

namespace M335_Quizlet.viewModels;

public sealed partial class Game : ObservableObject
{
	[ObservableProperty]
	private ObservableCollection<Question> questions = new();

	[ObservableProperty]
	private string cardName = string.Empty;

	[ObservableProperty]
	private bool isClicked = false;

	[ObservableProperty]
	private int index = 0;

	[ObservableProperty]
	private int quiz_id = ShowQuestion._id;

	public Game()
	{
		MainLoop();
		NextCard();
	}

    public void MelangerCartes<T>()
    {
        Random rand = new Random();
        int n = Questions.Count;
        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            Question value = Questions[k];
            Questions[k] = Questions[n];
            Questions[n] = value;
        }
    }

    public void MainLoop()
	{
		int quiz_id = Quiz_id;
		using(var db = new Database()) 
		{
			foreach(var question in db.Questions)
			{
				if(question.Quiz_Id == quiz_id)
				{
					Questions.Add(question);
				}
			}
		}

		MelangerCartes<Question>();
	}

	[RelayCommand]
	public void NextCard()
	{
		try 
		{
            Question question = Questions[Index];
            CardName = IsClicked ? question.Response : question.Answer;
            IsClicked = !IsClicked;
            Index++;
        } catch (Exception ex) 
		{
			// EndGame
		}
	}
}