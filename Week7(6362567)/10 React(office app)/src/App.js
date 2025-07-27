import React from "react";

function App() {
  // Office object
  const office = {
    name: "DBS",
    rent: 50000,
    address: "Chennai",
    image: "/images/dbs.jpg", // Make sure this image exists in public/images/
  };

  return (
    <div
      style={{
        fontFamily: "Arial, sans-serif",
        textAlign: "center",
        marginTop: "40px",
      }}
    >
      <h1>Office Space , at Affordable Range</h1>

      {/* Office Image */}
      <img
        src={office.image}
        alt={office.name}
        style={{ width: "300px", height: "200px", objectFit: "cover", margin: "20px 0" }}
      />

      {/* Name */}
      <h2 style={{ color: office.rent < 60000 ? "red" : "green" }}>
        Name: {office.name}
      </h2>

      {/* Rent */}
      <p style={{ fontSize: "18px", fontWeight: "bold", color: "red" }}>
        Rent: Rs. {office.rent}
      </p>

      {/* Address */}
      <p style={{ fontSize: "16px" }}>
        <strong>Address:</strong> {office.address}
      </p>
    </div>
  );
}

export default App;
