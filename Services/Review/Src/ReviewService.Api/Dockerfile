#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/Review/Src/ReviewService.Api/ReviewService.Api.csproj", "Services/Review/Src/ReviewService.Api/"]
COPY ["Services/Review/Src/ReviewService.Abstraction/ReviewService.Abstraction.csproj", "Services/Review/Src/ReviewService.Abstraction/"]
COPY ["Services/Article/Src/ArticleService.Abstraction/ArticleService.Abstraction.csproj", "Services/Article/Src/ArticleService.Abstraction/"]
COPY ["Services/Review/Src/ReviewService.Application/ReviewService.Application.csproj", "Services/Review/Src/ReviewService.Application/"]
COPY ["Services/Review/Src/ReviewService.Domain/ReviewService.Domain.csproj", "Services/Review/Src/ReviewService.Domain/"]
COPY ["Services/Review/Src/ReviewService.Infrastructure/ReviewService.Infrastructure.csproj", "Services/Review/Src/ReviewService.Infrastructure/"]
COPY ["Infrastructure/Abstraction/Abstraction.csproj", "Infrastructure/Abstraction/"]
COPY ["Infrastructure/ApiBase/ApiBase.csproj", "Infrastructure/ApiBase/"]
COPY ["Infrastructure/Data/Data.csproj", "Infrastructure/Data/"]

RUN dotnet restore "Services/Review/Src/ReviewService.Api/ReviewService.Api.csproj"
COPY . .
WORKDIR "/src/Services/Review/Src/ReviewService.Api"
RUN dotnet build "ReviewService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ReviewService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReviewService.Api.dll"]