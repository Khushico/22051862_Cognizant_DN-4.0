import React from 'react';

function Login({ onLogin }) {
  return (
    <div>
      <h2>Login Page</h2>
      <button onClick={onLogin}>Click to Login</button>
    </div>
  );
}

export default Login;
