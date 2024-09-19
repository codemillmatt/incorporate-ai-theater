# Incorpoating AI into a .NET Application

When building intelligent apps, you can build them with .NET.

And you don't need to use the massive LLMs like OpenAI's GPT 4 to do so. In this short example, we'll walk you through how to add AI to your .NET application using Microsoft's Phi-3 small language model (SLM).

## Introduction

In this sample you're going to learn how to add intelligence to a .NET application using Semantic Kernel and Microsoft's open source Phi-3 model.

Using Phi-3 is great because it will allow you to add the AI without needing to use a full-fledged LLM. And even better, all you need to run this sample is Docker or GitHub Codespaces.

And because we're using Semantic Kernel - the code we'll create will be the same regardless if we were using one of OpenAI's models, one found on GitHub Models, or Phi-3. The Semantic Kernel SDK will abstract away all of those implementation details so we can work on what matters to us most - our business code.

## Getting started

The easiest way to get stated is to open this repository up in GitHub Codespaces.

Or if you prefer to run everything locally. Clone this repo. Start up Docker. Then open the containing folder in VS Code. When prompted if you want to reopen it in a DevContainer, pick yes.

The DevContainer / Codespace already has everything installed that you will need to work with .NET and Phi-3.

## Simple text completions

The first thing we want to do is add in text completions. A text completion is asking the model a question and having it provide an answer.

A starter .NET console application has been provided in the **src** directory.

1. We need to add a reference to the Semantic Kernel NuGet package. Open a terminal, change to **src** directory and type: `dotnet add package Microsoft.SemanticKernel`
1. 

## Add chat history

## References

- [Phi-3 Microsoft Blog](https://aka.ms/phi3blog-april)
- [Phi-3 Technical Report](https://aka.ms/phi3-tech-report)
- [Phi-3 Cookbook](https://aka.ms/Phi-3CookBook)
- [Generative AI for beginners](https://github.com/microsoft/generative-ai-for-beginners)
- [Semantic Kernel main repository](https://github.com/microsoft/semantic-kernel)
- [Smart Components - Local Embeddings](https://github.com/dotnet-smartcomponents/smartcomponents/blob/main/docs/local-embeddings.md)