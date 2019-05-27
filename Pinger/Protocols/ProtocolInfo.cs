using System;
using System.Linq;

namespace Pinger.Protocols
{
    class ProtocolInfo : IProtocolInfo
    {
        public string GetJsonAttribute<T>()
        {
            var t = (typeof(T).CustomAttributes.Select(x => x.ConstructorArguments).FirstOrDefault() ??
                     throw new InvalidOperationException()).FirstOrDefault().Value;
            return t.ToString();
        }
    }
}
