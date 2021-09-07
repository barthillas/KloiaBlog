## KloiaCase

There are two microservices on this mono repository.

Each microservice has its own database, MSSql used as DB.

To run the project, you just need to run `build.sh` on project root.

If you got an error just manually check and follow the steps below.


```c#
# create a network 
docker network create -d bridge kloia-bridge-network

# setup MSSql server
docker run --name mssql --network kloia-bridge-network -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=kloia12345!@#$%' -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-CU10-ubuntu-20.04

# connect to MSSql CLI
docker exec -it mssql /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P kloia12345!@#$%

# create db for each microservices
create database Articles
go
create database Reviews
go

exit

# install EntityFramework globally

dotnet tool install --global dotnet-ef

# update databases

open a new terminal at project root 

cd Services/Article/Src/ArticleService.Infrastructure/

dotnet ef database update -c ArticleService.Infrastructure.Context.ArticleDbContext

cd ../../../../

cd Services/Review/Src/ReviewService.Infrastructure/

dotnet ef database update -c ReviewService.Infrastructure.Context.ReviewDbContext

cd ../../../../

```
At this point we have created and updated our databases

#To run on local machine

```c#

open a new terminal at project root 

cd Services/Article/Src/ArticleService.Api/

dotnet run

cd ../../../../

cd Services/Review/Src/ReviewService.Api/

dotnet run


```
#To run on docker

```c#


To access database on a docker network we need to use container name of MSSql

default-connectionString : "Server=localhost;Database=Articles;User Id=sa;Password=kloia12345!@#$%;"
docker-connectionString : "Server=mssql;Database=Articles;User Id=sa;Password=kloia12345!@#$%;"  

manually change Server parameter value "localhost" to "mssql" from all paths below.

Services/Review/Src/ReviewService.Infrastructure/Context/ReviewDbContext.cs 
Services/Article/Src/ArticleService.Infrastructure/Context/ArticleDbContext.cs 
Services/Article/Src/ArticleService.Api/appsettings.json
Services/Review/Src/ReviewService.Api/appsettings.json

open a new terminal on the project root directory

docker build -t articlesimage -f Services/Article/Src/ArticleService.Api/Dockerfile .
docker run --name articlemicroservice -p 5001:80 --network kloia-bridge-network articlesimage


open a new terminal on the project root directory

docker build -t reviewsimage -f Services/Review/Src/ReviewService.Api/Dockerfile .
docker run --name reviewmicroservice -p 5011:80 --network kloia-bridge-network reviewsimage

```

There is a Postman Collection Schema on the root directory if needed.


