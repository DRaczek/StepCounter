using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        private ObservableCollection<DailyStep> recentSteps = new();

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

            _ = LoadRecentStepsAsync();
        }

        private async Task LoadRecentStepsAsync()
        {
            var steps = await stepDatabase.GetStepsAsync(3);
            RecentSteps = new ObservableCollection<DailyStep>(steps);
        }

        [RelayCommand]
        private async Task ShowHistory()
        {
            await Shell.Current.GoToAsync(nameof(HistoryPage));
        }
    }
}