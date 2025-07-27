import React, { Component } from "react";

class CurrencyConvertor extends Component {
  constructor(props) {
    super(props);
    this.state = {
      rupees: "",
      euro: "",
    };
  }

  handleChange = (event) => {
    this.setState({ rupees: event.target.value });
  };

  handleSubmit = (event) => {
    event.preventDefault();
    const rate = 0.011; // 1 INR = 0.011 EUR
    const result = (this.state.rupees * rate).toFixed(2);
    this.setState({ euro: result });
  };

  render() {
    return (
      <div>
        <h2>Currency Convertor</h2>
        <form onSubmit={this.handleSubmit}>
          <label>Enter amount in Rupees: </label>
          <input
            type="number"
            value={this.state.rupees}
            onChange={this.handleChange}
          />
          <button type="submit">Convert</button>
        </form>
        {this.state.euro && (
          <p>
            <strong>{this.state.rupees} INR = {this.state.euro} EUR</strong>
          </p>
        )}
      </div>
    );
  }
}

export default CurrencyConvertor;
