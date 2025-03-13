using System.Collections.Generic;
using UnityEngine;

public abstract partial class EventChannel<T> : ScriptableObject
{
    public HashSet<IEventListener<T>> observers = new();

    public void Invoke(object sender, T value)
    {
        foreach (var observer in observers)
        {
            observer.OnEventRaised(sender, value);
        }
    }

    public void Register(IEventListener<T> observer) => observers.Add(observer);
    public void Unregister(IEventListener<T> observer) => observers.Remove(observer);
}

[CreateAssetMenu(menuName = "ScriptableObjects/Event Channel/EmptyEventChannel")]
public class EventChannel : EventChannel<Empty> { }
public readonly struct Empty { }