using Domain.Common;

namespace Domain.Events;

public class TaskDeletedEvent : BaseEvent
{
    public TaskDeletedEvent(Entities.Task task)
    {
        Task = task;
    }
    
    public Entities.Task Task { get; }
}