using System.Collections.ObjectModel;
using GraphVisualization.Models;

namespace GraphVisualization.ViewModels;

public class MainViewModel
{
    public ObservableCollection<GraphNode> Nodes { get; set; }

    public MainViewModel()
    {
        Nodes = NodeManager.GetNodes();
    }

}
