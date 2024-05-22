using M335_Quizlet.viewModels;

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
            await Navigation.PushAsync(new ShowQuestion());
        }
    }

}
