using System.Collections.ObjectModel;
using GraphVisualization.Models;


//Make singletone
public class NodeManager
{
    public static ObservableCollection<GraphNode> _nodes = new ObservableCollection<GraphNode> {};

    public static ObservableCollection<GraphNode> GetNodes() { return _nodes; }

}
