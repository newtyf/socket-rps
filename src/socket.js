import axios from "axios";
import { existUserInRoom } from "./helpers/existUserInRoom";
import { getUsersInRoom } from "./helpers/getUsersInRoom";
import { getWinner } from "./helpers/getWinner";

export default (io) => {
  io.on("connection", (socket) => {
    console.log(`a user connected ${socket.id}`);

    socket.on("join-room", async (user) => {
      try {
        const { belongsRoom, userId } = user;
        const { data } = await axios.put(
          `${process.env.HOST_API}rooms/joinRoom/${belongsRoom}`,
          { userId }
        );

        if (!data.available && !(await existUserInRoom(userId, belongsRoom))) {
          return socket
            .to(socket.id)
            .emit("full-room", "no pertenece a la sala y la sala esta llena");
        }
        socket.join(belongsRoom);
        socket.to(belongsRoom).emit("join-new-player", user);
      } catch (error) {
        console.log(error);
      }
    });

    socket.on("join-current-user-room", async (user) => {
      const { belongsRoom } = user;
      socket.join(belongsRoom);
      socket.to(belongsRoom).emit("join-new-player", user);
    });

    socket.on("user-picked", async ({ user, room }) => {
      socket.to(room).emit("alert-user-picked", user);
      console.log("emir");
    });

    socket.on("get-winner-game", async (room) => {
      const [socketOne, socketTwo] = await getUsersInRoom(room);

      if (!!socketOne.inGame.pick && !!socketTwo.inGame.pick) {
        const winner = getWinner(socketOne, socketTwo);

        if (winner.name === socketOne.name) {
          io.to(room).emit("winner", winner);
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
  });
};
