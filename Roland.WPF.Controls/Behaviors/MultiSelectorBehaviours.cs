using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Roland.WPF.Controls.Behaviors
{
    /// <summary>
    ///     A sync behaviour for a multiselector.
    /// </summary>
    public static class MultiSelectorBehaviours
    {
        #region Constants, Fields, Properties, Indexers

        #region Fields, Constants
        public static readonly DependencyProperty SynchronizedSelectedItems =
            DependencyProperty.RegisterAttached(
                "SynchronizedSelectedItems",
                typeof(IList),
                typeof(MultiSelectorBehaviours),
                new PropertyMetadata(null, OnSynchronizedSelectedItemsChanged));

        private static readonly DependencyProperty SynchronizationManagerProperty =
            DependencyProperty.RegisterAttached(
                "SynchronizationManager",
                typeof(SynchronizationManager),
                typeof(MultiSelectorBehaviours),
                new PropertyMetadata(null));
        #endregion

        #endregion

        #region Methods
        /// <summary>
        ///     Gets the synchronized selected items.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <returns>The list that is acting as the sync list.</returns>
        public static IList GetSynchronizedSelectedItems(DependencyObject dependencyObject)
        {
            return (IList)dependencyObject.GetValue(SynchronizedSelectedItems);
        }

        /// <summary>
        ///     Sets the synchronized selected items.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="value">The value to be set as synchronized items.</param>
        public static void SetSynchronizedSelectedItems(DependencyObject dependencyObject, IList value)
        {
            dependencyObject.SetValue(SynchronizedSelectedItems, value);
        }

        private static SynchronizationManager GetSynchronizationManager(DependencyObject dependencyObject)
        {
            return (SynchronizationManager)dependencyObject.GetValue(SynchronizationManagerProperty);
        }

        private static void OnSynchronizedSelectedItemsChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                SynchronizationManager synchronizer = GetSynchronizationManager(dependencyObject);
                synchronizer.StopSynchronizing();

                SetSynchronizationManager(dependencyObject, null);
            }

            IList list = e.NewValue as IList;
            Selector selector = dependencyObject as Selector;

            // check that this property is an IList, and that it is being set on a ListBox
            if (list != null && selector != null)
            {
                SynchronizationManager synchronizer = GetSynchronizationManager(dependencyObject);
                if (synchronizer == null)
                {
                    synchronizer = new SynchronizationManager(selector);
                    SetSynchronizationManager(dependencyObject, synchronizer);
                }

                synchronizer.StartSynchronizingList();
            }
        }

        private static void SetSynchronizationManager(DependencyObject dependencyObject, SynchronizationManager value)
        {
            dependencyObject.SetValue(SynchronizationManagerProperty, value);
        }
        #endregion

        #region Nested type: SynchronizationManager
        /// <summary>
        ///     A synchronization manager.
        /// </summary>
        private sealed class SynchronizationManager
        {
            #region Constants, Fields, Properties, Indexers

            #region Fields, Constants
            private readonly Selector _multiSelector;
            private TwoListSynchronizer _synchronizer;
            #endregion

            #endregion

            /// <summary>
            ///     Initializes a new instance of the <see cref="SynchronizationManager" /> class.
            /// </summary>
            /// <param name="selector">The selector.</param>
            internal SynchronizationManager(Selector selector)
            {
                this._multiSelector = selector;
            }

            #region Methods
            /// <summary>
            ///     Starts synchronizing the list.
            /// </summary>
            public void StartSynchronizingList()
            {
                IList list = GetSynchronizedSelectedItems(this._multiSelector);

                if (list != null)
                {
                    this._synchronizer = new TwoListSynchronizer(GetSelectedItemsCollection(this._multiSelector), list);
                    this._synchronizer.StartSynchronizing();
                }
            }

            /// <summary>
            ///     Stops synchronizing the list.
            /// </summary>
            public void StopSynchronizing()
            {
                this._synchronizer.StopSynchronizing();
            }

            private static IList GetSelectedItemsCollection(Selector selector)
            {
                MultiSelector _selector = selector as MultiSelector;
                if (_selector != null)
                {
                    return _selector.SelectedItems;
                }

                ListBox _box = selector as ListBox;
                if (_box != null)
                {
                    return _box.SelectedItems;
                }

                throw new InvalidOperationException("Target object has no SelectedItems property to bind.");
            }
            #endregion
        }
        #endregion
    }
}
