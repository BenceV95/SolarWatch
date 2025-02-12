import React, { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import './Login.css'

const Login = () => {

  const [responseMessage, setResponseMessage] = useState('');

  const navigate = useNavigate();

  const handleSubmit = async (event) => {
    event.preventDefault()
    const data = new FormData(event.target)
    const value = Object.fromEntries(data.entries())
    //console.log(JSON.stringify(value))

    const response = await fetch('/api/auth/login', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(value)
    })

    if (response.ok) {
      const result = await response.json();
      localStorage.setItem('username', result.userName);
      localStorage.setItem('email', result.email);
      localStorage.setItem('token', result.token);
      //console.log(result);
      
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
  }

  return (
    <div className='login'>
      <h1>Login</h1>
      {!responseMessage &&
      <>
      
      <form onSubmit={handleSubmit}>
        <input type="text" id="email" name="email" placeholder='Email' />
        <br />
        <input type="password" id="password" name="password" placeholder='Password' />
        <br />
        <button type="submit" className='buttonGreen'>Login</button>
      </form>
      <Link to='/register' className='buttonYellow'>Don't have an account? Register here</Link>
      </>
      }
      <br />
      {responseMessage && <p>{responseMessage}</p>}      
    </div>
  )
}

export default Login