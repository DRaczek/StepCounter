using Microsoft.Maui.Controls;
using StepCounter.ViewModels;

namespace StepCounter
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
            
        }
    }
}
