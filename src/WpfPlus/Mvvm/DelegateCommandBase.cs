using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Windows.Input;

namespace WpfPlus.Mvvm
{
    /// <summary>
    /// Base class for implementations of <see cref="ICommand"/> that use delegates.
    /// </summary>
    /// <seealso cref="System.Windows.Input.ICommand" />
    public abstract class DelegateCommandBase : ICommand
    {
        private SynchronizationContext? _synchronizationContext;

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler? CanExecuteChanged
        {
            add { _canExecuteChanged += value; }
            remove { _canExecuteChanged -= value; }
        }

        private event EventHandler? _canExecuteChanged;

        protected DelegateCommandBase()
        {
            _synchronizationContext = SynchronizationContext.Current;
        }

        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        bool ICommand.CanExecute(object? parameter) => CanExecute(parameter);

        void ICommand.Execute(object? parameter) => Execute(parameter);

        protected abstract bool CanExecute(object? parameter);

        protected abstract void Execute(object? parameter);

        protected virtual void OnCanExecuteChanged()
        {
            if (_canExecuteChanged != null)
            {
                if (_synchronizationContext != null && _synchronizationContext != SynchronizationContext.Current)
                    _synchronizationContext.Post((o) => _canExecuteChanged.Invoke(this, EventArgs.Empty), null);
                else
                    _canExecuteChanged.Invoke(this, EventArgs.Empty);
            }
        }
    }
}