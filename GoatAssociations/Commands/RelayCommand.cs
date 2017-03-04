using System;

using System.Windows.Input;

namespace GoatAssociations.Commands
{
    internal class RelayCommand : ICommand
    {
        private Action _execute;
        private Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute != null)
                return _canExecute();
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _execute();
        }
    }

    internal class RelayCommand<T> : ICommand
    {
        private readonly Predicate<T> _canExecute;
        private readonly Action<T> _execute;

        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
                return true;

            //return _canExecute((parameter == null) ? default(T) : (T)Convert.ChangeType(parameter, typeof(T)));
            return _canExecute((parameter == null) ? default(T) : (T)parameter);
        }

        public void Execute(object parameter)
        {
            //_execute((parameter == null) ? default(T) : (T)Convert.ChangeType(parameter, typeof(T)));
            _execute((parameter == null) ? default(T) : (T)parameter);
        }

        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }


}
