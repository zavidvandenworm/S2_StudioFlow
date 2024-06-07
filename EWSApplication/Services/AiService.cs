using ApplicationEF.Dtos;
using GroqSharp.Models;
using Newtonsoft.Json;

namespace ApplicationEF.Services;

public class AiService
{
    private readonly GroqClient _groqClient;
    private const string ApiModel = "llama3-70b-8192";

    public const string TaskRewriteSystemPrompt =
        "You are an assistant in Studioflow, an application designed to help artists manage their projects. Your task is to rewrite the title and description of tasks to make them clearer and more concise while ensuring they retain their original meaning. You will also generate appropriate tags based on the content. The rewritten output should be natural, reasonable, and match the following format:\\r\\n\\r\\n{\\r\\n  \\\"title\\\": \\\"generated title\\\",\\r\\n  \\\"description\\\": \\\"generated description\\\",\\r\\n  \\\"tags\\\": [\\\"tag1\\\", \\\"tag2\\\"]\\r\\n}\\r\\n\\r\\nThe input will follow the same format.\\r\\n\\r\\nExample:\\r\\n\\r\\nInput:\\r\\n\\r\\n{\\r\\n  \\\"title\\\": \\\"Finish painting the sunset scene\\\",\\r\\n  \\\"description\\\": \\\"Need to complete the final touches on the sunset scene painting for the upcoming exhibition.\\\",\\r\\n  \\\"tags\\\": [\\\"painting\\\", \\\"sunset\\\"]\\r\\n}\\r\\nOutput:\\r\\n\\r\\n{\\r\\n  \\\"title\\\": \\\"Complete Final Touches on Sunset Painting\\\",\\r\\n  \\\"description\\\": \\\"Finalize the sunset scene painting for the upcoming exhibition by adding the last details.\\\",\\r\\n  \\\"tags\\\": [\\\"artwork\\\", \\\"exhibition\\\", \\\"sunset\\\"]\\r\\n}\\r\\nYour goal is to provide a rewritten title, description, and relevant tags based on the given input. Please proceed with the task.";

    public const string ProjectRewriteSystemPrompt =
        "You are an assistant in Studioflow, an application designed to help artists create and manage their music projects. Your task is to rewrite the title and description of projects to make them clearer and more concise while ensuring they retain their original meaning. You will also generate appropriate tags based on the content. The rewritten output should be natural, reasonable, and match the following format:\\r\\n\\r\\n{\\r\\n\\\"title\\\": \\\"generated title\\\",\\r\\n\\\"description\\\": \\\"generated description\\\",\\r\\n\\\"tags\\\": [\\\"tag1\\\", \\\"tag2\\\"]\\r\\n}\\r\\n\\r\\nThe input will follow the same format.\\r\\n\\r\\nExample:\\r\\n\\r\\nInput:\\r\\n\\r\\n{\\r\\n\\\"title\\\": \\\"march uptempo song\\\",\\r\\n\\\"description\\\": \\\"collab with artist123\\\",\\r\\n\\\"tags\\\": [\\\"uptempo\\\"]\\r\\n}\\r\\nOutput:\\r\\n\\r\\n{\\r\\n\\\"title\\\": \\\"Hardcore Uptempo Collaboration\\\",\\r\\n\\\"description\\\": \\\"A collaboration with artist123 for the upcoming March release.\\\",\\r\\n\\\"tags\\\": [\\\"kicks\\\", \\\"hardcore\\\", \\\"uptempo\\\"]\\r\\n}\\r\\nYour goal is to provide a rewritten title, description, and relevant tags based on the given input. Please proceed with the task.";
    
    public AiService()
    {
        var apiKey = Environment.GetEnvironmentVariable("GROQ_API_KEY");

        _groqClient = new GroqClient(apiKey ?? throw new Exception("could not load groq api key."), ApiModel);
    }

    public async Task<string> GetResponse(string systemPrompt, string prompt)
    {
        var response = await _groqClient.CreateChatCompletionAsync(
            new Message {Role = MessageRoleType.System, Content = systemPrompt},
            new Message { Role = MessageRoleType.User, Content = prompt });

        return response!;
    }
}