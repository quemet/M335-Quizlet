using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using M335_Quizlet.Models;
using M335_Quizlet.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace M335_Quizlet.viewModels
{
    public sealed partial class AddQuiz : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Quiz> quizzes = new();

        public AddQuiz()
        {
            RefreshWishesFromDB();
        }

        [RelayCommand]
        private async Task Add()
        {
            string title = await Shell.Current.DisplayPromptAsync(title: "Entrer un titre", message: "");
            string description = await Shell.Current.DisplayPromptAsync(title: "Entrer une description", message: "");

            var quiz = new Quiz { Title = title, Description = description };

            using (var db = new Database())
            {
                db.Add(quiz);
                await db.SaveChangesAsync();
            }

            Quizzes.Add(quiz);

            RefreshWishesFromDB();
        }

        private void RefreshWishesFromDB(Database? database = null)
        {
            Quizzes.Clear();
            using(var db = database??new Database())
            {
                foreach (var dbQuiz in db.Quizzes)
                {
                    Quizzes.Add(dbQuiz);
                }
            }
        }

        [RelayCommand]
        private async Task Edit(Quiz quiz)
        {
            Trace.WriteLine($"Editing {quiz}");

            string updatedTitle = await Shell.Current.DisplayPromptAsync(title: "Modifier le titre", message: "");
            string updatedDescription = await Shell.Current.DisplayPromptAsync(title: "Modifer la description", message: "");

            if(updatedTitle != null && updatedDescription != null)
            {
                using(var db = new Database())
                {
                    await db.Quizzes
                        .Where(dbQuiz => dbQuiz.Id == quiz.Id)
                        .ExecuteUpdateAsync(setters => setters.SetProperty(dbQuiz => dbQuiz.Description, updatedDescription).SetProperty(dbQuiz => dbQuiz.Title, updatedTitle));

                    RefreshWishesFromDB(db);
                }
            }
        }

        [RelayCommand]
        private async Task Delete()
        {
            string deletedTitle = await Shell.Current.DisplayPromptAsync(title: "Supprimmer le titre", message: "Entrer le titre à supprimmer");
            Quiz quiz = Quizzes.First((dbQuiz) => dbQuiz.Title == deletedTitle);
            Trace.WriteLine($"Deleting {quiz}");
            using(var db = new Database())
            {
                await db.Quizzes.Where(dbQuiz => dbQuiz.Id == quiz.Id).ExecuteDeleteAsync();

                RefreshWishesFromDB(db);
            }
        }

        [RelayCommand]
        private async Task ChangePage(int id)
        {
            FileStream file = File.Create(@"C:\file.txt");
        }
    }
}