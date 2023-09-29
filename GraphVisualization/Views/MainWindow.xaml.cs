using System.Windows;
using System.Windows.Input;
using GraphVisualization.ViewModels;

namespace GraphVisualization.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainViewModel mainViewModel = new MainViewModel();
        DataContext = mainViewModel;
    }

    private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is MainViewModel viewModel)
        {
            viewModel.StartDragCommand.Execute(sender);
        }
    }

    private void Ellipse_MouseMove(object sender, MouseEventArgs e)
    {
        if (DataContext is MainViewModel viewModel && e.LeftButton == MouseButtonState.Pressed)
        {
            viewModel.DragCommand.Execute(e);
        }
    }

    private void Ellipse_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is MainViewModel viewModel)
        {
            viewModel.EndDragCommand.Execute(null);
        }
    }

}
