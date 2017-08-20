// Setup basic express server
const express = require('express');
const app = express();
const bodyParser = require('body-parser');
const server = require('http').createServer(app);
const io = require('socket.io')(server);
const port = process.env.PORT || 3000;

let numUsers = 0;

app.use(bodyParser.json());

server.listen(port, () => {
    console.log('Server listening at port %d', port);
});


app.post('/internal/broadcast', (req, res) => {
    console.log(req);
    console.log(req.body);
    io.sockets.emit('UPDATE_REDUX', req.body);
    res.sendStatus(200);
});


io.on('connection', socket => {
    socket.on('add', () => {
        ++numUsers;
        console.log('Someone joined');
        console.log(`Total users:${numUsers}`);
    });

    socket.on('disconnect', () => {
        --numUsers;
        console.log('Someone left');
        console.log(`Total users:${numUsers}`);
    });
});

io.origins('*:*');