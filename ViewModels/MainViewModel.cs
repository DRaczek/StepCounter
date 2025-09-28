using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Maui.Pedometer;
using StepCounter.Services;

namespace StepCounter.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly StepCounterService stepCounterService;

        public int DailySteps => stepCounterService.DailySteps;

        public MainViewModel(StepCounterService stepCounterService)
        {
            this.stepCounterService = stepCounterService;
            stepCounterService.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(StepCounterService.DailySteps))
                    OnPropertyChanged(nameof(DailySteps));
            };
        }
    }
}