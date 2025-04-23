using System;
using System.Collections.Generic;

namespace TT
{
    public class Observer : Singleton<Observer>
    {
        Dictionary<string, HashSet<Action<object>>> _observerTopics = new Dictionary<string, HashSet<Action<object>>>();

        public void AddObserver(string topicName, Action<object> observer)
        {
            HashSet<Action<object>> observers = CreateObserverTopic(topicName);
            observers.Add(observer);
        }

        public void RemoveObserver(string topicName, Action<object> observer)
        {
            HashSet<Action<object>> observers = CreateObserverTopic(topicName);
            observers.Remove(observer);
        }

        public void RemoveObserverTopic(string topicName)
        {
            if (_observerTopics.ContainsKey(topicName))
                _observerTopics.Remove(topicName);
        }

        public void Notify(string topicName)
        {
            HashSet<Action<object>> observers = CreateObserverTopic(topicName);
            foreach (Action<object> observer in observers)
                observer?.Invoke(null);
        }

        public void NotifyWithData(string topicName, object data)
        {
            var observers = CreateObserverTopic(topicName);
            foreach (var observer in observers)
            {
                observer?.Invoke(data);
            }
        }

        protected HashSet<Action<object>> CreateObserverTopic(string topicName)
        {
            if (!_observerTopics.ContainsKey(topicName))
                _observerTopics.Add(topicName, new HashSet<Action<object>>());
            return _observerTopics[topicName];
        }
    }
}
