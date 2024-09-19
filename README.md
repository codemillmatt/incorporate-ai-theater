# Incorporating AI into a .NET application

When building intelligent apps, you can build them with .NET.

And you don't need to use the massive LLMs like OpenAI's GPT 4 to do so. In this short example, we'll walk you through how to add AI to your .NET application using Microsoft's Phi-3 small language model (SLM).

## Introduction

In this sample, you're going to learn how to add intelligence to a .NET application using Semantic Kernel and Microsoft's open-source Phi-3 model.

Using Phi-3 is great because it will allow you to add the AI without needing to use a full-fledged LLM. And even better, all you need to run this sample is Docker or GitHub Codespaces.

And because you're using Semantic Kernel - the code you'll create will be the same regardless of whether you were using one of OpenAI's models, one found on GitHub Models, or Phi-3. The Semantic Kernel SDK will abstract away all those implementation details, so you can work on what matters to you the most - your business code.

## Getting started

The easiest way to get started is to open this repository up in GitHub Codespaces.

Or if you prefer to run everything locally. Clone this repo. Start up Docker. Then, open the containing folder in VS Code. When prompted if you want to reopen it in a DevContainer, pick yes.

The DevContainer / Codespace already has everything installed that you'll need to work with .NET and Phi-3.

## Simple text completions

The first thing you want to do is add text completions. A text completion is asking the model a question and having it provide an answer.

A starter .NET console application has been provided in the **start** directory.

1. You need to add a reference to the Semantic Kernel NuGet package. Open a terminal, change to **start** directory and type: `dotnet add package Microsoft.SemanticKernel --version 1.19.0`
1. Open the **Program.cs** file and remove the `Console.WriteLine("Hello AI!")` and add the following:

    ```csharp
    using Microsoft.SemanticKernel;

    #pragma warning disable SKEXP0001, SKEXP0003, SKEXP0010, SKEXP0011, SKEXP0050, SKEXP0052
    ```

    Semantic Kernel still has some experimental features in it, and by disabling the warnings you are acknowledging that fact.

1. Create an `IKernelBuilder` object. This will allow you to configure where the Semantic Kernel SDK should look to communicate with the model at. You'll also create the `Kernel` itself. This will facilitate the communication to the model. Note that the `modelId` and `endpoint` have been provided via the dev container/Codespace so all you need to do is copy the following text:

    ```csharp
    var kernelBuilder = Kernel.CreateBuilder();

    kernelBuilder.AddOpenAIChatCompletion(
        modelId: "phi3.5",
        endpoint: new Uri("http://localhost:11434"),
        apiKey: "apiKey"
    );

    Kernel sk = kernelBuilder.Build();
    ```

1. Define the prompt, or what you want to ask the model.

    ```csharp
    var prompt = "what is the state flower of Washington? Be brief and only tell me the answer nothing additional needed.";
    ```

1. Then send that prompt to the model using Semantic Kernel's `InvokePromptStreamingAsync` function

    ```csharp
    var response = sk.InvokePromptStreamingAsync(prompt);
    await foreach (var message in response)
    {
        Console.Write(message);
    }
    Console.WriteLine();
    ```

1. Run the application via `dotnet run` in the terminal to see the answer.
1. You may have noticed that even though you've told the model to be brief, it still can be quite verbose. It also seems to ramble a bit too. You can tweak all those parameters with kernel arguments. Add this `using` statement:

    ```csharp
    using Microsoft.SemanticKernel.Connectors.OpenAI;
    ```

1. Then replace `var response = sk.InvokePromptStreamingAsync(prmopt);` with the following:

    ```csharp
    var settings = new OpenAIPromptExecutionSettings
    {
        MaxTokens = 100,
        Temperature = .7,
        TopP = .5
    };
    var kernelArguments = new KernelArguments(settings);

    var response = sk.InvokePromptStreamingAsync(prompt, kernelArguments);
    ```

    Here you're telling the model to limit itself to a 100 token response. And you're also limiting the temperature or the randomness of the return and the TopP or the diversity of the return. Run the app again with `dotnet run` to see the difference.

## Add chat history

So far, you've built an app that lets you ask a predefined question. How much more difficult would it be to add functionality that lets you chat with the model?

1. Start off with adding `using Microsoft.SemanticKernel.ChatCompletion;` to the top of the **Program.cs**.
1. Then you'll leave everything you've done so far alone and start building the chat service at the bottom of the file. Copy these 3 lines:

    ```csharp
    var chatService = sk.GetRequiredService<IChatCompletionService>();

    var history = new ChatHistory();
    history.AddSystemMessage("You are a useful chatbot that provides brief answers. If you don't know an answer, say 'I don't know!'. Use emojis if possible.");
    ```

    Here you are having Semantic Kernel give you an object that will work with Phi-3 for chatting. Then you are adding a system message - or telling the model what you want the chat to be about - in the chat history.

1. Next up, you'll go into an infinite loop to do the following:

    * Have the user enter a question or comment for the model
    * Add that into the `ChatHistory`
    * Send the entire chat history to the model and get a response
    * Display the response
    * Add the response to the chat history
    * See if the user has anything else to say & start all over again.

    ```csharp
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
        var assistantResponse = new System.Text.StringBuilder();

        await foreach(var message in result)
        {
            assistantResponse.Append(message.Content);
            Console.Write(message.Content);
        }

        history.AddAssistantMessage(assistantResponse.ToString());
    }
    ```

    Note that you're sending the `settings` object in. This is so you can control the temperature, topP, and max tokens returned. You may want to adjust those values. You may also want to see how if the model's response gets cut-off midway through due to the token restriction, how you can have it pick right back up because we have a chat history, so it has context of what it was saying.

## References

- [.NET AI Docs](https://learn.microsoft.com/dotnet/ai)
- [Phi-3 Cookbook](https://aka.ms/Phi-3CookBook)
- [Generative AI for beginners](https://github.com/microsoft/generative-ai-for-beginners)
- [Semantic Kernel main repository](https://github.com/microsoft/semantic-kernel)
- [Smart Components - Local Embeddings](https://github.com/dotnet-smartcomponents/smartcomponents/blob/main/docs/local-embeddings.md)
