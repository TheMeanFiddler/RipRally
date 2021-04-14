using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MovingAvgFloat
{
    Queue<float> queue = new Queue<float>();
    int _count;
    int _maxcount;
    float Sum;

    //constructor
    public MovingAvgFloat(int count)
    {
        _maxcount = count;
    }
    /// <summary>
    /// Add to the queue,  dequeue the last item and update the sum
    /// </summary>
    /// <param name="val"></param>
    public void Push(float val)
    {
        queue.Enqueue(val);
        Sum = Sum + val;
        _count = queue.Count;
        if (_count > _maxcount) { Sum -= queue.Dequeue(); }
    }

    /// <summary>
    /// Add to the queue and return the dequeued item
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public float Shift(float val)
    {
        queue.Enqueue(val);
        return queue.Dequeue();
    }
    public void Clear()
    {
        _count = 0;
        queue.Clear();
        Sum = 0;
    }

    public int Count { get { return _count; } }
    public float Avg { get { return _count == 0 ? 0 : Sum / _count; } }

}

