using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace GraphVisualization.Models;

public class NodeManager
{
    private ObservableCollection<GraphNode> _nodes = new ObservableCollection<GraphNode> { };
    private ObservableCollection<GraphNodeConnection> _connections = new ObservableCollection<GraphNodeConnection> { };
    private float _repellingForceCoefficient = 1.0f;
    private float _pullingForceCoefficient = 1.0f;

    public ObservableCollection<GraphNode> Nodes
    {
        get { return _nodes; }
    }

    public ObservableCollection<GraphNodeConnection> Connections
    {
        get { return _connections; }
    }

    public float RepellingForceCoefficient
    { 
        get { return _repellingForceCoefficient; }
        set { _repellingForceCoefficient = value; }
    }

    public float PullingForceCoefficient
    {
        get { return _pullingForceCoefficient; }
        set { _pullingForceCoefficient = value; }
    }

    public void Clear()
    {
        _nodes.Clear();
        _connections.Clear();
    }

    public void UpdateNodeVelocities(float dt)
    {
        foreach (var node in _nodes)
        {
            node.VelocityX = 0;
            node.VelocityY = 0;
            foreach (var otherNode in _nodes) // Calculating pushing forces
                if (node != otherNode)
                {
                    // Сloser nodes are => more acceleration
                    float deltaX = node.X - otherNode.X;
                    float deltaY = node.Y - otherNode.Y;

                    float distance = MathF.Sqrt(deltaX * deltaX + deltaY * deltaY);
                    float G = 150000;
                    float speed = G / (distance * distance);

                    node.VelocityX += (speed * deltaX / distance) * _repellingForceCoefficient;
                    node.VelocityY += (speed * deltaY / distance) * _repellingForceCoefficient;
                }
        }
        foreach (var connection in _connections) // Calculating pulling forces
        {
            var node = connection.FirstNode;
            var otherNode = connection.SecondNode;

            float deltaX = node.X - otherNode.X;
            float deltaY = node.Y - otherNode.Y;

            float distance = MathF.Sqrt(deltaX * deltaX + deltaY * deltaY);
            if (distance > 1e-1)
            {
                float G = 10;
                float speed = distance / G;
                node.VelocityX -= (speed * deltaX / distance) * _pullingForceCoefficient;
                node.VelocityY -= (speed * deltaY / distance) * _pullingForceCoefficient;
                otherNode.VelocityX += (speed * deltaX / distance) * _pullingForceCoefficient;
                otherNode.VelocityY += (speed * deltaY / distance) * _pullingForceCoefficient;
            }
        }
        UpdateNodePositions(dt);
    }

    private void UpdateNodePositions(float dt)
    {
        foreach (var node in _nodes)
        {
            int dx = (int)(node.VelocityX * dt);
            int dy = (int)(node.VelocityY * dt);
            if (node.X + dx > 0 && node.X + dx < 1080) // hardcode values for window size :\
                node.X += dx;
            if (node.Y + dy > 0 && node.Y + dy < 685)
                node.Y += dy;
        }
    }

}
