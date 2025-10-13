using StepCounter.ViewModels;

namespace StepCounter;

public partial class SettingsPage : ContentPage
{
    private SettingsViewModel vm;
    public SettingsPage(SettingsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        this.vm = vm;
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        stepGoalPopup.Show();
    }

    private void stepGoalPopup_Closed(object sender, EventArgs e)
    {
        vm.SaveGoalCommand.Execute(null);
    }
}
