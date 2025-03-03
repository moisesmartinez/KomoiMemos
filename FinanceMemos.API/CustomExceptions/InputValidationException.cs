namespace FinanceMemos.API.CustomExceptions
{
    public class InputValidationException : Exception
    {
        public InputValidationException(string message) : base(message) { }
    }
}
