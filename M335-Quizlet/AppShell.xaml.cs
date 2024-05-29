namespace M335_Quizlet
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("MainPage", typeof(MainPage));
        }
    }
}
