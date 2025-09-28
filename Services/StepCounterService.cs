using CommunityToolkit.Mvvm.ComponentModel;
using Plugin.Maui.Pedometer;
using StepCounter.Models;

namespace StepCounter.Services
{
    public partial class StepCounterService : ObservableObject
    {
        [ObservableProperty]
        private int dailySteps;

        private readonly IPedometer pedometer;
        private int previousNumberOfSteps = 0;
        private DateTime lastResetDate = DateTime.Today;

        public StepCounterService(IPedometer pedometer)
        {
            this.pedometer = pedometer;
            pedometer.ReadingChanged += OnReadingChanged;

            pedometer.Start();
        }

        private void OnReadingChanged(object? sender, PedometerData reading)
        {
            CheckMidnightReset();
            int value = reading.NumberOfSteps;
            int diff = value - previousNumberOfSteps;
            previousNumberOfSteps = value;
            if (diff > 0)
                DailySteps += diff;
        }

        private void CheckMidnightReset()
        {
            if (DateTime.Today > lastResetDate)
            {
                DailySteps = 0;
                previousNumberOfSteps = 0;
                lastResetDate = DateTime.Today;
            }
        }
    }
}