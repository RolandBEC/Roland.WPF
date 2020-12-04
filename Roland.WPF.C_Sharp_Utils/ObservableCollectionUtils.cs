using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roland.WPF.C_Sharp_Utils
{
    public static class ObservableCollectionUtils
    {

        public static void AddRange<TSource>(this ObservableCollection<TSource> list, IEnumerable<TSource> values, IEqualityComparer<TSource> comparer)
        {
            Debug.Assert(list != null);
            if (values == null || (values.Count() == 0))
            {
                return;
            }

            foreach (TSource value in values)
            {
                if (!list.Contains(value, comparer))
                {
                    list.Add(value);
                }
            }
        }

        public static void RemoveRange<TSource>(this ObservableCollection<TSource> list, IEnumerable<TSource> values, IEqualityComparer<TSource> comparer)
        {
            Debug.Assert(list != null);
            if (values == null || (values.Count() == 0))
            {
                return;
            }

            foreach (TSource value in values)
            {
                if (list.Contains(value, comparer))
                {
                    list.Remove(value, comparer);
                }
            }
        }

        public static void Remove<TSource>(this ObservableCollection<TSource> list, TSource value, IEqualityComparer<TSource> comparer)
        {
            Debug.Assert(list != null);
            int index = list.IndexOf(value, comparer);
            if (index >= 0 && index < list.Count)
            {
                list.RemoveAt(index);
            }
        }

        public static int IndexOf<TSource>(this ObservableCollection<TSource> list, TSource value, IEqualityComparer<TSource> comparer)
        {
            Debug.Assert(list != null);
            int index = 0;
            foreach (TSource item in list)
            {
                if (comparer.Equals(item, value))
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        public static void Sort<TSource, TKey>(this ObservableCollection<TSource> source, Func<TSource, TKey> keySelector)
        {
            IList<TSource> sortedList = source.OrderBy(keySelector).ToList();
            Update(source, sortedList);
        }

        public static void Update<TSource>(this ObservableCollection<TSource> source, IList<TSource> newItems)
        {
            source.Clear();
            foreach (TSource newItem in newItems)
            {
                source.Add(newItem);
            }
        }

    }
}
