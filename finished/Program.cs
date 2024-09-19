using Microsoft.SemanticKernel;

#pragma warning disable SKEXP0001, SKEXP0003, SKEXP0010, SKEXP0011, SKEXP0050, SKEXP0052

var kernelBuilder = Kernel.CreateBuilder();

kernelBuilder.AddOpenAIChatCompletion(
    modelId: "phi3.5",
    endpoint: new Uri("http://localhost:11434"),
    apiKey: "apiKey"
);

Kernel sk = kernelBuilder.Build();

var response = sk.InvokePromptStreamingAsync("tell me a joke");
await foreach (var message in response)
{
    Console.WriteLine(message);
}