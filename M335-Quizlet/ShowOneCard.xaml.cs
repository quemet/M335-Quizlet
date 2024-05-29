using M335_Quizlet.viewModels;

namespace M335_Quizlet;

public partial class ShowOneCard : ContentPage
{
	public static int _id;
	public ShowOneCard(int id)
	{
		InitializeComponent();
		_id = id;
	}

	public static int ChangeId() { return _id; }
}