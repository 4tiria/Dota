"tools/mongodump.exe" --db dota --out ./data/dump/

docker cp ./data/dump mongodb:/backup
docker exec mongodb mongorestore --db dota /backup/dota

REM docker exec -it dota-api mongorestore --db dota /backup/dota


REM "tools/mongorestore.exe" --db dota2 ./data/dump/dota