using CommunityToolkit.Mvvm.ComponentModel;
using M335_Quizlet.Services;
using System.Collections.ObjectModel;
using M335_Quizlet.Models;
using CommunityToolkit.Mvvm.Input;

namespace M335_Quizlet.viewModels;

public sealed partial class Game : ObservableObject
{
	[ObservableProperty]
	private ObservableCollection<Card> cards = new();

	[ObservableProperty]
	private string cardName = string.Empty;

	[ObservableProperty]
	private bool isClicked = false;

	[ObservableProperty]
	private int index = 0;

	public Game()
	{
		MainLoop();
		NextCard();
	}

    public void MelangerCartes<T>()
    {
        Random rand = new Random();
        int n = Cards.Count;
        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            Card value = Cards[k];
            Cards[k] = Cards[n];
            Cards[n] = value;
        }
    }

    public void MainLoop()
	{
		using(var db = new Database()) 
		{
			foreach(var card in db.Cards)
			{
				Cards.Add(card);
			}
		}

		MelangerCartes<Card>();
	}

	[RelayCommand]
	private async Task NextCard()
	{
		try 
		{
            Card question = Cards[Index];
            CardName = IsClicked ? question.Response : question.Question;
            Index++;
			IsClicked = false;
        } catch (Exception ex) 
		{
			throw new Exception(ex.ToString());
		}
	}

	[RelayCommand]
	private async Task ChangeName()
	{
		try
		{
			Card question = Cards[Index];
            IsClicked = !IsClicked;
            CardName = IsClicked ? question.Response : question.Question;
        } catch (Exception ex)
		{
			throw new Exception(ex.ToString());
        }
    }
}