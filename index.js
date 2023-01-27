require('dotenv').config()

const express = require("express");
const cors = require("cors");
const app = express();
const http = require("http");
const server = http.createServer(app);
const { Server } = require("socket.io");
const { getWinner } = require("./helpers/getWinner");

const PORT = process.env.PORT || 3000;

const io = new Server(server, {
  cors: {
    origin: process.env.HOST_WEB,
  },
});

app.get("/", (req, res) => {
  // res.sendFile(__dirname + "/public/index.html");
  res.send("socket");
});

io.on("connection", (socket) => {
  console.log(`a user connected ${socket.id}`);

  socket.on("create-room", async (user) => {
    const id = new Date().getTime().toString();
    socket.join(id);
    socket.data = user;
    io.to(socket.id).emit("send-id", id);
  });

  socket.on("join-room", async (data) => {
    const { room, newUser } = data;
    socket.join(room);
    socket.data = newUser;
    const socketsConnect = await socket.in(room).fetchSockets();
    socket.to(room).emit("join-room", newUser);
    io.to(socket.id).emit("join-room", socketsConnect[0].data);
  });

  socket.on("user-picked", async ({user, room}) => {
    socket.data = user;
    // const socketsConnect = await socket.in(room).fetchSockets();
    socket.to(room).emit("alert-user-picked", user);
    console.log("emir");
  });

  socket.on("get-winner-game", async (room) => {
    const [socketOne, socketTwo] = await io.in(room).fetchSockets();
    if (!!socketOne.data.inGame.pick && !!socketTwo.data.inGame.pick) {
      const winner = getWinner(socketOne.data, socketTwo.data);

      if (winner.name === socketOne.data.name) {
        io.to(room).emit("winner", winner)
      }

      if (winner.name === "any") {
        io.to(room).emit("winner", {
          name: `EMPATE`,
        });
      } else {
        io.to(room).emit("winner", winner);
      }
    }
  });

  socket.on("update-user", (user) => {
    socket.data = user
  })
  // socket.on("disconnect", () => {
  //   console.log("discon");
  // });
});

server.listen(PORT, () => {
  console.log("listening on *:3000");
});
