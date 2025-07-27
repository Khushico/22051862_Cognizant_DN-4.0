import React from "react";
import ListofPlayers from "./ListofPlayers";
import IndianPlayers from "./IndianPlayers";

function App() {
  return (
    <div className="App">
      <h1>React App</h1>
      <ListofPlayers />
      <hr style={{ margin: "40px 0" }} />
      <IndianPlayers />
    </div>
  );
}

export default App;
