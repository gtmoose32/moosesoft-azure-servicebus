# Moosesoft.Azure.Servicebus
[![Build status](https://gtmoose.visualstudio.com/Mathis%20Home/_apis/build/status/Moosesoft.Azure.ServiceBus%20-%20CICD)](https://gtmoose.visualstudio.com/Mathis%20Home/_build/latest?definitionId=10)
[![nuget](https://img.shields.io/nuget/v/Moosesoft.Azure.ServiceBus.svg)](https://www.nuget.org/packages/Moosesoft.Azure.ServiceBus/)

## What is it?

Moosesoft.Azure.Servicebus is a .Net Standard 2.0 library that extends the functionality of [Microsoft.Azure.Service](https://github.com/Azure/azure-service-bus) by adding failure policies to automatically handle any message processing failures.  Failure policies can be configured to use different back off delay strategies for resending messages back to originating queues/topics.

For more information please visit the [wiki](https://github.com/gtmoose32/moosesoft-azure-servicebus/wiki).

## Installing Moosesoft.Azure.Servicebus

```
Install-Package Moosesoft.Azure.Servicebus
```

or via the .NET Core command line interface:

```
dotnet add package Moosesoft.Azure.Servicebus
```
