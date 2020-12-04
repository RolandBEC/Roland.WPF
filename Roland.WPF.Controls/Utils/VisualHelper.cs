using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Roland.WPF.Controls.Utils
{
    public static class VisualHelper
    {
        /// <summary>
        ///     Find child by type of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="visual"></param>
        /// <returns></returns>
        public static T FindVisualChild<T>(Visual visual) where T : Visual
        {
            for(int i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
            {
                Visual _child = (Visual)VisualTreeHelper.GetChild(visual, i);
                if(_child == null)
                {
                    continue;
                }

                if (_child is T correctlyTyped)
                {
                    return correctlyTyped;
                }

                T descendent = FindVisualChild<T>(_child);
                if (descendent != null)
                {
                    return descendent;
                }
            }
            return null;
        }

        /// <summary>
        ///     This method is an alternative to WPF's <see cref="VisualTreeHelper.GetParent"/> method, which also support
        ///     content elements. Do not that for content element, this method falls back to the logical tree of the element
        /// </summary>
        /// <param name="child">The item to be prcessed</param>
        /// <returns>
        ///     The submitted item's parent if available. Otherwise null.
        /// </returns>
        public static DependencyObject GetParentObject(DependencyObject child)
        {
            if(child == null)
            {
                return null;
            }

            if (!(child is ContentElement _contentElement))
            {
                // If it's not a ContentElement let's rely on VisualTreeHelper
                return VisualTreeHelper.GetParent(child);
            }

            DependencyObject _parent = ContentOperations.GetParent(_contentElement);
            if(_parent != null)
            {
                return _parent;
            }

            FrameworkContentElement _fce = _contentElement as FrameworkContentElement;
            return _fce?.Parent;
        }

        /// <summary>
        ///     Tries to locate a given item within the visual tree,
        ///     starting with the dependency object at a given position
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the element to be found
        ///     on the visual tree of the element at the given location
        /// </typeparam>
        /// <param name="reference">
        ///     The main element which is used to perform
        /// </param>
        /// <param name="point">The position to be evaluated on the origin</param>
        /// <returns></returns>
        public static T TryFindFromPoint<T>(UIElement reference, Point point) where T : DependencyObject
        {
            if (!(reference?.InputHitTest(point) is DependencyObject _element))
            {
                return null;
            }
            if (_element is T _point)
            {
                return _point;
            }
            return TryFindParent<T>(_element);
        }

        /// <summary>
        ///     Finds a parent of a given item on the visual tree
        /// </summary>
        /// <typeparam name="T">The type of the queried item</typeparam>
        /// <param name="child">
        ///     A direct or indirect child of the tree
        /// </param>
        /// <returns>
        ///     The first parent item that matches the T parameter. If no matching item can be found, a null reference is being returned.
        /// </returns>
        public static T TryFindParent<T>(DependencyObject child) where T : DependencyObject
        {
            // get parent item
            DependencyObject _parentObject = GetParentObject(child);

            if(_parentObject == null)
            {
                // the item have no parent
                return null;
            }

            // check if the parent mathes the type we're looking for
            T _parent = _parentObject as T;
            return _parent ?? TryFindParent<T>(_parentObject);
        }
    }
}
