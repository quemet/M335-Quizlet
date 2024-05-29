using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using M335_Quizlet.Models;
using M335_Quizlet.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace M335_Quizlet.viewModels
{
    public sealed partial class AddCard : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Card> cards = new();

        public AddCard()
        {
            RefreshWishesFromDB();
        }

        private void RefreshWishesFromDB(Database? db = null)
        {
            Cards.Clear();
            using (var database = db ?? new Database())
            {
                foreach (var dbQuestion in database.Cards)
                {
                    Cards.Add(dbQuestion);
                }
            }
        }

        [RelayCommand]
        private async Task Add()
        {
            string question = await Shell.Current.DisplayPromptAsync(title: "Ajouter une question", message: "");
            string response = await Shell.Current.DisplayPromptAsync(title: "Ajouter une réponse", message: "");

            var card = new Card { Question = question, Response = response };

            using (var db = new Database())
            {
                db.Cards.Add(card);
                await db.SaveChangesAsync();
            }
            Cards.Add(card);
            RefreshWishesFromDB();
        }

        [RelayCommand]
        private async Task Edit(Card card)
        {
            Trace.WriteLine($"Editing {card}");

            string updatedQuestion = await Shell.Current.DisplayPromptAsync(title: "Modifier le titre", message: "", placeholder: card.Question);
            string updatedResponse = await Shell.Current.DisplayPromptAsync(title: "Modifer la description", message: "", placeholder: card.Response);

            if (updatedQuestion != null || updatedResponse != null)
            {
                using (var db = new Database())
                {
                    await db.Cards
                        .Where(dbCard => dbCard.Id == card.Id)
                        .ExecuteUpdateAsync(setters => setters.SetProperty(dbQuiz => dbQuiz.Question, updatedQuestion).SetProperty(dbQuiz => dbQuiz.Response, updatedResponse));

                    RefreshWishesFromDB(db);
                }
            }
        }

        [RelayCommand]
        private async Task Delete(Card card)
        {
            Trace.WriteLine($"Deleting {card}");

            string deletedQuestion = await Shell.Current.DisplayPromptAsync(title: "Supprimmer la Question", message: "Entrer le titre de la question à supprimmer");

            using (var db = new Database())
            {
                await db.Cards.Where(dbQuiz => dbQuiz.Question == deletedQuestion).ExecuteDeleteAsync();

                RefreshWishesFromDB(db);
            }
        }
    }
}