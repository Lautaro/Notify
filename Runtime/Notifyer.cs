using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Lautaro.Notifyer
{
    public abstract class NotifyerEventBase { }

    public class NotifyerEventBase<T> : NotifyerEventBase
    {
        public T Data { get; set; }

        public NotifyerEventBase(T data)
        {
            Data = data;
        }
        public NotifyerEventBase()
        {
        }
    }

    public class Notifyer
    {
        private static readonly Notifyer instance = new Notifyer();
        private readonly Dictionary<Type, List<Delegate>> notificationTypes = new Dictionary<Type, List<Delegate>>();
        private readonly Dictionary<string, List<Delegate>> textNotifications = new Dictionary<string, List<Delegate>>();

        public static void Subscribe<T>(UnityAction<T> callback) where T : NotifyerEventBase
        {
            var type = typeof(T);
            if (!instance.notificationTypes.ContainsKey(type))
            {
                instance.notificationTypes[type] = new List<Delegate>();
            }
            instance.notificationTypes[type].Add(callback);
        }

       

        public static void Unsubscribe<T>(UnityAction<T> callback) where T : NotifyerEventBase
        {
            var type = typeof(T);
            if (instance.notificationTypes.ContainsKey(type))
            {
                var delegates = instance.notificationTypes[type];
                delegates.Remove(callback);

                if (delegates.Count == 0)
                {
                    instance.notificationTypes.Remove(type);
                }
            }
        }

        public static void Notify<T>(T eventInstance) where T : NotifyerEventBase
        {
            var type = eventInstance.GetType(); // Get the actual type of the event instance
            if (instance.notificationTypes.TryGetValue(type, out var list))
            {
                foreach (Delegate del in list)
                {
                    (del as UnityAction<T>)?.Invoke(eventInstance);
                }
            }
        }
        public static void Subscribe(UnityAction callback, string notificationId)
        {
            var type = typeof(T);
            if (!instance.textNotifications.ContainsKey(notificationId))
            {
                instance.textNotifications[type] = new List<Delegate>();
            }
            instance.textNotifications[type].Add(callback);
        }

        public static void Notify(T notificationId, bool throwError = false) where T : string
        {
            if (instance.textNotifications.TryGetValue(notificationId, out var list))
            {
                foreach (Delegate del in list)
                {
                    (del as UnityAction<T>)?.Invoke(eventInstance);
                }
            }
            else
            {
                if (throwError)
                    throw new Exception("Notifyer is trying to notify with notiticiation message id:"+notificationId+" but no one is receiving.");
            }
        }
    }
}