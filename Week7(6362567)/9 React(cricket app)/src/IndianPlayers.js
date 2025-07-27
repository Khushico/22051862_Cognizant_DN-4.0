import React from "react";

const IndianPlayers = () => {
  const oddPlayers = [
    { position: "First", name: "Sachin1" },
    { position: "Third", name: "Virat3" },
    { position: "Fifth", name: "Yuvaraj5" },
  ];

  const evenPlayers = [
    { position: "Second", name: "Dhoni2" },
    { position: "Fourth", name: "Rohit4" },
    { position: "Sixth", name: "Raina6" },
  ];

  const T20players = ["Mr. First Player", "Mr. Second Player", "Mr. Third Player"];
  const RanjiTrophyPlayers = ["Mr. Fourth Player", "Mr. Fifth Player", "Mr. Sixth Player"];
  const mergedPlayers = [...T20players, ...RanjiTrophyPlayers];

  return (
    <div style={{ padding: "20px" }}>
      <h2>Odd Players</h2>
      {oddPlayers.map((player, index) => (
        <p key={index}>{player.position} : {player.name}</p>
      ))}
      <hr />
      <h2>Even Players</h2>
      {evenPlayers.map((player, index) => (
        <p key={index}>{player.position} : {player.name}</p>
      ))}
      <hr />
      <h2>List of Indian Players Merged:</h2>
      <ul>
        {mergedPlayers.map((player, index) => (
          <li key={index}>{player}</li>
        ))}
      </ul>
    </div>
  );
};

export default IndianPlayers;
