import { useParams } from "react-router-dom";
import trainers from "./TrainersMock";

function TrainerDetail() {
  const { id } = useParams();
  const trainer = trainers.find(t => t.TrainerId === id);

  if (!trainer) return <p>Trainer not found</p>;

  return (
    <div>
      <h2>{trainer.Name}</h2>
      <p><strong>Email:</strong> {trainer.Email}</p>
      <p><strong>Phone:</strong> {trainer.Phone}</p>
      <p><strong>Technology:</strong> {trainer.Technology}</p>
      <p><strong>Skills:</strong> {trainer.Skills}</p>
    </div>
  );
}
export default TrainerDetail;
