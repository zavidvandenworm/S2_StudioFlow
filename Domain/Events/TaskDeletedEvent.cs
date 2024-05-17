using Domain.Common;

namespace Domain.Events;

public class TaskDeletedEvent : BaseEvent
{
    public TaskDeletedEvent(Entities.ProjectTask projectTask)
    {
        ProjectTask = projectTask;
    }
    
    public Entities.ProjectTask ProjectTask { get; }
}