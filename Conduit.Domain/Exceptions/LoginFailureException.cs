namespace Conduit.Domain.Exceptions
{
    public class LoginFailureException : Exception
    {
        public LoginFailureException() { }

        public LoginFailureException(string message) : base(message) { }
    }
}
