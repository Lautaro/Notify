using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lautaro
{
    public class Notification<T> 
    {
        public string Topic { get; set; }
        public T Context { get; set; }
        public Notification(T context, string topic = "")
        {
            Context = context;
            Topic = topic;
        }
    }
}
