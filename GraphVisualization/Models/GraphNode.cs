using System.ComponentModel;

namespace GraphVisualization.Models;

public class GraphNode : INotifyPropertyChanged
{
    public int X
    {
        get { return _x; }
        set
        {
            if (_x != value)
            {
                _x = value;
                OnPropertyChanged(nameof(X));
            }
        }
    }

    public int Y
    {
        get { return _y; }
        set
        {
            if (_y != value)
            {
                _y = value;
                OnPropertyChanged(nameof(Y));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private int _x;
    private int _y;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
