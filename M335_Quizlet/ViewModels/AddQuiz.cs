using CommunityToolkit.Mvvm.ComponentModel;
using M335_Quizlet.Models;
using M335_Quizlet.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace M335_Quizlet.ViewModels
{
    public sealed partial class AddQuiz : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Quiz> quizzes = new();

        public AddQuiz()
        {
            RefreshQuizFromDB();
        }

        private async Task Add(string title, string description)
        {
            var quiz = new Quiz { Title = title, Description = description };
            using (var db = new Database())
            {
                db.Add(quiz);
                await db.SaveChangesAsync();
            }

            Quizzes.Add(quiz);
        }

        private void RefreshQuizFromDB(Database? context = null)
        {
            Quizzes.Clear();
            using (var dbContext = context ?? new Database())
            {
                foreach (var dbWish in dbContext.Quizzes)
                {
                    Quizzes.Add(dbWish);
                }
            }
        }

        private async Task Edit(Quiz quiz)
        {
            Trace.WriteLine($"Editing {quiz}");

            string updatedTitle = await Shell.Current.DisplayPromptAsync(title: "Modifier le titre", message: $"{quiz.Title}", placeholder: quiz.Title);
            string updatedDescription = await Shell.Current.DisplayPromptAsync(title: "Modifier le description", message: $"{quiz.Description}", placeholder: quiz.Description);

            if (updatedTitle != null)
            {
                if (updatedDescription != null)
                {
                    using (var dbContext = new Database())
                    {
                        await dbContext.Quizzes
                            .Where(dbQuiz => dbQuiz.QuizId == quiz.QuizId)
                            .ExecuteUpdateAsync(setters => setters.SetProperty(dbQuiz => dbQuiz.Title, updatedTitle).SetProperty(dbQuiz => dbQuiz.Description, updatedDescription));

                        RefreshQuizFromDB(dbContext);
                    }
                }
                else
                {
                    using (var dbContext = new Database())
                    {
                        await dbContext.Quizzes
                            .Where(dbQuiz => dbQuiz.QuizId == quiz.QuizId)
                            .ExecuteUpdateAsync(setters => setters.SetProperty(dbQuiz => dbQuiz.Title, updatedTitle));

                        RefreshQuizFromDB(dbContext);
                    }

                }
            }
            else
            {
                if (updatedDescription != null)
                {
                    using (var dbContext = new Database())
                    {
                        await dbContext.Quizzes
                            .Where(dbQuiz => dbQuiz.QuizId == quiz.QuizId)
                            .ExecuteUpdateAsync(setters => setters.SetProperty(dbQuiz => dbQuiz.Description, updatedDescription));

                        RefreshQuizFromDB(dbContext);
                    }
                }
            }
        }

        private async Task Delete(Quiz quiz)
        {
            Trace.WriteLine($"Deleting {quiz}");
            using (var dbContext = new Database())
            {
                await dbContext.Quizzes
                    .Where(dbWish => dbWish.QuizId == quiz.QuizId)
                    .ExecuteDeleteAsync();

                RefreshQuizFromDB(dbContext);
            }
        }
    }
}