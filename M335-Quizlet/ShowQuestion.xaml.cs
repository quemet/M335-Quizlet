using System.Web;

namespace M335_Quizlet;

public partial class ShowQuestion : ContentPage
{
    public static int _id;
    public ShowQuestion()
    {
        InitializeComponent();
        StreamReader streamReader = new StreamReader("C:\\Users\\quent\\OneDrive\\Bureau\\M335-Quizlet\\M335-Quizlet\\file.txt");
        string id = streamReader.ReadLine();
        _id = Convert.ToInt32(id);
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ShowOneCard());
    }
}