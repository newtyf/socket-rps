require("dotenv").config();
import app from "./app";
import socket from "./socket";
import { Server as WebSocketServer } from "socket.io";
import { createServer } from "http";

const PORT = process.env.PORT || 3000;

const server = createServer(app);
const httpServer = server.listen(PORT);

const io = new WebSocketServer(httpServer, {
  cors: {
    origin: process.env.HOST_WEB,
  },
});

console.log(`listening on http://localhost:${PORT}`);
socket(io)