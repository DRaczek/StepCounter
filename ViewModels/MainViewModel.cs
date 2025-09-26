using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Maui.Pedometer;

namespace StepCounter.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private int steps;

        [ObservableProperty]
        private bool isCounting = false;

        private int previousNumberOfSteps = 0;

        readonly IPedometer pedometer;
        public MainViewModel(IPedometer pedometer)
        {
            this.pedometer = pedometer;
        }
            
        [RelayCommand]
        public void Start()
        {
            if (IsCounting == false) Reset();
            pedometer.ReadingChanged += (sender, reading) =>
            {
                int value = reading.NumberOfSteps;
                int diff =  value - previousNumberOfSteps;
                previousNumberOfSteps = reading.NumberOfSteps;
                Steps += diff;
            };

            pedometer.Start();
            IsCounting = true;
        }

        [RelayCommand]
        public void Stop()
        {
            pedometer.Stop();
            IsCounting = false;
        }

        [RelayCommand]
        public void Reset()
        {
            Steps = 0;
        }
    }
}