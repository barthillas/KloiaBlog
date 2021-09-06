#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/Article/Src/ArticleService.Abstraction/ArticleService.Abstraction.csproj", "Services/Article/Src/ArticleService.Abstraction/"]
COPY ["Services/Review/Src/ReviewService.Abstraction/ReviewService.Abstraction.csproj", "Services/Review/Src/ReviewService.Abstraction/"]
COPY ["Services/Article/Src/ArticleService.Api/ArticleService.Api.csproj", "Services/Article/Src/ArticleService.Api/"]
COPY ["Services/Article/Src/ArticleService.Application/ArticleService.Application.csproj", "Services/Article/Src/ArticleService.Application/"]
COPY ["Services/Article/Src/ArticleService.Domain/ArticleService.Domain.csproj", "Services/Article/Src/ArticleService.Domain/"]
COPY ["Services/Article/Src/ArticleService.Infrastructure/ArticleService.Infrastructure.csproj", "Services/Article/Src/ArticleService.Infrastructure/"]
COPY ["Infrastructure/Abstraction/Abstraction.csproj", "Infrastructure/Abstraction/"]
COPY ["Infrastructure/ApiBase/ApiBase.csproj", "Infrastructure/ApiBase/"]
COPY ["Infrastructure/Data/Data.csproj", "Infrastructure/Data/"]
#RUN dotnet restore "Infrastructure/Abstraction/Abstraction.csproj"
#RUN dotnet restore "Infrastructure/ApiBase/ApiBase.csproj"
#RUN dotnet restore "Infrastructure/Data/Data.csproj"
#RUN dotnet restore "Services/Article/Src/ArticleService.Abstraction/ArticleService.Abstraction.csproj"
#RUN dotnet restore "Services/Article/Src/ArticleService.Infrastructure/ArticleService.Infrastructure.csproj"
RUN dotnet restore "Services/Article/Src/ArticleService.Api/ArticleService.Api.csproj"
#RUN dotnet restore "Services/Article/Src/ArticleService.Application/ArticleService.Application.csproj"
#RUN dotnet restore "Services/Article/Src/ArticleService.Domain/ArticleService.Domain.csproj"



COPY . .
WORKDIR "/src/Services/Article/Src/ArticleService.Api"
RUN dotnet build "ArticleService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ArticleService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ArticleService.Api.dll"]