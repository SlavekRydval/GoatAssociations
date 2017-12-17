namespace GoatAssociations.Commands
{
    using System;

    class RelayCommandWithResult<T> : GalaSoft.MvvmLight.Command.RelayCommand
    {
        public RelayCommandWithResult(Action execute, Func<bool> canExecute) : base(execute, canExecute) { }
        public RelayCommandWithResult(Action execute) : this(execute, null) { }

        public T Result { get; set; }
    }

    class RelayCommandWithResult<S, T> : GalaSoft.MvvmLight.Command.RelayCommand<S>
    {
        public RelayCommandWithResult(Action<S> execute, Func<S, bool> canExecute) : base(execute, canExecute) { }
        public RelayCommandWithResult(Action<S> execute) : base(execute, null) { }

        public T Result { get; set; }
    }


}
