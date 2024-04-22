namespace ollamask;


using System.ComponentModel;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Azure.AI.OpenAI;

public class OllamaApiClient 
{

    private HttpClient _client = new();

	public Configuration Config { get; }

	public interface IResponseStreamer<T>
	{
		void Stream(T stream);
	}
	public class ChatMessage { 

			[JsonPropertyName("role")]
			public string Role { get; set;}
			
			[JsonPropertyName("content")]
			public string Content {get;set;}

	}

	public class ChatResponse
	{
		[JsonPropertyName("model")]
		public string Model { get; set; }

		[JsonPropertyName("created_at")]
		public string CreatedAt { get; set; }

		[JsonPropertyName("response")]
		public string Response { get; set; }

		[JsonPropertyName("done")]
		public bool Done { get; set; }
	}

	public class ChatRequest { 
		[JsonPropertyName("model")]
		public string Model { get;set;}

		[JsonPropertyName("prompt")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Prompt {get; set;}


		[JsonPropertyName("format")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Format {get; set;}


		[JsonPropertyName("messages")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public IList<ChatMessage> Messages {get; set;}

		[JsonPropertyName("stream")]
		public bool Stream {get; set;} = false;
	}


    public class Configuration
		{
			public Uri Uri { get; set; }

			public string Model { get; set; }
		}


    public OllamaApiClient(string uriString, string defaultModel = "")
        : this(new Uri(uriString), defaultModel)
		{
		}

    public OllamaApiClient(Uri uri, string defaultModel = "")
			: this(new Configuration { Uri = uri, Model = defaultModel })
		{
		}

    public OllamaApiClient(Configuration config)
			: this(new HttpClient() { BaseAddress = config.Uri }, config.Model)
		{
    		Config = config;

			}

    public OllamaApiClient(HttpClient client, string defaultModel = "")
		{
			_client = client ?? throw new ArgumentNullException(nameof(client));
			_client.Timeout = TimeSpan.FromMinutes(10);

			(Config ??=  new Configuration()).Model = defaultModel;
			
		}

	public async Task<ChatResponse> GetResponseForPromptAsync(ChatRequest message, CancellationToken token) {
		return await PostAsync<ChatRequest,ChatResponse>("/api/generate",message,token);
	}

    private async Task<TResponse> GetAsync<TResponse>(string endpoint, CancellationToken cancellationToken)
		{
			var response = await _client.GetAsync(endpoint, cancellationToken);
			response.EnsureSuccessStatusCode();

			var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
			return JsonSerializer.Deserialize<TResponse>(responseBody);
		}

    private async Task PostAsync<TRequest>(string endpoint, TRequest request, CancellationToken cancellationToken)
		{
			var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
			var response = await _client.PostAsync(endpoint, content, cancellationToken);
			response.EnsureSuccessStatusCode();
		}

    private async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest request, CancellationToken cancellationToken)
		{
			var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
			var response = await _client.PostAsync(endpoint, content, cancellationToken);
			response.EnsureSuccessStatusCode();

			var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

			return JsonSerializer.Deserialize<TResponse>(responseBody);
		}

    // private async Task StreamPostAsync<TRequest, TResponse>(string endpoint, TRequest requestModel, IResponseStreamer<TResponse> streamer, CancellationToken cancellationToken)
	// 	{
	// 		var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
	// 		{
	// 			Content = new StringContent(JsonSerializer.Serialize(requestModel), Encoding.UTF8, "application/json")
	// 		};

	// 		using var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
	// 		response.EnsureSuccessStatusCode();

	// 		await ProcessStreamedResponseAsync(response, streamer, cancellationToken);
	// 	}

    // private static async Task ProcessStreamedResponseAsync<TLine>(HttpResponseMessage response, IResponseStreamer<TLine> streamer, CancellationToken cancellationToken)
	// 	{
	// 		using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
	// 		using var reader = new StreamReader(stream);

	// 		while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
	// 		{
	// 			string line = await reader.ReadLineAsync();
	// 			var streamedResponse = JsonSerializer.Deserialize<TLine>(line);
	// 			streamer.Stream(streamedResponse);
	// 		}
	// 	}

    // private static async Task<ConversationContext> ProcessStreamedCompletionResponseAsync(HttpResponseMessage response, IResponseStreamer<GenerateCompletionResponseStream> streamer, CancellationToken cancellationToken)
	// 	{
	// 		using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
	// 		using var reader = new StreamReader(stream);

	// 		while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
	// 		{
	// 			string line = await reader.ReadLineAsync();
	// 			var streamedResponse = JsonSerializer.Deserialize<GenerateCompletionResponseStream>(line);
	// 			streamer.Stream(streamedResponse);

	// 			if (streamedResponse?.Done ?? false)
	// 			{
	// 				var doneResponse = JsonSerializer.Deserialize<GenerateCompletionDoneResponseStream>(line);
	// 				return new ConversationContext(doneResponse.Context);
	// 			}
	// 		}

	// 		return new ConversationContext(Array.Empty<long>());
	// 	}

	// 	private static async Task<IEnumerable<Message>> ProcessStreamedChatResponseAsync(ChatRequest chatRequest, HttpResponseMessage response, IResponseStreamer<ChatResponseStream> streamer, CancellationToken cancellationToken)
	// 	{
	// 		using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
	// 		using var reader = new StreamReader(stream);

	// 		ChatRole? responseRole = null;
	// 		var responseContent = new StringBuilder();

	// 		while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
	// 		{
	// 			string line = await reader.ReadLineAsync();

	// 			var streamedResponse = JsonSerializer.Deserialize<ChatResponseStream>(line);

	// 			// keep the streamed content to build the last message
	// 			// to return the list of messages
	// 			responseRole ??= streamedResponse?.Message?.Role;
	// 			responseContent.Append(streamedResponse?.Message?.Content ?? "");

	// 			streamer.Stream(streamedResponse);

	// 			if (streamedResponse?.Done ?? false)
	// 			{
	// 				var doneResponse = JsonSerializer.Deserialize<ChatDoneResponseStream>(line);
	// 				var messages = chatRequest.Messages.ToList();
	// 				messages.Add(new Message(responseRole, responseContent.ToString()));
	// 				return messages;
	// 			}
	// 		}

	// 		return Array.Empty<Message>();
	// 	}

}