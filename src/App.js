import './App.css';
import Login from './Login';
import { useState, useEffect } from 'react';


function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  // useEffect(() => {
  

  //   if (email !== '' && password !== '') {
  //     login();
  //   }
  // }, [email, password]);

  const handleEmailChange = (event) => {
    setEmail(event.target.value);
  };

  const handlePasswordChange = (event) => {
    setPassword(event.target.value);
  };

  const handleLogin = async (event) => {
    event.preventDefault()
    const loginEndpoint = 'https://tryitter.azurewebsites.net/login';
    const login = async () => {
      try {
        const response = await fetch(loginEndpoint, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify({ email, password })
        });

        if (response) {
          const data = await response;
          console.log('OK:', data);
        } else {
          console.error('Falha:', response.status);
        }
      } catch (error) {
        console.error('Erro:', error);
      }
    };

    login();
  }


  return (
    <div className="App">
      <header className="App-header">
        {isAuthenticated ?
          <div>oi</div>
          :
          <>
            <form>
              <p>Login</p>
              <div className="mb-3">
                <label htmlFor="exampleInputEmail1" className="form-label"></label>
                <input onChange={handleEmailChange} type="email" className="form-control" id="exampleInputEmail1" aria-describedby="emailHelp" placeholder="usuário" />
              </div>
              <div class="mb-3">
                <label htmlFor="exampleInputPassword1" className="form-label"></label>
                <input onChange={handlePasswordChange} type="password" className="form-control" id="exampleInputPassword1" placeholder="senha" />
              </div>
              <button onClick={handleLogin} className="btn btn-primary">Entrar</button>
            </form>
          </>
        }
      </header>
    </div>
  );
}

export default App;
