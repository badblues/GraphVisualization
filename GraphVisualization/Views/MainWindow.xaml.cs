﻿using System.Windows;
using System.Windows.Input;
using GraphVisualization.ViewModels;

namespace GraphVisualization.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        MainViewModel mainViewModel = new MainViewModel();
        DataContext = mainViewModel;
        InitializeComponent();
    }

    private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is MainViewModel viewModel)
        {
            if (viewModel.IsConnecting)
                viewModel.ConnectNodeCommand.Execute(sender);
            else
                viewModel.StartDragCommand.Execute(sender);
        }
    }

    private void Canvas_MouseMove(object sender, MouseEventArgs e)
    {
        if (DataContext is MainViewModel viewModel && e.LeftButton == MouseButtonState.Pressed)
        {
            if (!viewModel.IsConnecting)
                viewModel.DragCommand.Execute(e);
        }
    }

    private void Ellipse_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is MainViewModel viewModel)
        {
            if (!viewModel.IsConnecting)
                viewModel.EndDragCommand.Execute(null);
        }
    }

}
