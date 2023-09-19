using System.Windows;
using GraphVisualization.ViewModels;

namespace GraphVisualization.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainViewModel mainViewModel = new MainViewModel();
        this.DataContext = mainViewModel;
    }
}
