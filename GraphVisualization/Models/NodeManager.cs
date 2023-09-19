using System.Collections.ObjectModel;
using GraphVisualization.Models;


//Make singletone
public class NodeManager
{
    public static ObservableCollection<GraphNode> _nodes = new ObservableCollection<GraphNode> { new GraphNode { Position = (250, 250) } };

    public static ObservableCollection<GraphNode> GetNodes() { return _nodes; }

}
