namespace Conduit.Domain.Exceptions
{
    public class ForbiddenOperationException : Exception
    {
        public ForbiddenOperationException() { }

        public ForbiddenOperationException(string message) : base(message) { }
    }
}
