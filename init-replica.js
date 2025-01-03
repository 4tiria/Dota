rs.initiate({
  _id: "rsdota",
  members: [
    { _id: 0, host: "mongodb:27017" }
  ]
});

db.createUser({
  user: "admin",
  pwd: "admin",
  roles: [{ role: "root", db: "admin" }]
});