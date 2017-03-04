using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoatAssociations.Commands
{
    class RelayCommandWithResult<T>: RelayCommand
    {
        public RelayCommandWithResult(Action execute, Func<bool> canExecute) : base(execute, canExecute)
        {
        }

        T Result {get; set; }
    }


    class RelayCommandWithResult<S, T> : RelayCommand<S>
    {
        public RelayCommandWithResult(Action<S> execute, Predicate<S> canExecute) : base(execute, canExecute) { }
        public RelayCommandWithResult(Action<S> execute) : base(execute, null) { }

        public T Result { get; set; }
    }


}
