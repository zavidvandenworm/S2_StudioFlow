namespace Domain.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException() : base("User was not found!")
    {
        
    }
}