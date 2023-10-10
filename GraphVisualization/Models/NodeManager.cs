using System.Collections.ObjectModel;

namespace GraphVisualization.Models;

//TODO: Make singletone
public class NodeManager
{
    private static ObservableCollection<GraphNode> _nodes = new ObservableCollection<GraphNode> { };
    private static ObservableCollection<GraphNodeConnection> _connections = new ObservableCollection<GraphNodeConnection> { };

    public static ObservableCollection<GraphNode> Nodes { get { return _nodes; } }
    public static ObservableCollection<GraphNodeConnection> Connections { get { return _connections; } }

}
