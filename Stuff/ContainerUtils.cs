using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff
{
    public static class ContainerUtils
    {
        public static LinkedList<TSource> GetRange<TSource>(this LinkedList<TSource> list, int index, int count)
        {
            LinkedList<TSource> result = new LinkedList<TSource>();
            for (int i = index; i < index + count; i++)
                result.AddLast(list.ElementAt(i));
            return result;
        }

        public static int IndexOf<TSource>(this IEnumerable<TSource> list, TSource value)
        {
            for (int i = 0; i < list.Count(); i++)
            {
                if (list.ElementAt(i).Equals(value))
                    return i;
            }
            throw new Exception("value not in IEnumerable");
        }

        /// <summary>
        /// Returns the first element in the collection that matches the selector.
        /// Throws an exception if no such element is found.
        /// </summary>
        /// <returns>The first element in the collection that matches the selector.</returns>
        public static TSource SelectFirst<TSource>(this IEnumerable<TSource> list, Func<TSource, bool> selector)
        {
            foreach(var element in list)
            {
                if (selector.Invoke(element))
                    return element;
            }
            throw new Exception("No element found matching selector");
        }

        public static IEnumerable<T> Reverse<T>(this LinkedList<T> list)
        {
            var el = list.Last;
            while (el != null)
            {
                yield return el.Value;
                el = el.Previous;
            }
        }

        public static void RemoveAll<T>(this List<T> list, List<T> remover)
        {
            list.RemoveAll((T t) => { return remover.Contains(t); });
        }

        public static T[] Append<T>(this T[] firstArray, params T[] secondArray)
        {
            LinkedList<T> result = new LinkedList<T>();
            foreach (T t in firstArray)
                result.AddLast(t);
            foreach (T t in secondArray)
                result.AddLast(t);
            return result.ToArray();
        }

        public static LinkedList<T> Append<T>(this LinkedList<T> list1, IEnumerable<T> list2)
        {
            foreach (T t in list2)
                list1.AddLast(t);
            return list1;
        }

        public static List<T> Append<T>(this List<T> list1, IEnumerable<T> list2)
        {
            foreach (T t in list2)
                list1.Add(t);
            return list1;
        }

        public static void Print<T>(this IEnumerable<T> list, string prefix = "")
        {
            foreach (T t in list)
                Console.WriteLine(prefix + t);
        }

        public static T Random<T>(this IEnumerable<T> list)
        {
            return list.ElementAt(Rand.Next(list.Count()));
        }

        public static string AsString<T>(this char[] charArray)
        {
            string result = "";
            foreach (char c in charArray)
                result += c;
            return result;
        }

        /// <summary>Like Max(), but returns the element instead of the value.</summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="list"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static T MaxValue<T, TKey>(this IEnumerable<T> list, Func<T, TKey> keySelector)
        {
            return list.OrderBy(keySelector).Last();
        }

        public static T MinValue<T, TKey>(this IEnumerable<T> list, Func<T, TKey> keySelector)
        {
            return list.OrderBy(keySelector).First();
        }

        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int j = Rand.Next(i + 1);
                list.Swap(j, i);
            }
            return list;
        }
        public static IList<T> Swap<T>(this IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
            return list;
        }

        public static Dictionary<TKey, TValue> Copy<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            var result = new Dictionary<TKey, TValue>();
            foreach (var element in dict)
                result[element.Key] = element.Value;
            return result;
        }

        public static List<T> Copy<T>(this List<T> list)
        {
            var result = new List<T>(list.Count);
            foreach (var t in list)
                result.Add(t);
            return result;
        }

        public static T[] Copy<T>(this T[] list)
        {
            var result = new T[list.Length];
            list.CopyTo(result, 0);
            return result;
        }

        public static string AsString<T>(this IEnumerable<T> list)
        {
            if (list.Count() == 0)
                return "[]";
            string result = "[";
            foreach (T value in list)
            {
                result += value + ",";
            }
            return result.TrimEnd(',') + "]";
        }

        public static string AsString<T>(this T[] list)
        {
            string result = "{";
            foreach (T value in list)
            {
                result += value + ",";
            }
            return result.TrimEnd(',') + "}";
        }

        public static string AsString<T>(LinkedListNode<T> start, LinkedListNode<T> end)
        {
            string result = "{";
            var node = start;
            while (node != end.Next)
            {
                result += node.Value + ",";
                node = node.Next;
            }
            return result.TrimEnd(',') + "}";
        }
    }
}
