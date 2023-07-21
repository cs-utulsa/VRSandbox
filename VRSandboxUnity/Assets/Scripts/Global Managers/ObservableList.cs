using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObservableList<T>
{
    public event Action<int> ElementChanged;
    public event Action ListUpdated;
    [SerializeField] private List<T> _value;

    public int Count
    {
        get { return _value.Count; }
    }

    public T this[int index]
    {
        get
        {
            return _value[index];
        }
        set
        {
            //you might want to add a check here to only call changed
            //if _value[index] != value
            _value[index] = value;
            ElementChanged?.Invoke(index);
        }
    }

    public void Add(T item)
    {
        _value.Add(item);
        ListUpdated?.Invoke();
    }
    public void Remove(T item)
    {
        _value.Remove(item);
        ListUpdated?.Invoke();
    }
    public void AddRange(IEnumerable<T> collection)
    {
        _value.AddRange(collection);
        ListUpdated?.Invoke();
    }
    public void RemoveRange(int index, int count)
    {
        _value.RemoveRange(index, count);
        ListUpdated?.Invoke();
    }
    public void Clear()
    {
        _value.Clear();
        ListUpdated?.Invoke();
    }
    public void Insert(int index, T item)
    {
        _value.Insert(index, item);
        ListUpdated?.Invoke();
    }
    public void InsertRange(int index, IEnumerable<T> collection)
    {
        _value.InsertRange(index, collection);
        ListUpdated?.Invoke();
    }
    public void RemoveAll(Predicate<T> match)
    {
        _value.RemoveAll(match);
        ListUpdated?.Invoke();
    }

    public T[] ToArray()
    {
        return _value.ToArray();
    }

    public List<T> ToList()
    {
        return _value;
    }

}
