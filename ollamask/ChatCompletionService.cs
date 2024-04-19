using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace ollamask;

// https://elbruno.com/2024/04/01/%F0%9F%93%8E-extending-semantickernel-using-ollamasharp-for-chat-and-text-completion/
public class OllamaChatCompletionService : IChatCompletionService
{
    public string ModelApiEndPoint { get; set; }
    public string ModelName { get; set; }

    private HttpClient client = new();

    public IReadOnlyDictionary<string, object?> Attributes => throw new NotImplementedException();

    public Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
   {




// var ollama = new OllamaApiClient(ModelUrl, ModelName); // (uri);

//             var chat = new Chat(ollama, _ => { });


//             // iterate though chatHistory Messages
//             foreach (var message in chatHistory)
//             {
//                 if (message.Role == AuthorRole.System)
//                 {
//                     await chat.SendAs(ChatRole.System, message.Content);
//                     continue;
//                 }
//             }

//             var lastMessage = chatHistory.LastOrDefault();

//             string question = lastMessage.Content;
//             var chatResponse = "";
//             var history = (await chat.Send(question, CancellationToken.None)).ToArray();

//             var last = history.Last();
//             chatResponse = last.Content;

//             chatHistory.AddAssistantMessage(chatResponse);

//             return chatHistory;

        throw new NotImplementedException();
    }

    public IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
