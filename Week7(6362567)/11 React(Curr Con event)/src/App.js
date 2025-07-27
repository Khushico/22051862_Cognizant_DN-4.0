import React, { Component } from "react";
import CurrencyConvertor from "./CurrencyConvertor";

class App extends Component {
  constructor(props) {
    super(props);
    this.state = {
      counter: 0,
      message: "",
    };
  }

  incrementCounter = () => {
    this.setState({ counter: this.state.counter + 1 });
    this.sayHello();
  };

  decrementCounter = () => {
    this.setState({ counter: this.state.counter - 1 });
  };

  sayHello = () => {
    this.setState({ message: "Hello! Welcome to React Events" });
  };

  sayWelcome = (msg) => {
    alert(msg);
  };

  handleSyntheticEvent = (event) => {
    alert("I was clicked");
  };

  render() {
    return (
      <div style={{ padding: "30px", fontFamily: "Arial" }}>
        <h1>React Event Handling Example</h1>

        <h2>Counter Value: {this.state.counter}</h2>

        <button onClick={this.incrementCounter}>Increment</button>{" "}
        <button onClick={this.decrementCounter}>Decrement</button>

        <p>{this.state.message}</p>

        <hr />

        <button onClick={() => this.sayWelcome("Welcome")}>Say Welcome</button>

        <hr />

        <button onClick={this.handleSyntheticEvent}>OnPress</button>

        <hr />

        <CurrencyConvertor />
      </div>
    );
  }
}

export default App;
