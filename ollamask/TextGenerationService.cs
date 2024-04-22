using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.TextGeneration;

namespace ollamask;

    

public class TextGenerationService : ITextGenerationService
{
    
    public string ModelApiEndPoint { get; set; }
    public string ModelName { get; set; }

    public IReadOnlyDictionary<string, object?> Attributes => throw new NotImplementedException();

    private HttpClient client = new();

    public async Task<IReadOnlyList<TextContent>> GetTextContentsAsync(string prompt, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
    {
          
        var client = new OllamaApiClient(ModelApiEndPoint, ModelName);

        OllamaApiClient.ChatRequest req = new OllamaApiClient.ChatRequest() {
                Model=ModelName,
                Prompt=prompt,
        };

        OllamaApiClient.ChatResponse resp = await client.GetResponseForPromptAsync(req
            , cancellationToken);

        return  new List<TextContent>() { new TextContent(resp.Response) };
    }

    public IAsyncEnumerable<StreamingTextContent> GetStreamingTextContentsAsync(string prompt, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
    {
            // var ollama = new OllamaApiClient(ModelUrl, ModelName);

            // var completionResponse = await ollama.GetCompletion(prompt, null, CancellationToken.None);

            // TextContent stc = new TextContent(completionResponse.Response);
            // return new List<TextContent> { stc };    
            throw new NotImplementedException();
    }
}
