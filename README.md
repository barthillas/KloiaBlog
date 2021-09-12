# KloiaCase

There are two microservices on this mono repository.

Each microservice has its own database, MSSql used as DB.

# MSSql setup
```
#open a new terminal 

# create a network 
docker network create -d bridge kloia-bridge-network

# setup MSSql server
docker run --name mssql --network kloia-bridge-network -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=kloia12345!@#$%' -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-CU10-ubuntu-20.04
```

# Build and run each project on docker

```
#open a new terminal on the project root directory

docker build -t articlesimage -f Services/Article/Src/ArticleService.Api/Dockerfile .

docker run --name articlemicroservice -p 5000:80 --network kloia-bridge-network articlesimage

#open a new terminal on the project root directory

docker build -t reviewsimage -f Services/Review/Src/ReviewService.Api/Dockerfile .

docker run --name reviewmicroservice -p 5010:80 --network kloia-bridge-network reviewsimage
```

# There is a Postman Collection Schema on the root directory if needed.