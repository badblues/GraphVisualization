using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using GraphVisualization.Commands;
using GraphVisualization.Models;

namespace GraphVisualization.ViewModels;

public class MainViewModel
{
    public ObservableCollection<GraphNode> Nodes { get; set; }
    public ICommand AddNodeCommand { get; init; }
    public ICommand StartDragCommand { get; init; }
    public ICommand DragCommand { get; init; }
    public ICommand EndDragCommand { get; init; }
    private GraphNode? SelectedNode { get; set; }

    public MainViewModel()
    {
        Nodes = NodeManager.GetNodes();
        AddNodeCommand = new RelayCommand(ExecuteAddNode, CanAddNode);
        StartDragCommand = new RelayCommand(ExecuteStartDrag, CanStartDrag);
        DragCommand = new RelayCommand(ExecuteDrag, CanDrag);
        EndDragCommand = new RelayCommand(ExecuteEndDrag, CanEndDrag);
        SelectedNode = null;
    }

    private bool CanAddNode(object obj)
    {
        return true;
    }

    private void ExecuteAddNode(object obj)
    {
        Debug.WriteLine("Adding node");
        Random random = new Random();
        Nodes.Add(new GraphNode { X = 250 + random.Next(100) - 50, Y = 250 + random.Next(100) - 50 });
    }


    private void ExecuteStartDrag(object sender)
    {
        if (sender is UIElement element)
        {
            var node = FindNodeByUIElement(element);
            if (node != null)
            {
                SelectedNode = node;
            }
        }
    }

    private bool CanStartDrag(object parameter)
    {
        return SelectedNode is null;
    }

    private void ExecuteDrag(object parameter)
    {
        if (SelectedNode != null && parameter is MouseEventArgs e)
        {
            // Update the node's position based on the mouse drag.
            SelectedNode.X = (int)e.GetPosition(null).X - 15;
            SelectedNode.Y = (int)e.GetPosition(null).Y - 15;
        }
    }

    private bool CanDrag(object parameter)
    {
        return SelectedNode != null;
    }
    private void ExecuteEndDrag(object parameter)
    {
        SelectedNode = null; // Clear the selected node after dragging.
    }

    private bool CanEndDrag(object parameter)
    {
        return SelectedNode != null;
    }

    private GraphNode? FindNodeByUIElement(object sender)
    {
        if (sender is FrameworkElement element)
        {
            return element.DataContext as GraphNode;
        }
        return null;
    }


}