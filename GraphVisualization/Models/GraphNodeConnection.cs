namespace GraphVisualization.Models;

public class GraphNodeConnection
{
    public GraphNode FirstNode { get; }
    public GraphNode SecondNode { get; }

    public GraphNodeConnection(GraphNode firstNode, GraphNode secondNode)
    {
        FirstNode = firstNode;
        SecondNode = secondNode;
    }
}
