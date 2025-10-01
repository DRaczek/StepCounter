using System;
using System.Timers;
using CommunityToolkit.Mvvm.ComponentModel;
using Plugin.Maui.Pedometer;
using StepCounter.Data;
using StepCounter.Models;

namespace StepCounter.Services
{
    public partial class StepCounterService : ObservableObject
    {
        private const double StepLengthMeters = 0.75;
        private const double CaloriesPerStep = 0.04;

        [ObservableProperty]
        private int dailySteps;

        [ObservableProperty]
        private double distanceKm;

        [ObservableProperty]
        private double calories;

        private readonly IPedometer pedometer;
        private readonly StepDatabase stepDatabase;
        private int previousNumberOfSteps = 0;
        private DateTime lastResetDate = DateTime.Today;

        public StepCounterService(IPedometer pedometer, StepDatabase stepDatabase)
        {
            this.pedometer = pedometer;
            this.stepDatabase = stepDatabase;
            pedometer.ReadingChanged += OnReadingChanged;
            pedometer.Start();
            _ = UpdateDailyStepsFromDatabase();
        }

        public async Task UpdateDailyStepsFromDatabase()
        {
            DailyStep? dailyStep = await stepDatabase.GetStepForDateAsync(DateTime.Today);
            if(dailyStep != null)
            {
                DailySteps = dailyStep.Steps;
            }
        }

        partial void OnDailyStepsChanged(int value)
        {
            CalculateDistanceAndCalories();
        }

        private void CalculateDistanceAndCalories()
        {
            DistanceKm = (DailySteps * StepLengthMeters) / 1000.0;
            Calories = DailySteps * CaloriesPerStep;
        }


        private void OnReadingChanged(object? sender, PedometerData reading)
        {
            if (reading == null) return;

            int diff = reading.NumberOfSteps - previousNumberOfSteps;
            previousNumberOfSteps = reading.NumberOfSteps;

            if (diff > 0)
                DailySteps += diff;
        }

        private void ResetIfNewDay()
        {
            if (DateTime.Today <= lastResetDate) return;

            DailySteps = 0;
            lastResetDate = DateTime.Today;
        }

        public async Task SaveDailyStepsToDbAsync()
        {
            ResetIfNewDay();
            var today = DateTime.Today;
            var stepEntity = await stepDatabase.GetStepForDateAsync(today);
            if (stepEntity == null)
            {
                await stepDatabase.SaveStepAsync(new DailyStep { Date = today, Steps = DailySteps });
            }
            else
            {
                stepEntity.Steps = DailySteps;
                await stepDatabase.UpdateStepAsync(stepEntity);
            }
        }

        public void Dispose()
        {
            pedometer.ReadingChanged -= OnReadingChanged;
        }
    }
}