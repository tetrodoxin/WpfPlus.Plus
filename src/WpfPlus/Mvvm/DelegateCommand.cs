using System;
using System.Windows.Input;

namespace WpfPlus.Mvvm
{
    /// <summary>
    /// A delegate-based implementation of <see cref="ICommand"/> 
    /// that requires no command parameter
    /// </summary>
    /// <seealso cref="WpfPlus.Mvvm.DelegateCommandBase" />
    public class DelegateCommand : DelegateCommandBase
    {
        private Func<bool> _canExecuteDelegate;
        private Action _executeDelegate;
        
        private static Lazy<DelegateCommand> _empty = new Lazy<DelegateCommand>(() => new DelegateCommand(() => { }));

        public static DelegateCommand Nothing => _empty.Value;

        public DelegateCommand(Action actionToExecute) : this(actionToExecute, () => true)
        { }

        public DelegateCommand(Action actionToExecute, Func<bool> canExecutePredicate)
        {
            _executeDelegate = actionToExecute;
            _canExecuteDelegate = canExecutePredicate;
        }

        public void Execute() => _executeDelegate();

        protected bool CanExecute() => _canExecuteDelegate();

        protected override bool CanExecute(object? parameter) => CanExecute();

        protected override void Execute(object? parameter) => Execute();
    }
}