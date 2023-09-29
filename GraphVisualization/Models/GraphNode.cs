using System.ComponentModel;
using System.Reflection;

namespace GraphVisualization.Models;

public class GraphNode : INotifyPropertyChanged
{
    private int x;
    public int X {
        get { return x; }
        set {
            if (x!= value)
            {
                x = value;
                OnPropertyChanged(nameof(X));
            }        
        }
    }
    private int y;
    public int Y
    {
        get { return y; }
        set
        {
            if (y != value)
            {
                y = value;
                OnPropertyChanged(nameof(y));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
