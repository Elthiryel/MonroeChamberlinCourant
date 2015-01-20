using System;

namespace MonroeChamberlinCourant.Framework.Exceptions
{
    public class InvalidPreferencesFormatException : Exception
    {
        public InvalidPreferencesFormatException() { }
        public InvalidPreferencesFormatException(string message) : base(message) { }
        public InvalidPreferencesFormatException(string message, Exception innerException) : base(message, innerException) { }
    }
}
