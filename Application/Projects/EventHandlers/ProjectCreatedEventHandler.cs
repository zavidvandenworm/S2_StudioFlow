using Domain.Events;
using MediatR;

namespace Application.Projects.EventHandlers;

public class ProjectCreatedEventHandler : INotificationHandler<ProjectCreatedEvent>
{
    public Task Handle(ProjectCreatedEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Created project {notification.Project.Name} by author {notification.Project.ProjectMembers.First().User.Username}");
        
        return Task.CompletedTask;
    }
}