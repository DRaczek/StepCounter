using CommunityToolkit.Mvvm.ComponentModel;
using StepCounter.Data;
using StepCounter.Helpers;
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
        private readonly Utils utils;

        public int DailySteps => stepCounterService.DailySteps;
        public int DailyStepGoal => settingsViewModel.DailyStepGoal;
        public double DistanceKm => stepCounterService.DistanceKm;
        public double Calories => stepCounterService.Calories;

        [ObservableProperty]
        public ObservableCollection<DailyStep> stepHistory = new();

        public MainViewModel(
            StepCounterService stepCounterService,
            StepDatabase stepDatabase,
            SettingsViewModel settingsViewModel,
            Utils utils)
        {
            this.stepCounterService = stepCounterService;
            this.stepDatabase = stepDatabase;
            this.settingsViewModel = settingsViewModel;
            this.utils = utils;

            utils.ForwardProperties( stepCounterService, new List<string>
            {
                nameof(StepCounterService.DailySteps),
                nameof(StepCounterService.DistanceKm),
                nameof(StepCounterService.Calories)
            }, OnPropertyChanged);
            utils.ForwardProperty(settingsViewModel, nameof(SettingsViewModel.DailyStepGoal), OnPropertyChanged);

            _ = LoadHistoryAsync();
        }

        private async Task LoadHistoryAsync()
        {
            var steps = await stepDatabase.GetStepsAsync();
            StepHistory = new ObservableCollection<DailyStep>(steps.Where(step => step.Date < DateTime.Today));
        }
    }
}