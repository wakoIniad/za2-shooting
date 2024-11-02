const fs = require('fs')
const express = require('express');
const app = express();
const https = require('https');
const options = {
  //key:  fs.readFileSync('./key.pem'),
  //cert: fs.readFileSync('./cert.pem')
};
const server = https.createServer(options, app);
const io = require('socket.io')(server);
const WsServer = require('ws').Server;
const PORT = process.env.PORT || 3000;
const wss = new WsServer({ noServer: true });

server.removeAllListeners("upgrade");
server.on("upgrade", (req, socket, head) => {
  console.log('Upgrade!');
  wss.handleUpgrade(req, socket, head, (ws) => {
    console.log('ws connection!');
    ws.on('message', (mess) => {
      console.log(mess.toString());
    });
    ws.on('close', () => {
      console.log('ws close');
    });
  });
});

// --- ブラウザ向け ---
app.use(express.static('public'));
io.on('connection', function(socket){
  socket.on('message', function(mess){
    console.log(mess);
    // 例えば他の接続している端末に配信
    io.emit('message', mess);
  });
  socket.on('disconnect', () => {
    console.log('user disconnected');
  });
});
// ---

// サーバの立ち上げ
server.listen(PORT, () => {
  console.log('listening on https://localhost:' + PORT);
});