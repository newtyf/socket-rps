const axios = require("axios");

const getUsersInRoom = async (room) => {
  const { data } = await axios.get(`${process.env.HOST_API}rooms/${room}`);

  console.log(data.room.integrantes);

  let users = []

  for (let index = 0; index < data.room.integrantes.length; index++) {
    const element = data.room.integrantes[index];
    const res = await axios.get(
      `${process.env.HOST_API}users/${element}`
    );
    users.push(res.data.user)
  }
  return users
};

module.exports = { getUsersInRoom };
