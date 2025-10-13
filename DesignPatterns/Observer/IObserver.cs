namespace GameCore.Observers
{
    public interface IObserver<T>
    {
        void OnBeingNotified(string eventName, T data);
    }
}