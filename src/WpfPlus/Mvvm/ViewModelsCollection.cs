using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace WpfPlus.Mvvm
{
    /// <summary>
    /// Collection of view model, which is observably synced to a collection of model objects.
    /// <para>
    /// Inspired by http://stackoverflow.com/questions/15830008/mvvm-and-collections-of-vms
    /// </para>
    /// </summary>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <remarks>
    /// THIS IS NOT THREAD SAFE!
    /// </remarks>
    /// <seealso cref="System.Collections.ObjectModel.ObservableCollection{TViewModel}" />
    /// <seealso cref="http://stackoverflow.com/questions/15830008/mvvm-and-collections-of-vms"/>
    public class ViewModelsCollection<TViewModel, TModel> : ObservableCollection<TViewModel>
        where TViewModel : class, ISyncableViewModel<TModel>
        where TModel : class
    {
        private static readonly TViewModel[] NoViewModels = Array.Empty<TViewModel>();
        private static readonly TModel[] NoModels = Array.Empty<TModel>();
        
        private readonly ICollection<TModel> _itemModelCollection;
        private readonly Func<TModel, TViewModel> _viewModelFactoryFunction;
        private bool _whileSyncing = false;

        /// <summary>
        /// Occurs when an item is added, removed, changed, moved, or the entire list is refreshed.
        /// </summary>
        public sealed override event NotifyCollectionChangedEventHandler? CollectionChanged
        {
            add { base.CollectionChanged += value; }
            remove { base.CollectionChanged -= value; }
        }

        public ViewModelsCollection(ICollection<TModel> itemModelCollection, Func<TModel, TViewModel> viewModelFactoryFunction)
        {
            _itemModelCollection = itemModelCollection ?? new List<TModel>();
            _viewModelFactoryFunction = viewModelFactoryFunction;

            foreach (TModel itemModel in _itemModelCollection)
                addItemViewModelForItemModel(itemModel);

            if (_itemModelCollection is ObservableCollection<TModel> observableItemModelCollection)
            {
                observableItemModelCollection.CollectionChanged += itemModelCollectionChanged;
            }
        }

        /// <summary>
        /// Returns the view model, that encapsulates a certain model,
        /// if the collection (already) holds such a view model.
        /// </summary>
        /// <param name="modelInstance">The model object.</param>
        /// <returns>An instance of <typeparamref name="TViewModel"/> whose <see cref="ISyncableViewModel{TModel}.ModelInstance"/> 
        /// equals the instance provided in <paramref name="modelInstance"/>. If no such view model exists, the result will be <c>null</c></returns>
        public TViewModel? GetViewModelByModel(TModel modelInstance) => Items.FirstOrDefault(vm => vm.ModelInstance == modelInstance);

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs? e)
        {
            if (e is null)
                return;

            base.OnCollectionChanged(e);
            if (_whileSyncing)
                return;

            _whileSyncing = true;
            {
                try
                {
                    switch (e.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            foreach (var viewModel in e.NewItems?.OfType<TViewModel>() ?? NoViewModels)
                                _itemModelCollection.Add(viewModel.ModelInstance);
                            break;

                        case NotifyCollectionChangedAction.Remove:
                            foreach (var viewModel in e.OldItems?.OfType<TViewModel>() ?? NoViewModels)
                                _itemModelCollection.Remove(viewModel.ModelInstance);
                            break;

                        case NotifyCollectionChangedAction.Reset:
                            _itemModelCollection.Clear();
                            foreach (var viewModel in e.NewItems?.OfType<TViewModel>() ?? NoViewModels)
                                _itemModelCollection.Add(viewModel.ModelInstance);
                            break;
                    }
                }
                finally
                {
                    _whileSyncing = false;
                }
            }
        }

        private void addItemViewModelForItemModel(TModel itemModel)
        {
            if (itemModel == null)
                return;

            Add(_viewModelFactoryFunction(itemModel));
        }

        private void itemModelCollectionChanged(object? sender, NotifyCollectionChangedEventArgs? e)
        {
            if (e is null)
                return;

            if (_whileSyncing)
                return;

            _whileSyncing = true;
            {
                try
                {
                    switch (e.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            foreach (TModel itemModel in e.NewItems?.OfType<TModel>() ?? NoModels)
                                addItemViewModelForItemModel(itemModel);
                            break;

                        case NotifyCollectionChangedAction.Remove:
                            foreach (TModel itemModel in e.OldItems?.OfType<TModel>() ?? NoModels)
                                removeItemViewModelByItemModel(itemModel);
                            break;

                        case NotifyCollectionChangedAction.Reset:
                            Clear();
                            foreach (TModel itemModel in e.NewItems?.OfType<TModel>() ?? NoModels)
                                addItemViewModelForItemModel(itemModel);
                            break;
                    }
                }
                finally
                {
                    _whileSyncing = false;
                }
            }
        }

        private void removeItemViewModelByItemModel(TModel itemModel)
        {
            if (itemModel == null)
                return;

            IEnumerable<TViewModel> toDelete = Items.Where(vm => vm.ModelInstance == itemModel).ToArray();
            foreach (TViewModel viewModel in toDelete)
                Remove(viewModel);
        }
    }
}