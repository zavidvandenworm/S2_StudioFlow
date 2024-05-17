using Domain.Common;

namespace Domain.Events;

public class TaskCreatedEvent : BaseEvent
{
    public TaskCreatedEvent(Entities.ProjectTask projectTask)
    {
        ProjectTask = projectTask;
    }
    
    public Entities.ProjectTask ProjectTask { get; }
}