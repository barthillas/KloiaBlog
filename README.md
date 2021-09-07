# KloiaCase

There are two microservices on this mono repository.

Each microservice has its own database, MSSql used as DB.

# MSSql setup
```c#
open a new terminal 

# create a network 
docker network create -d bridge kloia-bridge-network

# setup MSSql server
docker run --name mssql --network kloia-bridge-network -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=kloia12345!@#$%' -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-CU10-ubuntu-20.04


```

# Create db
```c#

#open a new terminal  

# connect to MSSql CLI
docker exec -it mssql /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P kloia12345!@#$%

# create databases for each microservices
create database Articles
go
create database Reviews
go

exit
```
# Update db
```c#

# install EntityFramework cli tool globally
dotnet tool install --global dotnet-ef

cd Services/Article/Src/ArticleService.Infrastructure/

dotnet ef database update -c ArticleService.Infrastructure.Context.ArticleDbContext

cd ../../../../

cd Services/Review/Src/ReviewService.Infrastructure/

dotnet ef database update -c ReviewService.Infrastructure.Context.ReviewDbContext

cd ../../../../

```
At this point we have created and updated our databases

# To run on local machine

```c#

#open a new terminal at project root 

cd Services/Article/Src/ArticleService.Api/

dotnet run

cd ../../../../

cd Services/Review/Src/ReviewService.Api/

dotnet run


```
# To run on docker

```c#

#open a new terminal on the project root directory

docker build -t articlesimage -f Services/Article/Src/ArticleService.Api/Dockerfile .
docker run --name articlemicroservice -p 5001:80 --network kloia-bridge-network -e envConnectionString='Server=mssql;Database=Reviews;User Id=sa;Password=kloia12345!@#$%;' articlesimage


#open a new terminal on the project root directory

docker build -t reviewsimage -f Services/Review/Src/ReviewService.Api/Dockerfile .
docker run --name reviewmicroservice -p 5011:80 --network kloia-bridge-network -e envConnectionString='Server=mssql;Database=Reviews;User Id=sa;Password=kloia12345!@#$%;' reviewsimage 

```

# There is a Postman Collection Schema on the root directory if needed.