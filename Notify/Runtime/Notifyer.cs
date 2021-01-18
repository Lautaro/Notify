using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Lautaro
{
    public class Notifyer
    {
        static Notifyer instance = new Notifyer();
        Dictionary<(Type, string),object> notificationTypes = new Dictionary<(Type, string), object>();

        static bool Match<T>((Type, string) key, string title)
        {
            return (key.Item1 == typeof(T) && key.Item2 == title);
        }

        public static void Notify<T>(T context, string title)
        {
            var notificationType = instance.notificationTypes.Where(s => Match<T>(s.Key, title)).
            FirstOrDefault().Value as NotificationType<T>;

            if (notificationType != null)
                notificationType.subscribers.Invoke(context);
        }

        public static void Unsubscribe<T>(UnityAction<T> callback, string title = "")
        {
            NotificationType<T> notificationType = instance.notificationTypes.FirstOrDefault(s => Match<T>(s.Key, title)).Value as NotificationType<T>;
            if (notificationType != null)
            {
                notificationType.subscribers.RemoveListener(callback);
            }
        }

        public static void Subscribe<T>(UnityAction<T> callback, string title = "" )
        {
            NotificationType<T> notificationType;

            if (instance.notificationTypes.Any(s => Match<T>(s.Key, title)))
            {
                notificationType = instance.notificationTypes.First(s => Match<T>(s.Key, title)).Value as NotificationType<T>;
            }
            else
            {
                notificationType = new NotificationType<T>( title);
                instance.notificationTypes.Add( (typeof(T),title),notificationType)                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ;
            }
            notificationType.subscribers.AddListener(callback);
        }
    }
    class NotificationType<T>
    {
        internal Type Type;
        internal readonly string Title;
        internal UnityEvent<T> subscribers = new UnityEvent<T>();

        public NotificationType(string title)
        {  
            Title = title;
        }
    }
}
