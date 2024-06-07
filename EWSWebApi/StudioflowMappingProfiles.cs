using ApplicationEF.Dtos;
using EWSDomain.Entities;
using Profile = AutoMapper.Profile;

namespace EWSWebApi;

public class StudioflowMappingProfiles : Profile
{
    public StudioflowMappingProfiles()
    {
        CreateMap<Project, ProjectDto>().ReverseMap();
        CreateMap<ProjectMember, ProjectMemberDto>().ReverseMap();
        CreateMap<Project, UpdateProjectDto>().ReverseMap();
        CreateMap<ProjectFile, ProjectFileReferenceDto>().ReverseMap();
        CreateMap<ProjectFile, ProjectFileDto>().ReverseMap();
        CreateMap<ProjectMember, ProjectMemberReferenceDto>().ReverseMap();
        CreateMap<ProjectTask, ProjectTaskDto>().ReverseMap();
        CreateMap<ProjectTask, UpdateProjectTaskDto>().ReverseMap();
    }
}