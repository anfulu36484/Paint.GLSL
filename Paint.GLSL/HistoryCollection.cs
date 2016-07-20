using System;
using System.Collections.Generic;
using System.Linq;

namespace Paint.GLSL
{
    class HistoryCollection<T>
        where T:IDisposable
    {
        private readonly int _maxCapacity;
        List<T> list = new List<T>();

        public HistoryCollection(int maxCapacity)
        {
            _maxCapacity = maxCapacity;
        }

        public void Push(T input)
        {
            if (list.Count > _maxCapacity)
            {
                T t = list.First();
                list.Remove(t);
                t.Dispose();
            }
                
            list.Add(input);
        }

        public T Peek()
        {
            return list.Last();
        }

        public T Pop()
        {
            T t= list.Last();
            list.Remove(t);
            return t;
        }

        public int Count => list.Count;

        public void Clear()
        {
            foreach (var o in list)
            {
                o.Dispose();
            }
            list.Clear();
        }
    }
  
}
