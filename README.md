# Pinecone .NET SDK
[![Pinecone_sdk](https://img.shields.io/nuget/v/pinecone_sdk?style=for-the-badge)](https://www.nuget.org/packages/pinecone_sdk/)


## Description

Pinecone .NET SDK is a C# library that provides a convenient way to interact with Pinecone's Vector Indexing and Search services. This library simplifies the process of connecting, indexing, and searching for vectors within the Pinecone ecosystem. The SDK is available as a package on nuget.org under the name `pinecone_sdk`.

## Features

- Easy integration with Pinecone's Vector Indexing and Search services
- Easy to use with GPT implementations
- Asynchronous API support for better performance
- Comprehensive error handling and logging

## Installation

To install the Pinecone .NET SDK, simply add the `pinecone_sdk` package from nuget.org to your .NET project.

Using Package Manager:

```powershell
Install-Package pinecone_sdk
```

Using .NET CLI:

```csharp
dotnet add package pinecone_sdk
```

## Quick Start

To get started with the Pinecone .NET SDK, follow these steps:

 **Import the namespace:**

```csharp
using pinecone;
```

 **Create a Pinecone client:**
Replace <YOUR_API_KEY> with your actual Pinecone API key and <Environment> with the environment of created index.

```csharp
var pineconeClient = new PineconeProvider("<YOUR_API_KEY>","<Environment>");
```



## Documentation
For more detailed information on the Pinecone .NET SDK, please visit the official documentation.

## Support
If you encounter any issues or require further assistance, please raise an issue in the GitHub repository.

## License
This SDK is distributed under the MIT License.