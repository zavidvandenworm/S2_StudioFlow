using ApplicationEF.Dtos;
using ApplicationEF.Services;
using MediatR;
using Newtonsoft.Json;

namespace ApplicationEF.AiCompletions.Commands;

public class AiCompleteTaskCommand : IRequest<AiCompleteDto>
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required List<string> Tags { get; set; }
}

public class AiCompleteTaskCommandHandler : IRequestHandler<AiCompleteTaskCommand, AiCompleteDto>
{
    private readonly AiService _aiService;

    public AiCompleteTaskCommandHandler(AiService aiService)
    {
        _aiService = aiService;
    }

    public async Task<AiCompleteDto> Handle(AiCompleteTaskCommand request, CancellationToken cancellationToken)
    {
        var aiRequest = new AiCompleteDto
        {
            Title = request.Title,
            Description = request.Description,
            Tags = request.Tags
        };
        
        var response = await _aiService.GetResponse(AiService.TaskRewriteSystemPrompt, JsonConvert.SerializeObject(aiRequest));
        var aiResponse = JsonConvert.DeserializeObject<AiCompleteDto>(response);

        return aiResponse ?? throw new Exception("failed to convert ai task rewrite response to object");
    }
}