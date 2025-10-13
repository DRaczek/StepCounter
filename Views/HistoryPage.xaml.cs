using StepCounter.Data;
using StepCounter.ViewModels;

namespace StepCounter;

public partial class HistoryPage : ContentPage
{
    public HistoryPage(HistoryViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
