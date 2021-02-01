using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace WpfPlus.Mvvm
{
    /// <summary>
    /// Base class for MVVM view models, which implements the <see cref="INotifyPropertyChanged"/> interface.
    /// Additionally, there is tracking mechanism for changed properties.
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Contains, if <see cref="DirtyPropertyTrackingEnabled"/> is <c>true</c>,
        /// the names of all properties, that has been marked as changed via
        /// the <see cref="RaisePropertyChanged(string, bool)"/> or the
        /// <see cref="RaiseAllPropertiesChanged(bool, string[])"> method and
        /// as long as the property is not listed in <see cref="NonTrackedProperties"/>.
        /// </summary>
        protected HashSet<string> DirtyProperties = new HashSet<string>();

        /// <summary>
        /// The names of the properties, which are ignored the dirty tracking
        /// mechanism (see <see cref="DirtyPropertyTrackingEnabled"/>), if ti is enabled.
        /// </summary>
        protected HashSet<string> NonTrackedProperties = new HashSet<string>();


        protected Func<string, bool> IsValidPropertyName { get; }

        private HashSet<string> _reflectedPropertyNames;

        /// <summary>
        /// Gets a value indicating whether this instance has changes (is dirty).
        ///
        /// This is is influenced, among other things, by <see cref="RaisePropertyChanged"/>
        /// </summary>
        public bool HasChanges { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this view model keeps a log
        /// of the property names, which are provided to the <see cref="RaisePropertyChanged(string, bool)"/> method
        /// (or <see cref="RaiseAllPropertiesChanged(bool, string[])"/> likewise)
        /// <para>
        /// By default, this mechanism is disabled.
        /// </para>
        /// </summary>
        protected bool DirtyPropertyTrackingEnabled { get; private set; } = false;

        /// <summary>
        /// Occurs when the value of <see cref="HasChanges"/> changes.
        /// </summary>
        public event EventHandler? HasChangesChanged
        {
            add { _hasChangesChanged += value; }
            remove { _hasChangesChanged -= value; }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged
        {
            add { _propertyChanged += value; }
            remove { _propertyChanged -= value; }
        }
        private event EventHandler? _hasChangesChanged;

        private event PropertyChangedEventHandler? _propertyChanged;

        protected ViewModelBase(PropertyNameValidationType propertyNameValidation)
        {
            var shallThrowExceptions = (propertyNameValidation & PropertyNameValidationType.ThrowExceptions) == PropertyNameValidationType.ThrowExceptions;

            if ((propertyNameValidation & PropertyNameValidationType.UseReflection) == 0)
            {
                _reflectedPropertyNames = new HashSet<string>();
                IsValidPropertyName = _ => true;
            }
            else
            {
                BindingFlags bf = (propertyNameValidation & PropertyNameValidationType.AcceptNonPublic) == PropertyNameValidationType.AcceptNonPublic
                    ? BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public
                    : BindingFlags.Instance | BindingFlags.Public;
                var props = GetType().GetProperties(bf);
                _reflectedPropertyNames = props.Select(p => p.Name).Distinct().ToHashSet();
                var acceptEmpty = (propertyNameValidation & PropertyNameValidationType.AcceptEmptyName) == PropertyNameValidationType.AcceptEmptyName;

                IsValidPropertyName = name => validIfNameReflected(name, shallThrowExceptions, acceptEmpty);
            }
        }

        /// <summary>
        /// Sets the <see cref="HasChanges" property to <c>false</c>./>
        /// </summary>
        public void ResetChanged()
        {
            HasChanges = false;
            raiseHasChangedChanged();
        }

        /// <summary>
        /// Sets the <see cref="HasChanges" property to <c>true</c>./>
        /// </summary>
        public void SetChanged()
        {
            HasChanges = true;
            raiseHasChangedChanged();
        }

        protected void DisablePropertyTracking()
        {
            if (!DirtyPropertyTrackingEnabled)
                return;
            DirtyPropertyTrackingEnabled = false;
            DirtyProperties = new HashSet<string>();
        }

        protected void EnablePropertyTracking()
        {
            DirtyPropertyTrackingEnabled = true;
        }
        /// <summary>
        /// Raises the <see cref="PropertyChanged" /> event for several named properties.
        /// </summary>
        /// <param name="setChangedFlag">>if set to <c>true</c>, the method will set the \
        ///     <see cref="HasChanges"/> property to <c>true</c>.</param>
        /// <param name="propertyNames">The names of the properties that have changed..</param>
        protected void RaiseAllPropertiesChanged(bool setChangedFlag, params string[] propertyNames)
        {
            // unrolled, to put this check outside of the loop (minimal performance hit)
            if (_propertyChanged != null && propertyNames != null)
            {
                foreach (var propertyName in propertyNames.Where(IsValidPropertyName))
                {
                    invokePropertyChanged(propertyName);
                }
            }

            if (setChangedFlag)
            {
                SetChanged();
            }
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event unspecifically,
        /// means without specifying a certain property.
        /// </summary>
        protected void RaisePropertyChanged()
        {
            RaisePropertyChanged(string.Empty);
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged" /> event for a certain property.
        /// </summary>
        /// <param name="propertyName">Name of the property that has changed.</param>
        /// <param name="setChangedFlag">if set to <c>true</c>, the method will set the \
        ///     <see cref="HasChanges"/> property to <c>true</c>.</param>
        protected void RaisePropertyChanged(string propertyName, bool setChangedFlag = true)
        {
            if (IsValidPropertyName(propertyName) && _propertyChanged != null)
            {
                invokePropertyChanged(propertyName);
            }
            if (setChangedFlag)
            {
                SetChanged();
            }
        }


        private void invokePropertyChanged(string propertyName)
        {
            _propertyChanged!(this, new PropertyChangedEventArgs(propertyName));
            if (DirtyPropertyTrackingEnabled && !NonTrackedProperties.Contains(propertyName))
            {
                DirtyProperties.Add(propertyName);
            }
        }

        private void raiseHasChangedChanged() => _hasChangesChanged?.Invoke(this, EventArgs.Empty);

        private bool validIfNameReflected(string name, bool throwIfInvalid, bool allowEmpty)
        {
            var valid = name is string str && (_reflectedPropertyNames.Contains(str) || str.Length == 0 && allowEmpty);
            if (throwIfInvalid && !valid)
            {
                throw new ArgumentException($"Property '{name}' does not exist.");
            }
            return valid;
        }


        [Flags]
        protected enum PropertyNameValidationType
        {
            NoValidation = 0,

            UseReflection = 1,

            ThrowExceptions = 2,

            AcceptNonPublic = 4,

            AcceptEmptyName = 8
        }
    }
}