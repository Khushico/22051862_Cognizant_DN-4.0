import React from 'react';
import '../Stylesheets/mystyle.css';

function CalculateScore() {
  const name = "Steeve";
  const school = "DNV Public School";
  const total = 284;
  const goal = 300;

  const average = (total / goal) * 100;

  return (
    <div className="score-container">
      <h1>Student Details:</h1>
      <p className="detail"><span className="name">Name:</span> Steeve</p>
      <p className="detail"><span className="school">School:</span> {school}</p>
      <p className="detail"><span className="total">Total:</span> {total} Marks</p>
      <p className="detail"><span className="score">Score:</span> {average.toFixed(2)}%</p>
    </div>
  );
}

export default CalculateScore;
