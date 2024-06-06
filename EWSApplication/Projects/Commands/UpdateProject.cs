using ApplicationEF.Dtos;
using ApplicationEF.Exceptions;
using ApplicationEF.Services;
using AutoMapper;
using EWSDomain.Entities;
using EWSDomain.Enums;
using EWSInfrastructure.Interfaces;
using MediatR;

namespace ApplicationEF.Projects.Commands;

public class UpdateProjectCommand : IRequest
{
    public required UpdateProjectDto UpdateProjectDto { get; set; }
    public required int UserId { get; set; }
}

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand>
{
    private readonly ProjectService _projectService;
    private readonly IProjectRepository _projects;
    private readonly IMapper _mapper;

    public UpdateProjectCommandHandler(IProjectRepository projects, IMapper mapper, ProjectService projectService)
    {
        _projects = projects;
        _mapper = mapper;
        _projectService = projectService;
    }

    public async Task Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectService.GetProjectAndValidateUserAccess(request.UpdateProjectDto.ProjectId, request.UserId,
            ProjectRole.Participant);

        request.UpdateProjectDto.ProjectMembers = request.UpdateProjectDto.ProjectMembers.DistinctBy(m => m.UserId).ToList();
        request.UpdateProjectDto.Tags = request.UpdateProjectDto.Tags.Distinct().ToList();

        await _projectService.EnsureThatMembersExist(request.UpdateProjectDto.ProjectMembers);
        
        var updatedProject = _mapper.Map(request.UpdateProjectDto, project);
        
        await _projects.UpdateAsync(updatedProject);
    }
}