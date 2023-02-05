const axios = require("axios");

const existUserInRoom = async (userId, belongsRoom) => {
  const { data } = await axios.get(
    `${process.env.HOST_API}rooms/${belongsRoom}`
  );
  const { integrantes } =  data.room;
  const exists = integrantes.filter((integrante) => integrante === userId);

  return exists.length === 1;
};

module.exports = { existUserInRoom };
