using Domain.Common;
using Domain.Entities;

namespace Domain.Events;

public class UserLoggedInEvent : BaseEvent
{
    public UserLoggedInEvent(User user)
    {
        this.User = user;
    }
    
    public User User { get; set; }
}