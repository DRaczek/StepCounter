using CommunityToolkit.Mvvm.ComponentModel;
using StepCounter.Data;
using StepCounter.Models;
using StepCounter.Services;
using System.Collections.ObjectModel;

namespace StepCounter.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly StepCounterService stepCounterService;
        private readonly SettingsViewModel settingsViewModel;
        private readonly StepDatabase stepDatabase;

        public int DailySteps => stepCounterService.DailySteps;

        public int DailyStepGoal => settingsViewModel.DailyStepGoal;

        [ObservableProperty]
        public ObservableCollection<DailyStep> stepHistory = new();

        public MainViewModel(StepCounterService stepCounterService, StepDatabase stepDatabase, SettingsViewModel settingsViewModel)
        {
            this.stepCounterService = stepCounterService;
            this.stepDatabase = stepDatabase;
            this.settingsViewModel = settingsViewModel;

            stepCounterService.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(StepCounterService.DailySteps))
                    OnPropertyChanged(nameof(DailySteps));
            };

            settingsViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(SettingsViewModel.DailyStepGoal))
                    OnPropertyChanged(nameof(DailyStepGoal));
            };

            _ = LoadHistoryAsync();
        }

        private async Task LoadHistoryAsync()
        {
            var steps = await stepDatabase.GetStepsAsync();
            StepHistory = new ObservableCollection<DailyStep>(steps.Where(step => step.Date < DateTime.Today));
        }
    }
}