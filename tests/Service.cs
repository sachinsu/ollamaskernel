using ollamask;

namespace OllamaSK.Tests;


public class ServiceProvider { 

        private const string modelName = "phi3";
    private const string endpoint = "http://localhost:11434";

    public static TextGenerationService GetTextGenerationService() {
        var ollamatext = new TextGenerationService();
        ollamatext.ModelApiEndPoint = endpoint;
        ollamatext.ModelName = modelName;
        return ollamatext;        
    }

    public static OllamaChatCompletionService GetChatCompletionService() {
        var ollamachat = new OllamaChatCompletionService();
        ollamachat.ModelApiEndPoint = endpoint;
        ollamachat.ModelName = modelName;
        return ollamachat;
    }

    public static OllamaApiClient GetApiClient() {
        return new OllamaApiClient(endpoint,modelName);
    }


}