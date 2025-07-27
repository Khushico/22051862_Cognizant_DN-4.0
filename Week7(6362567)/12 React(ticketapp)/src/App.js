import React, { useState } from 'react';

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);

  let content;
  if (isLoggedIn) {
    content = (
      <div style={{ textAlign: 'center', marginTop: '100px' }}>
        <h1>Welcome back</h1>
        <button onClick={() => setIsLoggedIn(false)}>Logout</button>
      </div>
    );
  } else {
    content = (
      <div style={{ textAlign: 'center', marginTop: '100px' }}>
        <h1>Please sign up.</h1>
        <button onClick={() => setIsLoggedIn(true)}>Login</button>
      </div>
    );
  }

  return <div>{content}</div>;
}

export default App;
