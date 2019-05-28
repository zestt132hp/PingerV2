using System;
using System.Linq;

namespace Pinger.Protocols
{
    public class ProtocolInfo : IProtocolInfo
    {
        public string GetJsonAttribute<T>()
        {
            var t = (typeof(T).CustomAttributes.Select(x => x.ConstructorArguments).FirstOrDefault() ??
                     throw new InvalidOperationException()).FirstOrDefault().Value;
            if (t != null)
                return t.ToString();
            throw new ArgumentNullException($"В данном типе нет атрибутов");
        }
    }
}
