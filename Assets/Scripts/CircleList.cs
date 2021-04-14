using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CircleList<T> : List<T> where T : IEquatable<T>
{
    /// <summary>
    /// Gets/sets the value at the specified index.
    /// </summary>
    public new T this[int idx]
    {
        get
        {
            idx = idx % Count;
            if (idx < 0) idx = Count + idx;
            return base[idx];
        }
        set
        {
            idx = idx % Count;
            if (idx < 0) idx = Count + idx;
            base[idx] = value;
        }

    }

    public bool IsOrderedAs(T a, T b)
    {
        int aIdx = IndexOf(a);
        int bIdx = IndexOf(b);
        int HalfCount = Count / 2;
        int diff = bIdx - aIdx;
        if ((diff > 0 && diff < HalfCount) || (diff < -HalfCount)) return true; else return false;

    }
    public T Next(T a)
    {
        int aIdx = IndexOf(a);
        aIdx++;
        if (aIdx == Count) aIdx = 0;
        return base[aIdx];
    }

    public T Prev(T a)
    {
        int aIdx = IndexOf(a);
        aIdx--;
        if (aIdx == -1) aIdx = Count - 1;
        return base[aIdx];
    }

    public int Diff(T Frst, T Scnd)
    {
        int d = IndexOf(Scnd) - IndexOf(Frst);
        if (d < -Count / 2) d += Count;
        return d;
    }

    public T FindNext(Func<T, bool> Predicate, T Current)
    {
        T TestObj = Next(Current);
        while (!TestObj.Equals(Current))
        {
            if (Predicate(TestObj)) return TestObj;
            TestObj = Next(TestObj);
        }
        return default(T);
    }

}


