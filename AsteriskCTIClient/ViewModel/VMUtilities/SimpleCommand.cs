using System;
using System.Windows.Input;

namespace AsteriskCTIClient.ViewModel.VMUtilities
{
  public class SimpleCommand : ICommand
  {
    private readonly Action _action;
  
    public SimpleCommand(Action action)
    {
      _action = action;
    }

    public void Execute(object parameter)
    {
      _action();
    }

    public bool CanExecute(object parameter)
    {
      return true;
    }

    public event EventHandler CanExecuteChanged;
  }
}