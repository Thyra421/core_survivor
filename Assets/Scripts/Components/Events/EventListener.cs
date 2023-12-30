public class EventListener<T>
{
    public delegate void ListenerHandler(T value);

    public event ListenerHandler Listener;

    public ListenerHandler ListenerEvent => Listener;
}