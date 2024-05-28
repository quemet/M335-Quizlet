namespace M335_Quizlet;

public partial class ShowOneCard : ContentPage
{
	public int _id;
	public ShowOneCard()
	{
		InitializeComponent();
		StreamReader streamReader = new StreamReader("C:\\Users\\quent\\OneDrive\\Bureau\\M335-Quizlet\\M335-Quizlet\\file.txt");
		string id = streamReader.ReadLine();
		_id = Convert.ToInt32(id);
	}
}