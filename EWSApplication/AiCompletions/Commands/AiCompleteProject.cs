using ApplicationEF.Dtos;
using ApplicationEF.Services;
using MediatR;
using Newtonsoft.Json;

namespace ApplicationEF.AiCompletions.Commands;

public class AiCompleteProjectCommand : IRequest<AiCompleteDto>
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required List<string> Tags { get; set; }
}

public class AiCompleteProjectCommandHandler : IRequestHandler<AiCompleteProjectCommand, AiCompleteDto>
{
    private readonly AiService _aiService;

    public AiCompleteProjectCommandHandler(AiService aiService)
    {
        _aiService = aiService;
    }

    public async Task<AiCompleteDto> Handle(AiCompleteProjectCommand request, CancellationToken cancellationToken)
    {
        var aiRequest = new AiCompleteDto
        {
            Title = request.Title,
            Description = request.Description,
            Tags = request.Tags
        };
        
        var response = await _aiService.GetResponse(AiService.ProjectRewriteSystemPrompt, JsonConvert.SerializeObject(aiRequest));
        var aiResponse = JsonConvert.DeserializeObject<AiCompleteDto>(response);

        return aiResponse ?? throw new Exception("failed to convert ai project rewrite response to object");
    }
}