// Setup basic express server
var express = require('express');
var app = express();
var bodyParser = require('body-parser');
var server = require('http').createServer(app);
var io = require('socket.io')(server);
var port = process.env.PORT || 3000;
var numUsers = 0;
app.use(bodyParser.json());
server.listen(port, function () {
    console.log('Server listening at port %d', port);
});
app.post('/internal/broadcast', function (req, res) {
    console.log(req.body);
    io.sockets.emit('UPDATE_REDUX', req.body);
    res.sendStatus(200);
});
io.on('connection', function (socket) {
    socket.on('add', function () {
        ++numUsers;
        console.log('Someone joined');
        console.log("Total users:" + numUsers);
    });
    socket.on('disconnect', function () {
        --numUsers;
        console.log('Someone left');
        console.log("Total users:" + numUsers);
    });
});
io.origins('*:*');
//# sourceMappingURL=server.js.map