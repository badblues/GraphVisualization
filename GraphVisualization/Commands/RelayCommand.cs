using System;
using System.Windows.Input;

namespace GraphVisualization.Commands;

public class RelayCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;
    private Action<object> _Execute { get; set; }
    private Predicate<object> _CanExecute { get; set; }


    public RelayCommand(Action<object> executeMethod, Predicate<object> canExecuteMethod)
    {
        _Execute = executeMethod;
        _CanExecute = canExecuteMethod;
    }

    public bool CanExecute(object? parameter)
    {
        return _CanExecute(parameter);
    }

    public void Execute(object? parameter)
    {
        _Execute(parameter);
    }
}
