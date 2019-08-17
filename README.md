# Moosesoft.Azure.Servicebus
[![nuget](https://img.shields.io/nuget/v/MooseSoft.Azure.ServiceBus.svg)](https://www.nuget.org/packages/MooseSoft.Azure.ServiceBus/)

## What is it?

Moosesoft.Azure.Servicebus is a .Net Standard 2.0 library that extends the functionality of [Microsoft.Azure.Service](https://github.com/Azure/azure-service-bus) by adding a failure policies to automatically handle any message processing failures.  Failure policies can be configured to use different back off delay strategies for resending messages back to originating queues/topics.

For more information please visit the [wiki](https://github.com/gtmoose32/moosesoft-azure-servicebus/wiki).

## Installing Moosesoft.Azure.Servicebus

```
Install-Package Moosesoft.Azure.Servicebus
```

or via the .NET Core command line interface:

```
dotnet add package Moosesoft.Azure.Servicebus
```
