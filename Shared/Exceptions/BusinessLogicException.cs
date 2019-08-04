using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Naandi.Shared.Exceptions
{
    [Serializable]
    public class BusinessLogicException : Exception
    {
        public string ResourceReferenceProperty { get; set; }
        public BusinessLogicException() { }

        public BusinessLogicException(string message)
            : base(message)
        {
        }
        public BusinessLogicException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected BusinessLogicException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ResourceReferenceProperty = info.GetString("ResourceReferenceProperty");
        }
    }
}
