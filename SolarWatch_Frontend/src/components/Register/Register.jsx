import React, { useState } from 'react'
import './Register.css'
import { Link, useNavigate } from 'react-router-dom';

const Register = () => {

  const [responseMessage, setResponseMessage] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (event) => {
    event.preventDefault()
    const data = new FormData(event.target)
    const value = Object.fromEntries(data.entries())
    console.log(value)

    const response = await fetch('/api/auth/register', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(value)
    })

    if (response.ok) {
      setResponseMessage(`${value.email} registered successfully. Redirecting to the login page...`);
      setTimeout(() => {
        navigate('/login') // Redirect to home page
      }, 2000);
    } else {
      setResponseMessage('Failed to register user');
      console.error(response);
    }
  }

  return (
    <div className='register'>
      <h1>Register</h1>
      {!responseMessage &&
        <form onSubmit={handleSubmit}>
          <input type="text" id="username" name="username" placeholder='Username' />
          <br />
          <input type="text" id="email" name="email" placeholder='Email' />
          <br />
          <input type="password" id="password" name="password" placeholder='Password' />
          <br />
          <br />
          <button type="submit" className='buttonGreen'>Register</button>
        </form>
      }
      {responseMessage && <p>{responseMessage}</p>}
      <br />
      <Link to="/login" className='buttonYellow'>{responseMessage ? "Login" : "Already have an account? Login"}</Link>
    </div>
  )

}

export default Register