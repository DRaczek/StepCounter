using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Storage;

namespace StepCounter.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        private const string DailyGoalKey = "DailyStepGoal";
        private const int DefaultGoal = 5000;

        [ObservableProperty]
        private double dailyStepGoal;

        public SettingsViewModel()
        {
            DailyStepGoal = Preferences.Get(DailyGoalKey, DefaultGoal);
        }

        [RelayCommand]
        private void SaveGoal()
        {
            Preferences.Set(DailyGoalKey, (int)DailyStepGoal);
        }
    }
}
