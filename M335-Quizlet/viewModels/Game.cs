using CommunityToolkit.Mvvm.ComponentModel;
using M335_Quizlet.Services;
using System.Collections.ObjectModel;
using M335_Quizlet.Models;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

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

    [ObservableProperty]
    private string timeToFinish = string.Empty;

    [ObservableProperty]
    private string harderCard = string.Empty;

    [ObservableProperty]
    private ObservableCollection<Card> allPerfectCard = new();

    [ObservableProperty]
    private string percentage = string.Empty;

    [ObservableProperty]
    private Stopwatch time = new();

    [ObservableProperty]
    private string lbl = string.Empty;

    public Game()
    {
        Time.Start();
        MainLoop();
        NextCard();
        EnableAccelerometer();
    }

    [RelayCommand]
    private void EnableAccelerometer()
    {
        Accelerometer.Default.ShakeDetected += Default_ShakeDetected; ;
        Accelerometer.Default.Start(SensorSpeed.Default);
    }

    [RelayCommand]
    private void DisableAccelerometer()
    {
        Accelerometer.Default.ShakeDetected -= Default_ShakeDetected;
        Accelerometer.Default.Stop();
    }

    private void Default_ShakeDetected(object? sender, EventArgs e)
    {
        Debug.WriteLine("Accelerometer reading changed event triggered.");
        if (!string.IsNullOrEmpty(CardName))
        {
            foreach (Card card1 in Cards)
            {
                if (card1.Question == CardName)
                {
                    Cards.Add(card1);
                    break;
                }
            }
        }
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
        using (var db = new Database())
        {
            foreach (var card in db.Cards)
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
            CardName = question.Question;
            Index++;
        }
        catch (Exception ex) 
        {
            await Stats();
        }
    }

    [RelayCommand]
    private async Task EndGame()
    {
        if (Index < Cards.Count)
        {
            List<Card> tempList = new List<Card>();
            for (int i = Index; i < Cards.Count; i++)
            {
                tempList.Add(Cards[i]);
            }

            foreach (var card in tempList)
            {
                Cards.Add(card);
            }
        }

        await Stats();
    }


    [RelayCommand]
    private async Task Stats()
    {
        Time.Stop();
        List<int> countNmbByCard = new List<int>();

        List<int> indexPerfect = new List<int>();
        List<Card> perfectCard = new();

        string allCard = "";

        foreach (var card in Cards)
        {
            countNmbByCard.Add(Cards.Count(lcard => lcard == card));
        }

        int max = countNmbByCard.Max();
        int index = countNmbByCard.IndexOf(max);
        Card hardCard = Cards[index];

        for (int i = 0; i < Cards.Count; i++)
        {
            if (countNmbByCard[i] == 1)
            {
                indexPerfect.Add(i);
            }
        }

        foreach (int ind in indexPerfect)
        {
            perfectCard.Add(Cards[ind]);
        }

        TimeToFinish = "You finished it in " + Time.Elapsed.ToString();
        HarderCard = "The harder card is : " + hardCard.ToString();

        foreach (Card card in perfectCard)
        {
            AllPerfectCard.Add(card);
            allCard += card.ToString() + "\n";
        }

        Lbl = "All the perfect card are : ";
        Percentage = "Your percentage of perfect is : " + ((perfectCard.Count / Cards.Count) * 100).ToString() + "%";
        DisableAccelerometer();

        await Shell.Current.DisplayAlert(title: "Here is your Stats", message: TimeToFinish + "\n\n" + HarderCard + "\n\n" + "All the perfect card is : " + "\n" + allCard + "\n\n" + Percentage, cancel: "Ok");

        await Shell.Current.GoToAsync("//MainPage");
    }
}
