namespace OllamaSK.Tests;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ollamask;


public class TestApiClient
{

    private const string modelName = "phi3";
    private const string endpoint = "http://localhost:11434";

    [Fact]
    public void TestClientCreation() 
    {
        OllamaApiClient client = new(endpoint,modelName);

        Assert.NotEqual(null,client );      
    }


    [Fact]
    public async void TestPromptResponseNotStreaming() 
    {
        OllamaApiClient client = new(endpoint,modelName);

        OllamaApiClient.ChatRequest req = new OllamaApiClient.ChatRequest() {
                Prompt="what is meteor?"
        };

        Assert.NotNull(client );      

        Assert.NotEqual("what",req.Prompt );
        Assert.False(req.Stream );
        
        CancellationTokenSource source = new();
        CancellationToken tkn = source.Token;
        

        OllamaApiClient.ChatResponse resp = await  client.GetResponseForPromptAsync(req
            , tkn);

        Assert.NotNull(resp );

    }


    [Fact]
    public async void TestPromptResponseStreaming() 
    {
        OllamaApiClient client = new(endpoint,modelName);

        OllamaApiClient.ChatRequest req = new OllamaApiClient.ChatRequest() {
                Prompt="Is India a Great Country?",
                Stream=true
        };

        Assert.NotNull(client );      

        Assert.NotEqual("what",req.Prompt );
        Assert.NotEqual(false,req.Stream );
        
        CancellationTokenSource source = new CancellationTokenSource();
        CancellationToken tkn = source.Token;
        
        await foreach( OllamaApiClient.ChatResponse resp in client.GetStreamForPromptAsync(req
            , tkn)) { 
                Assert.NotNull(resp);
                System.Diagnostics.Debug.WriteLine(resp.Response);
            }

    }


    [Fact(Skip="chat response code is pending")]
    public void TestChatResponseNotStreaming() 
    {
        OllamaApiClient client = new(endpoint,modelName);

        Assert.NotNull(client );      
    }


    [Fact(Skip="chat response code is pending")]
    public void TestChatResponseStreaming() 
    {
        OllamaApiClient client = new(endpoint,modelName);

        Assert.NotNull(client );      
    }



}
