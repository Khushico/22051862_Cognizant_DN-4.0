import React from 'react';

function Logout({ onLogout }) {
  return (
    <div>
      <h2>User Page</h2>
      <p>You are logged in and can book tickets.</p>
      <button onClick={onLogout}>Click to Logout</button>
    </div>
  );
}

export default Logout;
