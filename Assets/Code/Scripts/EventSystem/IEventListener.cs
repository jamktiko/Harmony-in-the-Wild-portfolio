public interface IEventListener<T>
{
    public void OnEventRaised(object sender, T value);
}