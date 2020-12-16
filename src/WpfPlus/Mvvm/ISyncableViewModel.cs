using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfPlus.Mvvm
{
    /// <summary>
    /// An interface for view models, that may be synchronized by <see cref="ViewModelsCollection{TViewModel, TModel}"/>
    /// </summary>
    /// <typeparam name="TModel">The type of the underlying model.</typeparam>
    public interface ISyncableViewModel<TModel>
    {
        /// <summary>
        /// Returns the model object, this view model encapsulates.
        /// </summary>
        TModel ModelInstance { get; }
    }
}
