namespace GameCore.DesignPatterns
{
    public interface IObserver<T>
    {
        void OnBeingNotified(string eventName, T data);
    }
}