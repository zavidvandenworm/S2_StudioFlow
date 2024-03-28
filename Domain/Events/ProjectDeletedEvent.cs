using Domain.Common;
using Domain.Entities;

namespace Domain.Events;

public class ProjectDeletedEvent : BaseEvent
{
    public ProjectDeletedEvent(Project project)
    {
        Project = project;
    }
    
    public Project Project { get; }
}