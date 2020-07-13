using System;
using System.Collections;
using System.Collections.Generic;

public class FixedCapacityDeque<T> : IEnumerable<T>
{
  private readonly T[] _buffer;
  private int _head;

  private int Tail {
    get { return (_head + Length) % Capacity; }
  }

  private int Capacity {
    get { return _buffer.Length; }
  }

  public int Length { get; private set; }

  public T First {
    get {
      if (Length <= 0) throw new InvalidOperationException("Buffer is empty.");
      return _buffer[_head];
    }
  }

  public T Last {
    get {
      if (Length <= 0) throw new InvalidOperationException("Buffer is empty.");
      return _buffer[(_head + Length - 1) % Capacity];
    }
  }

  public T this[int index] {
    get { return _buffer[(_head + index) % Capacity]; }
  }

  public FixedCapacityDeque(int capacity)
  {
    _buffer = new T[capacity];
  }

  public void PushFirst(T v)
  {
    if (Capacity <= Length) throw new InvalidOperationException("Not remaining buffer.");

    var index = _head - 1;
    if (index < 0) index = Capacity - 1;

    _buffer[index] = v;
    _head = index;

    ++Length;
  }

  public void PushLast(T v)
  {
    if (Capacity <= Length) throw new InvalidOperationException("Not remaining buffer.");

    _buffer[Tail] = v;
    ++Length;
  }

  public T PopFirst()
  {
    var v = First;
    _head = ++_head % Capacity;
    --Length;
    return v;
  }

  public T PopLast()
  {
    var v = Last;
    --Length;
    return v;
  }


  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

  public IEnumerator<T> GetEnumerator()
  {
    for (var i = 0; i < Length; ++i) {
      var index = (_head + i) % Capacity;
      yield return _buffer[index];
    }
  }
}
