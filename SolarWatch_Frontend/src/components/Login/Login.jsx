import React, { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import './Login.css'

const Login = () => {

  const [responseMessage, setResponseMessage] = useState('');
  const [loading, setLoading] = useState(false);
  const [success, setSuccess] = useState(false);
  const navigate = useNavigate();

  const handleSubmit = async (event) => {
    event.preventDefault();
    setLoading(true);
    setSuccess(false);
    setResponseMessage('');

    const data = new FormData(event.target)
    const value = Object.fromEntries(data.entries())
    //console.log(JSON.stringify(value))

    try {
      const response = await fetch('/api/auth/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(value)
      });

      if (response.ok) {
        const result = await response.json();
        localStorage.setItem('username', result.userName);
        localStorage.setItem('email', result.email);
        localStorage.setItem('token', result.token);
        //console.log(result);
        setSuccess(true);
        setResponseMessage('Login successful. Redirecting to home page...');
        setTimeout(() => {
          navigate('/') // Redirect to home page
        }, 2000);


      } else {
        const result = await response.json();
        const errorKey = Object.keys(result)[0];
        const errorDetails = result[errorKey][0];
        const errorMessage = `Failed to log in. ${errorKey}: ${errorDetails}`;
        setResponseMessage(errorMessage);
      }
    } catch (e) {
      setSuccess(false);
      console.log(e);
    }
    finally {
      setLoading(false);
    }
  }

  return (
    <div className='login'>
      <h1>Login</h1>
      {!success &&
        <>
          <form onSubmit={handleSubmit}>
            <input type="text" id="email" name="email" placeholder='Email' required minLength={4} />
            <br />
            <input type="password" id="password" name="password" placeholder='Password' required minLength={6} />
            <br />
            <button type="submit" className='buttonGreen' disabled={loading}>Login</button>
          </form>
          <Link to='/register' className='buttonBlue'>Don't have an account? Register here</Link>
        </>
      }
      <br />
      {responseMessage && <p style={{"color":success ? "green" : "red"}}>{responseMessage}</p>}
    </div>
  )
}

export default Login