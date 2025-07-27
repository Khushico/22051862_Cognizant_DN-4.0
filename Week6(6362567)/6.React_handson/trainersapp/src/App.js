import { BrowserRouter, Routes, Route, Link } from "react-router-dom";
import Home from "./components/Home";
import TrainersList from "./components/TrainersList";
import TrainerDetail from "./components/TrainerDetail";
import trainers from "./components/TrainersMock";

function App() {
  return (
    <BrowserRouter>
      <div>
        <nav>
          <Link to="/">Home</Link> |{" "}
          <Link to="/trainers">Trainers</Link>
        </nav>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/trainers" element={<TrainersList trainers={trainers} />} />
          <Route path="/trainers/:id" element={<TrainerDetail />} />
        </Routes>
      </div>
    </BrowserRouter>
  );
}
export default App;
