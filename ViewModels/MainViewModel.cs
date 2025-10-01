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
        private readonly StepDatabase stepDatabase;

        public int DailySteps => stepCounterService.DailySteps;

        [ObservableProperty]
        public ObservableCollection<DailyStep> stepHistory = new();

        public MainViewModel(StepCounterService stepCounterService, StepDatabase stepDatabase)
        {
            this.stepCounterService = stepCounterService;
            this.stepDatabase = stepDatabase;

            stepCounterService.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(StepCounterService.DailySteps))
                    OnPropertyChanged(nameof(DailySteps));
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