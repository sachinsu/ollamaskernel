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
    public async void TestStreamTextGenerationviaSK() 
    {
        var ollamaText = ServiceProvider.GetTextGenerationService();


        // semantic kernel builder
        var builder = Kernel.CreateBuilder();
        // builder.Services.AddKeyedSingleton<IChatCompletionService>("ollamaChat", ollamaChat);
        builder.Services.AddKeyedSingleton<ITextGenerationService>("ollamaText", ollamaText);
        var kernel = builder.Build();

        // text generation
        var textGen = kernel.GetRequiredService<ITextGenerationService>();

        await foreach(StreamingTextContent str in  textGen.GetStreamingTextContentsAsync("Who is President of USA?")){
            Assert.NotNull(str.Text);
        }


    }
    [Fact]
    public async void TestTextGenerationviaSK() 
    {
        var ollamaText = ServiceProvider.GetTextGenerationService();


        // semantic kernel builder
        var builder = Kernel.CreateBuilder();
        builder.Services.AddKeyedSingleton<ITextGenerationService>("ollamaText", ollamaText);
        var kernel = builder.Build();

        // text generation
        var textGen = kernel.GetRequiredService<ITextGenerationService>();
        var response = await textGen.GetTextContentsAsync("The weather in January in Toronto is usually ");
        Assert.NotEqual(response[^1].Text,String.Empty);



    }


}
