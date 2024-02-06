using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    //private readonly Dictionary<NotiPings, List<Delegate>> notificationPings = new Dictionary<NotiPings, List<Delegate>>();

    //public static void Subscribe(NotiPings notiPing, UnityAction callback)
    //{
    //    if (!instance.notificationPings.ContainsKey(notiPing))
    //    {
    //        instance.notificationPings[notiPing] = new List<Delegate>();
    //    }
    //    instance.notificationPings[notiPing].Add(callback);
    //}
    public static void Subscribe<T>(UnityAction<T> callback) where T : NotifyerEventBase
    {
        var type = typeof(T);
        if (!instance.notificationTypes.ContainsKey(type))
        {
            instance.notificationTypes[type] = new List<Delegate>();
        }
        instance.notificationTypes[type].Add(callback);
    }
    //public static void UnSubscribe(NotiPings notiPing, UnityAction callback)
    //{
    //    if (instance.notificationPings.ContainsKey(notiPing))
    //    {
    //        var delegates = instance.notificationPings[notiPing];
    //        delegates.Remove(callback);

    //        if (delegates.Count == 0)
    //        {
    //            instance.notificationPings.Remove(notiPing);
    //        }
    //    }
    //}

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

    //public static void Notify(NotiPings notiPing) 
    //{
    //    if (instance.notificationPings.TryGetValue(notiPing, out var list))
    //    {
    //        foreach (Delegate del in list)
    //        {
    //            (del as UnityAction)?.Invoke();
    //        }
    //    }
    //}
}
