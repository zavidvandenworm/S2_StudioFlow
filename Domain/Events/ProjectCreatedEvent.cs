using Domain.Common;
using Domain.Entities;

namespace Domain.Events;

public class ProjectCreatedEvent : BaseEvent
{
    public ProjectCreatedEvent(Project project)
    {
        Project = project;
    }
    
    public Project Project { get; }
}