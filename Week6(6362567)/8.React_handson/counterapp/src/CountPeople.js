import React from "react";

class CountPeople extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      entryCount: 0,
      exitCount: 0
    };
  }

  UpdateEntry = () => {
    this.setState((prevState) => ({
      entryCount: prevState.entryCount + 1
    }));
  };

  UpdateExit = () => {
    this.setState((prevState) => ({
      exitCount: prevState.exitCount + 1
    }));
  };

  render() {
    return (
      <div style={styles.container}>
        <h1 style={styles.heading}>People Counting App</h1>
        <p style={styles.text}>People Entered: {this.state.entryCount}</p>
        <p style={styles.text}>People Exited: {this.state.exitCount}</p>
        <div>
          <button style={styles.button} onClick={this.UpdateEntry}>Login</button>
          <button style={styles.button} onClick={this.UpdateExit}>Exit</button>
        </div>
      </div>
    );
  }
}

const styles = {
  container: {
    textAlign: "center",
    marginTop: "50px",
    fontFamily: "Arial, sans-serif"
  },
  heading: {
    color: "green",
    fontSize: "28px"
  },
  text: {
    fontSize: "20px",
    margin: "10px"
  },
  button: {
    margin: "10px",
    padding: "10px 20px",
    fontSize: "16px",
    cursor: "pointer",
    backgroundColor: "#4CAF50",
    color: "white",
    border: "none",
    borderRadius: "5px"
  }
};

export default CountPeople;
