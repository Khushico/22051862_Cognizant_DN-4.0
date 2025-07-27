import React from "react";
import Cart from "./Cart";

class OnlineShopping extends React.Component {
  render() {
    const items = [
      { Itemname: "Laptop", Price: 80000 },
      { Itemname: "TV", Price: 120000 },
      { Itemname: "Washing Machine", Price: 50000 },
      { Itemname: "Mobile", Price: 30000 },
      { Itemname: "Fridge", Price: 70000 }
    ];

    return (
      <div style={styles.container}>
        <h1 style={styles.heading}>Items Ordered :</h1>
        <table style={styles.table}>
          <thead>
            <tr>
              <th style={styles.header}>Name</th>
              <th style={styles.header}>Price</th>
            </tr>
          </thead>
          <tbody>
            {items.map((item, index) => (
              <Cart key={index} Itemname={item.Itemname} Price={item.Price} />
            ))}
          </tbody>
        </table>
      </div>
    );
  }
}

const styles = {
  container: {
    textAlign: "center",
    marginTop: "50px"
  },
  heading: {
    color: "green",
    fontSize: "32px"
  },
  table: {
    margin: "0 auto",
    border: "1px solid #888",
    borderCollapse: "collapse",
    width: "400px"
  },
  header: {
    border: "1px solid #888",
    padding: "10px",
    backgroundColor: "#f5f5f5",
    color: "green",
    fontWeight: "bold",
    fontSize: "20px"
  }
};

export default OnlineShopping;
