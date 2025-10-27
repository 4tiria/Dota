## Excuse
This repository is in the middle of refactoring. An old architecture is being transformed into clean architecture.
A lot of bad choices were taken years ago...
Also all the passwords are exposed.

# Dota Statistics
The project has nothing of value, this is simply an experimental project, in which I learn and use different technologies.

## Setup with Docker
1. Type in cmd in root directory:
```bash
docker compose up -d
```

At this stage not all the projects are supposed to be active, some are likely to fail because of mongo had not being set up yet.

2. Create Replica Set for MongoDb:
```bash
docker exec -it mongo1 mongosh -u admin -p admin --authenticationDatabase admin
```

```
rs.initiate({
  _id: "rsdota",
  members: [
    { _id: 0, host: "mongo1:27017" },
    { _id: 1, host: "mongo2:27017" },
    { _id: 2, host: "mongo3:27017" }
  ]
})
```

3. Restart all the offline containers
They should work if `2` has finished successfully


## Open and use
`localhost:3000`

It works without authentication for now.

Be aware that `Dota.Generator` container spams new accounts every `30` seconds.
