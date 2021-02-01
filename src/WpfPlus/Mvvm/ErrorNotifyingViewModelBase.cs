using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WpfPlus.Mvvm
{
    public abstract partial class ErrorNotifyingViewModelBase : ViewModelBase, INotifyDataErrorInfo
    {
        private Dictionary<string, string[]> _propertyErrors = new Dictionary<string, string[]>();

        public virtual bool HasErrors => _propertyErrors.Any();

        private event EventHandler<DataErrorsChangedEventArgs>? _errorsChanged;
        
        event EventHandler<DataErrorsChangedEventArgs>? INotifyDataErrorInfo.ErrorsChanged
        {
            add { _errorsChanged += value; }
            remove { _errorsChanged -= value; }
        }

        protected void RaiseErrorChanged(string propertyName)
        {
            if (IsValidPropertyName(propertyName))
            {
                raiseErrorChangedInternal(propertyName);
            }

        }

        private void raiseErrorChangedInternal(string propertyName)
        {
            try
            {
                _errorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }
            catch { }
        }

        protected void SetPropertyError(string propertyName, string error)
        {
            if(IsValidPropertyName(propertyName))
            {
                string key = propertyName ?? string.Empty;
                if (string.IsNullOrEmpty(error))
                {
                    // delete error(s) for property
                    _propertyErrors.Remove(key);
                }
                else
                {
                    if(!_propertyErrors.TryGetValue(key, out var existing) || existing.Length==0 && !existing[0].Equals(error))
                    {
                        // error is not equal to existing or doesn't exist at all, set and raise event
                        _propertyErrors[key] = new string[] { error };
                        raiseErrorChangedInternal(key);
                    }
                }
            }
        }

        protected ErrorNotifyingViewModelBase(PropertyNameValidationType propertyNameValidation) : base(propertyNameValidation)
        {
        }

        IEnumerable INotifyDataErrorInfo.GetErrors(string? propertyName)
        {
            return _propertyErrors.TryGetValue(propertyName ?? string.Empty, out var exist)
                ? exist.ToList().AsReadOnly()
                : new string[0];
        }
    }
}
