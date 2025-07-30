@echo off
:: Windows-совместимый .bat скрипт
echo Ждём запуска mongo1...
timeout /t 10 > nul

docker exec -i mongo1 mongosh -u admin -p admin --authenticationDatabase admin ^
  --eval "rs.initiate({
    _id: 'rsdota',
    members: [
      { _id: 0, host: 'mongo1:27017' },
      { _id: 1, host: 'mongo2:27017' },
      { _id: 2, host: 'mongo3:27017' }
    ]
  })"