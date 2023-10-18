using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using GraphVisualization.Commands;
using GraphVisualization.Converters;
using GraphVisualization.Models;

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
    private NodeManager _nodeManager;
    private int _nodeRadius = 15;
    public NodeManager NodeManager {  get { return _nodeManager; } }
    public int NodeRadius { get { return _nodeRadius;  } set { _nodeRadius = value;  OnPropertyChanged(nameof(NodeRadius)); } }

    public MainViewModel()
    {
        _nodeManager = new NodeManager();
        Nodes = _nodeManager.Nodes;
        Connections = _nodeManager.Connections;
        AddNodeCommand = new RelayCommand(ExecuteAddNode, CanAddNode);
        StartDragCommand = new RelayCommand(ExecuteStartDrag, CanStartDrag);
        DragCommand = new RelayCommand(ExecuteDrag, CanDrag);
        EndDragCommand = new RelayCommand(ExecuteEndDrag, CanEndDrag);
        AllowConnectionCommand = new RelayCommand(ExecuteAllowConnection, CanAllowConnection);
        ConnectNodeCommand = new RelayCommand(ExecuteConnectNode, CanConnectNode);
        _selectedNode = null;
        IsConnecting = false;
        CompositionTarget.Rendering += UpdateNodePositions;
    }

    private void UpdateNodePositions(Object? sender, EventArgs e)
    {
        if (!stopwatch.IsRunning)
            stopwatch.Start();
        long currentFrameTime = stopwatch.ElapsedMilliseconds;
        float dt = (currentFrameTime - previousFrameTime) / 1000.0f;        
        _nodeManager.UpdateNodeVelocities(dt);
        previousFrameTime = currentFrameTime;
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
            _selectedNode.X = (int)e.GetPosition(null).X;
            _selectedNode.Y = (int)e.GetPosition(null).Y;
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