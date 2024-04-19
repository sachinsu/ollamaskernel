namespace OllamaSK.Tests;
using System;
using Xunit;
using ollamask;


public class TestApiClient
{
    [Fact]
    public void TestClientCreation() 
    {
        OllamaApiClient client = new("http://localhost:11434","gemma:2b");

        Assert.NotEqual(client, null);      
    }
}