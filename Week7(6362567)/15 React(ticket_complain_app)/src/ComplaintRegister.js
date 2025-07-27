import React, { useState } from 'react';

function ComplaintRegister() {
  const [employeeName, setEmployeeName] = useState('');
  const [complaint, setComplaint] = useState('');

  const handleSubmit = (e) => {
    e.preventDefault();

    // Generate random reference number
    const referenceNumber = 'REF-' + Math.floor(1000 + Math.random() * 9000);

    alert(
      `Complaint submitted successfully!\nEmployee: ${employeeName}\nReference Number: ${referenceNumber}`
    );

    // Clear form
    setEmployeeName('');
    setComplaint('');
  };

  return (
    <div style={{ textAlign: 'center', marginTop: '50px' }}>
      <h2>Raise a Complaint</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Employee Name:</label><br />
          <input
            type="text"
            value={employeeName}
            onChange={(e) => setEmployeeName(e.target.value)}
            required
            style={{ padding: '5px', width: '300px', marginTop: '5px' }}
          />
        </div>
        <br />
        <div>
          <label>Complaint:</label><br />
          <textarea
            value={complaint}
            onChange={(e) => setComplaint(e.target.value)}
            required
            rows="5"
            cols="40"
            style={{ padding: '5px', marginTop: '5px' }}
          />
        </div>
        <br />
        <button type="submit" style={{ padding: '10px 20px' }}>
          Submit
        </button>
      </form>
    </div>
  );
}

export default ComplaintRegister;
