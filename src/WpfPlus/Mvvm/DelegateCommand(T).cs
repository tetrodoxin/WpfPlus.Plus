using System;
using System.Windows.Input;

namespace WpfPlus.Mvvm
{
    /// <summary>
    /// A generic implementation of <see cref="ICommand"/> using delegates.
    /// </summary>
    /// <seealso cref="WpfPlus.Mvvm.DelegateCommandBase" />
    public class DelegateCommand<TParam> : DelegateCommandBase
    {
        private Func<TParam?, bool> _canExecuteDelegate;
        private Action<TParam?> _executeDelegate;

        private static Lazy<DelegateCommand<TParam?>> _empty = new Lazy<DelegateCommand<TParam?>>(() => new DelegateCommand<TParam?>(p => { }));

        public static DelegateCommand<TParam?> Nothing => _empty.Value;


        public DelegateCommand(Action<TParam?> actionToExecute) : this(actionToExecute, p => true)
        { }

        public DelegateCommand(Action<TParam?> actionToExecute, Func<TParam?, bool> canExecutePredicate)
        {
            _executeDelegate = actionToExecute;
            _canExecuteDelegate = canExecutePredicate;
        }

        public void Execute(TParam? parameter) => _executeDelegate(parameter);

        protected bool CanExecute(TParam? parameter) => _canExecuteDelegate(parameter);

        protected override bool CanExecute(object? parameter) => CanExecute((TParam)parameter);

        protected override void Execute(object? parameter) => Execute((TParam)parameter);
    }
}