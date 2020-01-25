# DORYWCZA API - [LINK](https://dorywcza.azurewebsites.net/swagger/index.html) TO AZURE WEBSITE

Server-side app providing Web API created with .NET Core

## Table of Contents

* [General info](#general-info)
* [Technologies](#technologies)
* [Run](#run)

## General info

This Web API:
- serves and receives data stored in Azure SQL Database to Dorywcza Client App([link](https://dorywcza-client.azurewebsites.net)) using HTTP requests;
- sends emails to users;
- authenticates user during registration and logging in;
- migrates data beetween controllers and database;

## Technologies

App is basically based on .NET Core 3, however some feature required additonal packages, such as:
- sending emails are performed by MailKit (package suggested by Microsoft);
- ORM tool provided by Entity Framework Core;
- JSON Web Token authentication provided by Microsoft Authentication JWT Bearer;
- mapping user model with AutoMapper;
- receving request from client in JSON format with NewtonsoftJson;
- Swagger for testing with Swashbuckle;

Additionaly app can be containerized as docker files (docker-compose) are set up.

## Run

API is deployed on Azure and can be tested with Swagger under following [link](https://dorywcza.azurewebsites.net/swagger/index.html)

To download docker image, use command

```
docker pull dorywcza.azurecr.io/dorywcza-api:fist_push
```
