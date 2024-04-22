namespace OllamaSK.Tests;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ollamask;


public class TestApiClient
{
    [Fact]
    public void TestClientCreation() 
    {
        OllamaApiClient client = new("http://localhost:11434","gemma:2b");

        Assert.NotEqual(null,client );      
    }


    [Fact]
    public async void TestPromptResponseNotStreaming() 
    {
        OllamaApiClient client = new("http://localhost:11434","gemma:2b");

        OllamaApiClient.ChatRequest req = new OllamaApiClient.ChatRequest() {
                Model="gemma:2b",
                Prompt="what is meteor?",
        };

        Assert.NotNull(client );      

        Assert.NotEqual("tata",req.Model );
        Assert.NotEqual("what",req.Prompt );
        Assert.NotEqual(true,req.Stream );
        
        CancellationTokenSource source = new CancellationTokenSource();
        CancellationToken tkn = source.Token;
        

        OllamaApiClient.ChatResponse resp = await client.GetResponseForPromptAsync(req
            , tkn);

        Assert.NotNull(resp );

    }

    [Fact]
    public void TestPromptResponseStreaming() 
    {
        OllamaApiClient client = new("http://localhost:11434","gemma:2b");

        Assert.NotNull(client );      
    }

    [Fact]
    public void TestChatResponseNotStreaming() 
    {
        OllamaApiClient client = new("http://localhost:11434","gemma:2b");

        Assert.NotNull(client );      
    }


}
