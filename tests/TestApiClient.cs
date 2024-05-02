namespace OllamaSK.Tests;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ollamask;
using Azure.AI.OpenAI;

public class TestApiClient
{

    [Fact]
    public void TestClientCreation() 
    {
        OllamaApiClient client = ServiceProvider.GetApiClient();

        Assert.NotNull(client );      
    }

    [Fact]
    public async void TestEmbeddingsResponseNotStreaming() 
    {
        OllamaApiClient client = ServiceProvider.GetApiClient();

        OllamaApiClient.ChatRequest req = new OllamaApiClient.ChatRequest() {
                Prompt="What is JDK?"
        };

        Assert.NotNull(client );      

        Assert.False(req.Stream );
        
        CancellationTokenSource source = new();
        CancellationToken tkn = source.Token;
        

        OllamaApiClient.ChatResponse resp = await  client.GetEmbeddingsAsync(req
            , tkn);

        Assert.NotNull(resp );
        Assert.True(resp.Embeddings.Count > 0);

    }



    [Fact]
    public async void TestPromptResponseNotStreaming() 
    {
        OllamaApiClient client = ServiceProvider.GetApiClient();

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
    public async void TestChatResponseNotStreaming() 
    {
        OllamaApiClient client = ServiceProvider.GetApiClient();

        List<OllamaApiClient.ChatMessage> mesgs = new() {
            new OllamaApiClient.ChatMessage(){Role="system",Content="What is sixth sense?"}
        };

        OllamaApiClient.ChatRequest req = new OllamaApiClient.ChatRequest() {
                Messages= mesgs
        };

        Assert.NotNull(client );      

        Assert.NotNull( req.Messages );
    
        Assert.False(req.Stream );
        
        CancellationTokenSource source = new();
        CancellationToken tkn = source.Token;
        

        OllamaApiClient.ChatResponse resp = await  client.GetResponseForChatAsync(req
            , tkn);

        Assert.NotNull(resp );
        Assert.NotNull(resp.Message);
        Assert.NotEmpty(resp.Message.Content);
    }



    [Fact]
    public async void TestPromptResponseStreaming() 
    {
        OllamaApiClient client = ServiceProvider.GetApiClient();

        OllamaApiClient.ChatRequest req = new OllamaApiClient.ChatRequest() {
                Prompt="Is India a Great Country?",
                Stream=true
        };

        Assert.NotNull(client );      

        Assert.NotEqual("what",req.Prompt );
        Assert.True(req.Stream );
        
        CancellationTokenSource source = new CancellationTokenSource();
        CancellationToken tkn = source.Token;
        
        await foreach( OllamaApiClient.ChatResponse resp in client.GetStreamForPromptAsync(req
            , tkn)) { 
                Assert.NotNull(resp);
                System.Diagnostics.Debug.WriteLine(resp.Response);
            }

    }



    [Fact]
    public async void TestChatResponseStreaming() 
    {
        OllamaApiClient client = ServiceProvider.GetApiClient();

        List<OllamaApiClient.ChatMessage> mesgs = new() {
            new OllamaApiClient.ChatMessage(){Role="user",Content="What is sixth sense?"}
        };

        OllamaApiClient.ChatRequest req = new OllamaApiClient.ChatRequest() {
                Messages= mesgs,
                Stream=true
        };


    
        Assert.NotNull(client );      

        Assert.NotNull(req.Messages );
        Assert.True(req.Stream );
        
        CancellationTokenSource source = new CancellationTokenSource();
        CancellationToken tkn = source.Token;
        
        await foreach( OllamaApiClient.ChatResponse resp in client.GetStreamForChatAsync(req
            , tkn)) { 
                Assert.NotNull(resp);   
                Assert.NotNull(resp.Message);
                System.Diagnostics.Debug.WriteLine(resp.Message.Content);
            }

    }



}
