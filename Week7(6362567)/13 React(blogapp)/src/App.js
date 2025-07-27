import React from 'react';
import CourseDetails from './components/CourseDetails';
import BookDetails from './components/BookDetails';
import BlogDetails from './components/BlogDetails';

function App() {
  return (
    <div style={{
      display: 'flex',
      justifyContent: 'space-around',
      padding: '50px',
      fontFamily: 'Arial'
    }}>
      <CourseDetails />
      <div style={{ borderLeft: '3px solid green', height: 'auto' }}></div>
      <BookDetails />
      <div style={{ borderLeft: '3px solid green', height: 'auto' }}></div>
      <BlogDetails />
    </div>
  );
}

export default App;
