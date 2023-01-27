function getWinner(userOne, userTwo) {
  if (
    (userOne.inGame.pick.name === "papel" &&
      userTwo.inGame.pick.name === "piedra") ||
    (userOne.inGame.pick.name === "piedra" &&
      userTwo.inGame.pick.name === "tijeras") ||
    (userOne.inGame.pick.name === "tijeras" &&
      userTwo.inGame.pick.name === "papel")
  ) {
    return {
      ...userOne,
      inGame: {
        pick: userOne.inGame.pick,
        points: userOne.inGame.points + 1,
      },
    };
  } else if (userOne.inGame.pick.name === userTwo.inGame.pick.name.name) {
    return { name: "any" };
  } else {
    return {
      ...userTwo,
      inGame: {
        pick: userTwo.inGame.pick,
        points: userTwo.inGame.points + 1,
      },
    };
  }
}

module.exports = { getWinner };
