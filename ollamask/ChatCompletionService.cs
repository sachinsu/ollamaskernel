using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace ollamask;

// https://elbruno.com/2024/04/01/%F0%9F%93%8E-extending-semantickernel-using-ollamasharp-for-chat-and-text-completion/
public class OllamaChatCompletionService : IChatCompletionService
{
    public string ModelApiEndPoint { get; set; }
    public string ModelName { get; set; }

    public IReadOnlyDictionary<string, object?> Attributes => throw new NotImplementedException();

    public async Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
   {


        var client = new OllamaApiClient(ModelApiEndPoint, ModelName);

        OllamaApiClient.ChatRequest req = new OllamaApiClient.ChatRequest() {
                Model=ModelName
        };

        req.Messages = new List<OllamaApiClient.ChatMessage>();

        // iterate though chatHistory Messages
        foreach (var history in chatHistory)
        {
            req.Messages.Add(new OllamaApiClient.ChatMessage{
                Role=history.Role.ToString(),
                Content=history.Content
            });
        }

        OllamaApiClient.ChatResponse resp = await client.GetResponseForChatAsync(req
            , cancellationToken);

        List<ChatMessageContent> content = new();
        content.Add( new(role:resp.Message.Role.Equals("system",StringComparison.InvariantCultureIgnoreCase)?AuthorRole.System:AuthorRole.User,content:resp.Message.Content));

        return content;
    }

    public async IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
    {

        var client = new OllamaApiClient(ModelApiEndPoint, ModelName);

        OllamaApiClient.ChatRequest req = new OllamaApiClient.ChatRequest() {
                Model=ModelName
        };

        req.Messages = new List<OllamaApiClient.ChatMessage>();

        // iterate though chatHistory Messages
        foreach (var history in chatHistory)
        {
            req.Messages.Add(new OllamaApiClient.ChatMessage{
                Role=history.Role.ToString(),
                Content=history.Content
            });
        }

        CancellationTokenSource source = new CancellationTokenSource();
        CancellationToken token = source.Token;

        await foreach (OllamaApiClient.ChatResponse resp in  client.GetStreamForChatAsync(req,token)) { 
            yield return new(role:resp.Message.Role.Equals("system",StringComparison.InvariantCultureIgnoreCase)?AuthorRole.System:AuthorRole.User,
            content:resp.Message.Content ?? string.Empty); 
        }

     }
}
