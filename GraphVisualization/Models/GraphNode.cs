﻿using System.ComponentModel;
using System.Drawing;
using System.Windows.Media;

namespace GraphVisualization.Models;

public class GraphNode : INotifyPropertyChanged
{
    public int X
    {
        get { return _x; }
        set
        {
            _x = value;
            OnPropertyChanged(nameof(X));
        }
    }

    public int Y
    {
        get { return _y; }
        set
        {
            _y = value;
            OnPropertyChanged(nameof(Y));
        }
    }

    public float VelocityX
    {
        get { return _velocityX; }
        set { _velocityX = value; }
    }

    public float VelocityY
    {
        get { return _velocityY; }
        set { _velocityY = value; }
    }

    public Brush NodeColor
    {
        get { return _color; }
        set
        {
            _color = value;
            OnPropertyChanged(nameof(NodeColor));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private Brush _color = new SolidColorBrush(Colors.AliceBlue);
    private int _x = 0;
    private int _y = 0;
    private float _velocityX;
    private float _velocityY;

}

