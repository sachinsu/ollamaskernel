namespace OllamaSK.Tests;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.TextGeneration;

using Xunit;
using ollamask;


//ref: https://techcommunity.microsoft.com/t5/educator-developer-blog/extending-semantic-kernel-using-ollamasharp-for-chat-and-text/ba-p/4104953
public class TestChatGeneration
{
        private const string modelName = "gemma:2b";
    private const string endpoint = "http://localhost:11434";



    [Fact]
    public async void TestChatGenerationviaSK() 
    {
        var ollamachat = ServiceProvider.GetChatCompletionService();


        // semantic kernel builder
        var builder = Kernel.CreateBuilder();
        builder.Services.AddKeyedSingleton<IChatCompletionService>("ollamaChat", ollamachat);
        // builder.Services.AddKeyedSingleton<ITextGenerationService>("ollamaText", ollamaText);
        var kernel = builder.Build();


        // chat generation
        var chatGen = kernel.GetRequiredService<IChatCompletionService>();
        ChatHistory chat = new("You are an AI assistant that helps people find information.");
        chat.AddUserMessage("What is Sixth Sense?");
        var answer = await chatGen.GetChatMessageContentAsync(chat);
        Assert.NotNull(answer);
        Assert.NotEmpty(answer.Content!);
        System.Diagnostics.Debug.WriteLine(answer.Content!);


    }

    [Fact]
    public async void TestStreamChatGenerationviaSK() 
    {
        var ollamachat = ServiceProvider.GetChatCompletionService();


        // semantic kernel builder
        var builder = Kernel.CreateBuilder();
        builder.Services.AddKeyedSingleton<IChatCompletionService>("ollamaChat", ollamachat);
        var kernel = builder.Build();


        // chat generation
        var chatGen = kernel.GetRequiredService<IChatCompletionService>();
        ChatHistory chat = new("You are an AI assistant that helps people find information.");
        chat.AddUserMessage("Tell me joke about dogs");
        await foreach (StreamingChatMessageContent message in chatGen.GetStreamingChatMessageContentsAsync(chat))
        {
            Assert.NotNull(message);
            Assert.NotNull(message.Content!);
            System.Diagnostics.Debug.WriteLine(message);
        }


    }




}
