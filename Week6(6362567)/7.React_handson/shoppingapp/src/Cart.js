import React from "react";

class Cart extends React.Component {
  render() {
    return (
      <tr>
        <td style={styles.cell}>{this.props.Itemname}</td>
        <td style={styles.cell}>{this.props.Price}</td>
      </tr>
    );
  }
}

const styles = {
  cell: {
    border: "1px solid #888",
    padding: "10px",
    textAlign: "center",
    color: "green",
    fontSize: "18px"
  }
};

Cart.defaultProps = {
  Itemname: "Unknown Item",
  Price: 0
};

export default Cart;
