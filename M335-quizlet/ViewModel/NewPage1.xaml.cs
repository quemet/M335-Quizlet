using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace M335_quizlet.ViewModel;

public partial class NewPage1 : ObservableObject
{
    [ObservableProperty]
    private int counter = 0;

    [RelayCommand]
    private void Increment(int incrementValue)
    {
        Counter += incrementValue;
    }
}