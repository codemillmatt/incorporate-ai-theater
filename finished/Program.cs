using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Text;

#pragma warning disable SKEXP0001, SKEXP0003, SKEXP0010, SKEXP0011, SKEXP0050, SKEXP0052

var kernelBuilder = Kernel.CreateBuilder();

kernelBuilder.AddOpenAIChatCompletion(
    modelId: "phi3.5",
    endpoint: new Uri("http://localhost:11434"),
    apiKey: "apiKey"
);

Kernel sk = kernelBuilder.Build();

string prompt = "What is the state flower of Washington? Be as brief and only tell me the answer, nothing else is needed.";

var settings = new OpenAIPromptExecutionSettings
{
    MaxTokens = 100,
    Temperature = .7,
    TopP = .5
};
var kernelArguments = new KernelArguments(settings);

var response = sk.InvokePromptStreamingAsync(prompt, kernelArguments);
await foreach (var message in response)
{
    Console.Write(message);
}
Console.WriteLine();


var chatService = sk.GetRequiredService<IChatCompletionService>();

var history = new ChatHistory();
history.AddSystemMessage("You are a useful chatbot. If you don't know an answer, say 'I don't know!'. Always reply in a funny ways. Use emojis if possible.");

while (true)
{
    Console.Write("Question: ");
    var userQ = Console.ReadLine();
    if (string.IsNullOrEmpty(userQ))
    {
        break;
    }
    history.AddUserMessage(userQ);

    var result = chatService.GetStreamingChatMessageContentsAsync(history, settings);
    StringBuilder assistantResponse = new StringBuilder();

    await foreach(var message in result)
    {
        assistantResponse.Append(message.Content);
        Console.Write(message.Content);
    }

    history.AddAssistantMessage(assistantResponse.ToString());
}