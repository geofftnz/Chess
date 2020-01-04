using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Chess.Engine.Exceptions
{
    public class InvalidMoveException : Exception
    {
        public InvalidMoveReason Reason { get; set; }

        public InvalidMoveException()
        {
        }

        public InvalidMoveException(InvalidMoveReason reason) : this()
        {
            Reason = reason;
        }
        public InvalidMoveException(InvalidMoveReason reason, string message) : this(message)
        {
            Reason = reason;
        }

        public InvalidMoveException(string message) : base(message)
        {
        }

        public InvalidMoveException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidMoveException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
