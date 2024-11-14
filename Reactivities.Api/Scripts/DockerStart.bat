docker build --tag reactivities-api ../

docker run --name reactivities-api -p 6001:8080 -p 5001:8080 -e ConnectionStrings__PostgresDb="Host=localhost;Port=5432;Database=reactivities;Username=admin;Password=mysecretpassword" -e Authorization__AccessTokenKey=GKARLzCyZDYerPpZA6xcwFCQDKDdwy4nMqMt9vrJSPGAMpDyWxmhy7sTARr4QtqL --detach reactivities-api