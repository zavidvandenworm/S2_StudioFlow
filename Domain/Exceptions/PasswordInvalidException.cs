namespace Domain.Exceptions;

public class PasswordInvalidException : Exception
{
    public PasswordInvalidException() : base("The supplied password was incorrect!")
    {
        
    }
}