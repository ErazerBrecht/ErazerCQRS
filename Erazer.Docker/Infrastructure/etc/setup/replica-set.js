rsconf = {
  _id: "rs0",
  members: [
    { _id: 0, host: "mongo-1:3000" },
    { _id: 1, host: "mongo-2:3001" },
  ]
}

rs.initiate(rsconf);
rs.conf();