using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GraphVisualization.Commands;
using GraphVisualization.Models;

namespace GraphVisualization.ViewModels;

public class MainViewModel
{
    public ObservableCollection<GraphNode> Nodes { get; set; }

    public ICommand AddNodeCommand { get; set; }

    public MainViewModel()
    {
        Nodes = NodeManager.GetNodes();
        AddNodeCommand = new RelayCommand(AddNode, CanAddNode);
    }

    private bool CanAddNode(object obj)
    {
        return true;
    }

    private void AddNode(object obj)
    {
        Random random = new Random();
        Nodes.Add(new GraphNode { X = 250 + random.Next(100) - 50, Y = 250 + random.Next(100) - 50 });
    }

}
