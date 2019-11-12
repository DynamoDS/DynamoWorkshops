using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace RapidFire
{
    public static class FireUtilities
    {
        /// <summary>
        /// Finds a Child of a given item in the visual tree. 
        /// Stolen directly from dynamo 
        /// </summary>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="childName">x:Name or Name of child. </param>
        /// <returns>The first parent item that matches the submitted type parameter. 
        /// If not matching item can be found, 
        /// a null parent is being returned.</returns>
        public static T ChildOfType<T>(this DependencyObject parent, string childName = null)
           where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            if (childName != null)
            {
                return parent.ChildrenOfType<T>()
                    .FirstOrDefault(x =>
                    {
                        var xf = x as FrameworkElement;
                        if (xf == null) return false;
                        return xf.Name == childName;
                    });
            }

            return parent.ChildrenOfType<T>().FirstOrDefault();
        }

        public static IEnumerable<T> ChildrenOfType<T>(this DependencyObject parent)
          where T : DependencyObject
        {
            foreach (var child in parent.Children())
            {
                var childType = child as T;
                if (childType == null)
                {
                    foreach (var ele in ChildrenOfType<T>(child)) yield return ele;
                }
                else
                {
                    yield return childType;
                }
            }
        }

        public static IEnumerable<DependencyObject> Children(this DependencyObject parent)
        {
            var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < childrenCount; i++)
                yield return VisualTreeHelper.GetChild(parent, i);
        }
    }
}

