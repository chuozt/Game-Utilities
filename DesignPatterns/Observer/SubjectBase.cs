using System.Collections.Generic;

namespace GameCore.Observers
{
    public abstract class SubjectBase<T> : ISubject<T>
    {
        private Dictionary<string, List<IObserver<T>>> observers = new Dictionary<string, List<IObserver<T>>>();

        public void Subscribe(string eventName, IObserver<T> observer)
        {
            if (!observers.ContainsKey(eventName))
                observers[eventName] = new List<IObserver<T>>();

            observers[eventName].Add(observer);
        }

        public void Unsubscribe(string eventName, IObserver<T> observer)
        {
            if (!observers.ContainsKey(eventName))
                return;

            observers[eventName].Remove(observer);

            if (observers[eventName].Count == 0)
                observers.Remove(eventName);
        }

        protected void Notify(string eventName, T data)
        {
            if (!observers.ContainsKey(eventName))
                return;

            for (int i = observers[eventName].Count - 1; i >= 0; i--)
                observers[eventName][i].OnBeingNotified(eventName, data);
        }
    }
}