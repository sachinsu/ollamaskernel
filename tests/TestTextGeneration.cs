namespace OllamaSK.Tests;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.TextGeneration;

using Xunit;
using ollamask;


//ref: https://techcommunity.microsoft.com/t5/educator-developer-blog/extending-semantic-kernel-using-ollamasharp-for-chat-and-text/ba-p/4104953
public class TestTextGeneration
{
    [Fact]
    public void TestTextGenerationviaSK() 
    {
        var ollamaText = new TextGenerationService();
        ollamaText.ModelApiEndPoint = "http://localhost:11434";
        ollamaText.ModelName = "gemma:2b";


        // semantic kernel builder
        var builder = Kernel.CreateBuilder();
        // builder.Services.AddKeyedSingleton<IChatCompletionService>("ollamaChat", ollamaChat);
        builder.Services.AddKeyedSingleton<ITextGenerationService>("ollamaText", ollamaText);
        var kernel = builder.Build();

        // text generation
        var textGen = kernel.GetRequiredService<ITextGenerationService>();
        var response = textGen.GetTextContentsAsync("The weather in January in Toronto is usually ").Result;
        Assert.NotEqual(response[^1].Text,String.Empty);



    }


}
