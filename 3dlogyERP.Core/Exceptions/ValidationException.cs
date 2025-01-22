namespace _3dlogyERP.Core.Exceptions
{
    public class ValidationException : BusinessException
    {
        public ValidationException(string message) : base(message, "VALIDATION_ERROR")
        {
        }
    }
}
