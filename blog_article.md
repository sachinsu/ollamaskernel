
## Introduction 

Artificial Intelligence, especially Large language models (LLMs) are all in high demand. Since OpenAI released ChatGPT, interest has gone up multi-fold. Since 2023, Powerful LLMs can be run on local machines. Local Large Language Models  offer advantages in terms of data privacy and security and can be enriched using enterprise-specific data using Retrieval augmentation generation (RAG).Several tools exist that make it relatively easy to obtain, run and manage such models locally on our machines. Few examples are [Ollama](https://ollama.com/), [Langchain](https://github.com/hwchase17/langchain),  [LocalAI](localai.io). 

[Semantic Kernel](https://github.com/microsoft/semantic-kernel) is an SDK from Microsoft that integrates Large Language Models (LLMs) like OpenAI, Azure OpenAI, and Hugging Face with conventional programming languages like C#, Python, and Java. Semantic Kernel also has plugins that can be chained together to integrate with other tools like Ollama. 

This post describes usage of Ollama to run  model locally, communicate with it using REST API from Semantic kernel SDK. 

## Ollama 

To setup Ollama follow the installation and setup instructions from the Ollama [website](https://ollama.ai). Ollama runs as a service, exposing a REST API on a localhost port.Once installed, you can invoke ollama run <modelname> to talk to this model; the model is downloaded, if not already and cached the first time it's requested.

For the sake of this post, we can use Phi3 model, so run ```ollama run phi3```. This will download phi3 model, if not already, and once done, it will present a prompt. Using this prompt, one can start chatting with the model. 

## Why SemanticKernel ?

As such , Ollama can be integrated with from any application via REST API. Then why go for SemanticKernel SDK?  It provides a simplified integration of AI capabilities into existing applications, lowering the barrier of entry for new developers and supporting the ability to fine-tune models. It supports multiple languages like C#, Python and Java.

## Using Ollama 

Install Ollama by following instructions [here](https://github.com/ollama/ollama/blob/main/README.md#quickstart).Ollama exposes set of REST APIs, check Documentation [here](https://github.com/ollama/ollama/blob/main/docs/api.md). It provides range of functions like get response for Prompt, get Chat response. for Specific operations, it supports streaming and non-streaming response. First step is to download/pull  using ```ollama run phi3```. This will pull, if required, the model and set it up locally. In the end, it will show prompt where user can interact with model. 



## Integrating with SemanticKernel 


## Summary

## Helpful Links
