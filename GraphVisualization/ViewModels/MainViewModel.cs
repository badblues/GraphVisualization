using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using GraphVisualization.Commands;
using GraphVisualization.Models;
using Microsoft.Xaml.Behaviors.Layout;

namespace GraphVisualization.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    public ObservableCollection<GraphNode> Nodes { get; set; }
    public ObservableCollection<GraphNodeConnection> Connections { get; set; }
    public Boolean IsConnecting { get; set; }
    public ICommand AddNodeCommand { get; init; }
    public ICommand StartDragCommand { get; init; }
    public ICommand DragCommand { get; init; }
    public ICommand EndDragCommand { get; init; }
    public ICommand AllowConnectionCommand { get; init; }
    public ICommand ConnectNodeCommand { get; init; }
    public event PropertyChangedEventHandler? PropertyChanged;

    private Stopwatch stopwatch = new Stopwatch();
    private long previousFrameTime = 0;
    private GraphNode? _selectedNode = null;

    public MainViewModel()
    {
        Nodes = NodeManager.Nodes;
        Connections = NodeManager.Connections;
        AddNodeCommand = new RelayCommand(ExecuteAddNode, CanAddNode);
        StartDragCommand = new RelayCommand(ExecuteStartDrag, CanStartDrag);
        DragCommand = new RelayCommand(ExecuteDrag, CanDrag);
        EndDragCommand = new RelayCommand(ExecuteEndDrag, CanEndDrag);
        AllowConnectionCommand = new RelayCommand(ExecuteAllowConnection, CanAllowConnection);
        ConnectNodeCommand = new RelayCommand(ExecuteConnectNode, CanConnectNode);
        _selectedNode = null;
        IsConnecting = false;
        CompositionTarget.Rendering += UpdateNodeVelocities;
        Nodes.Add(new GraphNode { X = 15, Y = 45});
        Nodes.Add(new GraphNode { X = 100, Y = 255 });
    }

    private void UpdateNodeVelocities(Object? sender, EventArgs e)
    {
        if (!stopwatch.IsRunning)
            stopwatch.Start();
        long currentFrameTime = stopwatch.ElapsedMilliseconds;
        float dt = (currentFrameTime - previousFrameTime) / 1000.0f;
        if (dt < 0.1)
        {
            UpdateNodePositions(dt);
            return;
        }
        previousFrameTime = currentFrameTime;
        foreach (var node in Nodes)
        {
            node.VelocityX = 0;
            node.VelocityY = 0;
            foreach (var otherNode in Nodes) //Calculating pushing forces
                if (node != otherNode)
                {
                    // closer nodes are = more acceleration
                    float deltaX = node.X - otherNode.X;
                    float deltaY = node.Y - otherNode.Y;

                    float distance = MathF.Sqrt(deltaX * deltaX + deltaY * deltaY);

                    float G = 100000;

                    float speed = G / (distance * distance);

                    node.VelocityX += speed * deltaX / distance;
                    node.VelocityY += speed * deltaY / distance;
                }
        }
        foreach(var connection in Connections) //Calculating pulling forces
        {
            var node = connection.FirstNode;
            var otherNode = connection.SecondNode;

            float deltaX = node.X - otherNode.X;
            float deltaY = node.Y - otherNode.Y;

            float distance = MathF.Sqrt(deltaX * deltaX + deltaY * deltaY);

            float G = 10;

            float speed = distance / G;

            node.VelocityX -= speed * deltaX / distance;
            node.VelocityY -= speed * deltaY / distance;
            otherNode.VelocityX += speed * deltaX / distance;
            otherNode.VelocityY += speed * deltaY / distance;


        }
        UpdateNodePositions(dt);
    }

    private void UpdateNodePositions(float dt)
    {
        foreach (var node in Nodes)
        {
            node.X += (int)(node.VelocityX * dt);
            node.Y += (int)(node.VelocityY * dt);
        }
    }

    private bool CanAddNode(object parameter)
    {
        return true;
    }

    private void ExecuteAddNode(object parameter)
    {
        Debug.WriteLine("Adding node");
        Random random = new Random();
        Nodes.Add(new GraphNode { X = 250 + random.Next(100) - 50, Y = 250 + random.Next(100) - 50 });
    }

    private void ExecuteStartDrag(object parameter)
    {
        if (parameter is FrameworkElement element)
        {
            var node = element.DataContext as GraphNode;
            if (node != null)
            {
                _selectedNode = node;
            }
        }
    }

    private bool CanStartDrag(object parameter)
    {
        return _selectedNode is null;
    }

    private void ExecuteDrag(object parameter)
    {
        if (_selectedNode != null && parameter is MouseEventArgs e)
        {
            //TODO: hardcoded values
            _selectedNode.X = (int)e.GetPosition(null).X - 15;
            _selectedNode.Y = (int)e.GetPosition(null).Y - 15;
            _selectedNode.VelocityX = 0;
            _selectedNode.VelocityY = 0;
        }
    }

    private bool CanDrag(object parameter)
    {
        return _selectedNode != null;
    }

    private void ExecuteEndDrag(object parameter)
    {
        _selectedNode = null;
    }

    private bool CanEndDrag(object parameter)
    {
        return _selectedNode != null;
    }

    private void ExecuteAllowConnection(object parameter)
    {
        IsConnecting = true;
        OnPropertyChanged(nameof(IsConnecting));
    }


    private bool CanAllowConnection(object parameter)
    {
        return true;
    }

    private void ExecuteConnectNode(object parameter)
    {
        if (parameter is FrameworkElement element)
        {
            var node = element.DataContext as GraphNode;
            if (node == null)
                return;
            if (_selectedNode == null)
                _selectedNode = node;
            else
            {
                Connections.Add(new GraphNodeConnection(_selectedNode, node));
                _selectedNode = null;
                IsConnecting = false;
                OnPropertyChanged(nameof(IsConnecting));
            }
        }
    }

    private bool CanConnectNode(object parameter)
    {
        return IsConnecting;
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}