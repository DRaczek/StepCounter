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

        public HistoryViewModel(StepDatabase stepDatabase)
        {
            this.stepDatabase = stepDatabase;
            _ = LoadAllStepsAsync();
        }

        private async Task LoadAllStepsAsync()
        {
            var steps = await stepDatabase.GetStepsAsync();
            AllSteps = new ObservableCollection<DailyStep>(steps);
        }
    }
}
