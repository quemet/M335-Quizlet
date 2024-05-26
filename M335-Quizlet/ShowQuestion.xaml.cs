namespace M335_Quizlet;

public partial class ShowQuestion : ContentPage
{
	public static int _id;
	public ShowQuestion(int id)
	{
		InitializeComponent();
		_id = id;
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new ShowOneCard());
    }
}