﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EP_HSRlearnIT.BusinessLayer.UniversalTools
{
    /// <summary>
    /// Class to service methods to create a list of objects from a xaml file.
    /// </summary>
    public static class DependencyObjectExtension
    {

        #region Public Methods
        /// <summary>
        /// Method creates a list of all xmal objects from a page.
        /// </summary>
        /// <param name="root">Root is the reference to the page.</param>
        /// <returns>Returns a list of all xmal objects from root.</returns>
        public static IEnumerable<DependencyObject> GetAllChildren(DependencyObject root)
        {
            if (root == null)
            {
                yield break;
            }

            yield return root;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
            {
                var child = VisualTreeHelper.GetChild(root, i);

                foreach (var subChild in GetAllChildren(child))
                {
                    yield return subChild;
                }
            }
        }

        /// <summary>
        /// Method creates a list with the xamal objects from a explizit type.
        /// </summary>
        /// <typeparam name="T">Type to searching the object list.</typeparam>
        /// <param name="root">Is the reference to the page.</param>
        /// <returns>Returns a list of all xmal objects from type T from root.</returns>
        public static IEnumerable<T> GetAllChildren<T>(DependencyObject root) where T : class
        {
            foreach (var element in GetAllChildren(root))
            {
                if (element is T)
                {
                    yield return element as T;
                }
            }
        }

        /// <summary>
        /// Method returns the Page of an element.
        /// </summary>
        /// <param name="element">The element for which the parent Page is searching.</param>
        /// <returns>The Page in which the element is existing.</returns>
        public static DependencyObject GetParentPage(DependencyObject element)
        {
            var parent = VisualTreeHelper.GetParent(element);
            while (!(parent is Page))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent;
        }

        #endregion
    }
}