using System.Diagnostics;

namespace M335_Quizlet
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            int id = Convert.ToInt32(b.ClassId);
            await Navigation.PushAsync(new ShowQuestion(id));
        }
    }
}
