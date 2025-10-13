namespace GameCore.Observers
{
    public interface ISubject<T>
    {
        void Subscribe(string eventName, IObserver<T> observer);
        void Unsubscribe(string eventName, IObserver<T> observer);
    }
}