var WebSocketServer = require('ws').Server
var wss = new WebSocketServer({
	port: 8080
});
let players = [];
let counter = 0;
let myID = 0;
wss.on('connection', function(ws) {
    console.log("connected");
    players.push(ws);
    myID = counter;
    counter++;
	ws.on('message', function(message) {
		console.log('received: %s', message);
        players.forEach((v,i)=> {;
            if(i !== myID) {
                console.log(myID,i)
                v.send(message);
            }
        });
	});
	//ws.send('This is server');
});