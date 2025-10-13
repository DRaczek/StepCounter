using CommunityToolkit.Mvvm.ComponentModel;
using StepCounter.Data;
using StepCounter.Models;
using System.Collections.ObjectModel;

namespace StepCounter.ViewModels
{
    public partial class HistoryViewModel : ObservableObject
    {
        private readonly StepDatabase stepDatabase;

        [ObservableProperty]
        private ObservableCollection<DailyStep> allSteps = new();

        [ObservableProperty]
        private ObservableCollection<DailyStep> lastMonthSteps = new();

        public HistoryViewModel(StepDatabase stepDatabase)
        {
            this.stepDatabase = stepDatabase;
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var steps = await stepDatabase.GetStepsAsync();
            var ordered = steps.OrderByDescending(s => s.Date).ToList();
            AllSteps = new ObservableCollection<DailyStep>(ordered);

            var fromDate = DateTime.Today.AddDays(-29); // 30 dni łącznie: dziś + 29 dni wstecz
            var last30 = ordered
                .Where(s => s.Date.Date >= fromDate && s.Date.Date <= DateTime.Today)
                .Select(s => new DailyStep
                {
                    Date = s.Date.Date,   // <<< to usuwa część czasu
                    Steps = s.Steps
                })
                .OrderBy(s => s.Date)
                .ToList();

            LastMonthSteps = new ObservableCollection<DailyStep>(last30);
        }
    }
}
