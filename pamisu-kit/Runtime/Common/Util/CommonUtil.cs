using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PamisuKit.Common.Util
{
    public static class CommonUtil
    {

        public static string ToString(Array array)
        {
            if (array == null)
                return "null";
            else
                return "{" + string.Join(", ", array.Cast<object>().Select(o => o.ToString()).ToArray()) + "}";
        }

        public static string ToString<T>(List<T> list)
        {
            if (list == null)
                return "null";
            else
                return "{" + string.Join(", ", list.Cast<object>().Select(o => o.ToString()).ToArray()) + "}";
        }

        public static string ToString<TKey, TValue>(Dictionary<TKey, TValue> dict)
        {
            if (dict == null)
                return "null";
            else
                return "{" + string.Join(", ", dict.Select(kvp => kvp.Key.ToString() + ":" + kvp.Value.ToString()).ToArray()) + "}";
        }

        public static int IndexOf<T>(this T[] array, T item)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (Equals(array[i], item))
                    return i;
            }
            return -1;
        }

        public static bool AddUnique<T>(this List<T> list, T item)
        {
            if (!list.Contains(item))
            {
                list.Add(item);
                return true;
            }
            return false;
        }

        public static bool AddRangeUnique<T>(this List<T> list, IEnumerable<T> collection)
        {
            bool changed = false;
            foreach (var item in collection)
            {
                if (!list.Contains(item))
                {
                    list.Add(item);
                    changed = true;
                }
            }
            return changed;
        }

        public static void AddUnique<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict.ContainsKey(key))
                dict[key] = value;
            else
                dict.Add(key, value);
        }

        public static float Remap(float v, float vMin, float vMax, float tMin, float tMax)
        {
            return (v - vMin) * (tMax - tMin) / (vMax - vMin) + tMin;
        }

        public static float RemapFrom01(float v, float tMin, float tMax)
        {
            return Remap(v, 0f, 1f, tMin, tMax);
        }
        
        public static float RemapTo01(float v, float vMin, float vMax)
        {
            return Remap(v, vMin, vMax, 0f, 1f);
        }
        
        public static T DeepCopyByReflection<T>(T obj)
        {
            if (obj is string || obj.GetType().IsValueType)
                return obj;

            object retval = Activator.CreateInstance(obj.GetType());
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Static|BindingFlags.Instance);
            foreach(var field in fields)
            {
                try
                {
                    field.SetValue(retval, DeepCopyByReflection(field.GetValue(obj)));
                }
                catch { }
            }

            return (T)retval;
        }

        public static string GetName(this Enum e)
        {
            return Enum.GetName(e.GetType(), e);
        }
        
        public static bool IsNullOrEmpty<T>(this IList<T> list) => list == null || list.Count == 0;

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        public static void Shuffle<T>(this List<T> list)
        {
            // Fisher Yates
            var rand = new Random();
            for (var i = list.Count - 1; i > 0; i--)
            {
                var j = rand.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
        
        public static List<T> Shuffle<T>(this List<T> list, int count)
        {
            if (list.Count <= count)
            {
                list.Shuffle();
                return list;
            }
            var newList = new List<T>();
            // Fisher Yates
            var rand = new Random();
            for (var i = 0; i < list.Count - 1; i++)
            {
                var j = rand.Next(i, list.Count);
                (list[i], list[j]) = (list[j], list[i]);
                newList.Add(list[i]);
                if (i == count - 1)
                    break;
            }
            return newList;
        }
        
    }
}