using UnityEngine;
using UnityEngine.Events;

public abstract class EventListener<T> : MonoBehaviour, IEventListener<T>
{
    [SerializeField] EventChannel<T> eventChannel;
    [SerializeField] UnityEvent<T> response;

    private void Awake() => eventChannel.Register(this);
    private void OnDestroy() => eventChannel.Unregister(this);
    
    void IEventListener<T>.OnEventRaised(object sender, T value)
    {
        response?.Invoke(value);
    }
}

public class EventListener : EventListener<Empty> { }
