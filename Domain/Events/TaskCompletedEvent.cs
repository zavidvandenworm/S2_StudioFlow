using Domain.Common;

namespace Domain.Events;

public class TaskCompletedEvent : BaseEvent
{
    public TaskCompletedEvent(Entities.ProjectTask projectTask)
    {
        ProjectTask = projectTask;
    }
    
    public Entities.ProjectTask ProjectTask { get; }
}