TO RUN THIS PROJECT YOU SHOULD FOLLOW THESE STEP:
# 1. SETUP SQL SERVER ON DOCKER
## Create docker container for sql server
``` powershell
docker pull mcr.microsoft.com/mssql/server
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Admin@123' -p 5433:1433 --name sql_server_container -d mcr.microsoft.com/mssql/server
```

## Create database for write database
Run script in file: `Infrastructure\BookEventStores\seed_post_write_database.sql`

## If you have different configuration for connection string then update in user secrets
``` secrets.json
  "BookWriteDatabaseSettings": {
    "ConnectionString": "Server=.,5433;Database=post_write;User Id=sa;Password=Admin@123;TrustServerCertificate=True"
  }
```

# 2. SETUP MONGODB ON DOCKER
## Create docker container for MongoDB
``` powershell
docker pull mongo

docker run -d -p 27017:27017 --name my-mongo-container mongo
```

## Download [MongoDB Compass](https://www.mongodb.com/try/download/compass) and install

## Connect using MongoDB Compass and create a database name: *post_read* with collections: *books*

## If you ave different configuration for connection string then update in user secrets
``` secrets.json
"BookReadDatabaseSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "book_read",
    "BooksCollectionName": "books"
  }
```

# 3. RUN THE BACKEND
## Just choose profile: *my-app-backend* and start to debug

# 4. RUN THE FRONTEND
## Open project frontend
## Run script
``` Terminal
ng serve --open
```
Enjoy!

# 4. Home work
Base on the feature in [this page](https://ambitious-bay-0a6b5df00.4.azurestaticapps.net), implement the feature: update book, delete book, view statistic (number of create & update & delete performed)


