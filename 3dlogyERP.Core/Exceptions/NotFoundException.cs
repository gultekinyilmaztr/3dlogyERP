namespace _3dlogyERP.Core.Exceptions
{
    public class NotFoundException : BusinessException
    {
        public NotFoundException(string message) : base(message, "NOT_FOUND")
        {
        }
    }
}
