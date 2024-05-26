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
        private ObservableCollection<Question> questions = new();

        [ObservableProperty]
        public static int quiz_id = ShowQuestion._id;

        public AddCard()
        {
            RefreshWishesFromDB();
        }

        private void RefreshWishesFromDB(Database? db = null)
        {
            Questions.Clear();

            if (db == null)
            {
                db = new Database();
            }

            var allQuestions = db.Questions.Where((dbQuestion) => dbQuestion.Quiz_Id == Quiz_id);

            foreach (var que in allQuestions)
            {
                Questions.Add(new Question { Answer = que.Answer, Response = que.Response, Quiz_Id = Quiz_id });
            }
        }

        [RelayCommand]
        private async Task Add()
        {
            string answer = await Shell.Current.DisplayPromptAsync(title: "Ajouter une question", message: "");
            string response = await Shell.Current.DisplayPromptAsync(title: "Ajouter une réponse", message: "");
            var question = new Question { Answer = answer, Response = response, Quiz_Id = Quiz_id };
            using (var db = new Database())
            {
                db.Questions.Add(question);
                await db.SaveChangesAsync();
            }
            Questions.Add(question);
            RefreshWishesFromDB();
        }

        [RelayCommand]
        private async Task Edit(Question question)
        {
            Trace.WriteLine($"Editing {question}");

            string updatedTitle = await Shell.Current.DisplayPromptAsync(title: "Modifier le titre", message: "", placeholder: question.Answer);
            string updatedDescription = await Shell.Current.DisplayPromptAsync(title: "Modifer la description", message: "", placeholder: question.Response);

            if (updatedTitle != null && updatedDescription != null)
            {
                using (var db = new Database())
                {
                    await db.Questions
                        .Where(dbCard => dbCard.Id == question.Id)
                        .ExecuteUpdateAsync(setters => setters.SetProperty(dbQuiz => dbQuiz.Answer, updatedDescription).SetProperty(dbQuiz => dbQuiz.Response, updatedTitle));

                    RefreshWishesFromDB(db);
                }
            }
        }

        [RelayCommand]
        private async Task Delete(Question question)
        {
            Trace.WriteLine($"Deleting {question}");

            string deletedTitle = await Shell.Current.DisplayPromptAsync(title: "Supprimmer la Question", message: "Entrer le titre de la question à supprimmer");

            using (var db = new Database())
            {
                await db.Questions.Where(dbQuiz => dbQuiz.Answer == deletedTitle).ExecuteDeleteAsync();

                RefreshWishesFromDB(db);
            }
        }
    }
}