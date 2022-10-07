
import './App.css';
import Button from 'react-bootstrap/Button';

const handleClick = () => {
  console.log('button clicked');
};

function App() {
  return (
    <div>
      <h3 className="m-3 d-flex justify-content-center">
        Dota 2
      </h3>
      <Button variant="primary" onClick={handleClick}>Make a request</Button>{' '}

    </div>
  );
}

export default App;
