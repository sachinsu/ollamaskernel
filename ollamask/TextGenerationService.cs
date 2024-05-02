using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.TextGeneration;

namespace ollamask;

public class TextGenerationService : ITextGenerationService
{
    
    public string ModelApiEndPoint { get; set; }
    public string ModelName { get; set; }

    public IReadOnlyDictionary<string, object?> Attributes => throw new NotImplementedException();

    public async Task<IReadOnlyList<TextContent>> GetTextContentsAsync(string prompt, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
    {
          
        var client = new OllamaApiClient(ModelApiEndPoint, ModelName);

        OllamaApiClient.ChatRequest req = new OllamaApiClient.ChatRequest() {
                Model=ModelName,
                Prompt=prompt,
        };

        OllamaApiClient.ChatResponse resp = await client.GetResponseForPromptAsync(req
            , cancellationToken);

        return new List<TextContent>() { new TextContent(resp.Response) };
    }

    public async IAsyncEnumerable<StreamingTextContent> GetStreamingTextContentsAsync(string prompt, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
    {
            var ollama = new OllamaApiClient(ModelApiEndPoint, ModelName);

            OllamaApiClient.ChatRequest req = new OllamaApiClient.ChatRequest() {
                    Prompt=prompt,
                    Stream=true
            };

            await foreach( OllamaApiClient.ChatResponse resp in ollama.GetStreamForPromptAsync(req, cancellationToken)) {
                    yield return new StreamingTextContent( text:  resp.Response) ;
            } 

    }
}
