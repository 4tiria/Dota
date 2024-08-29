"tools/mongodump.exe" --db dota --out ./data/dump/
"tools/mongodump.exe" --db dota-auth --out ./data/dump/

docker cp ./data/dump mongodb:/backup
docker exec mongodb mongorestore --db dota /backup/dump/dota
docker exec mongodb mongorestore --db dota-auth /backup/dump/dota-auth

REM docker exec -it dota-api mongorestore --db dota /backup/dota


REM "tools/mongorestore.exe" --db dota2 ./data/dump/dota