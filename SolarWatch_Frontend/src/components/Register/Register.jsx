import React, { useState } from 'react'
import './Register.css'
import { Link, useNavigate } from 'react-router-dom';

const Register = () => {

  const [responseMessage, setResponseMessage] = useState("");
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
    console.log(value)
    try {
      const response = await fetch('/api/auth/register', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(value)
      });
  
      if (response.ok) {
        setResponseMessage(`${value.email} registered successfully. Redirecting to the login page...`);
        setSuccess(true);
        setTimeout(() => {
          navigate('/login') // Redirect to home page
        }, 2000);
      } else {
        throw new Error(response);
      }
    } catch (e) {
      console.log(e);      
      setResponseMessage('Failed to register. Try Again Later!');
    }
    finally {
      setLoading(false);
    }
    
  }

  return (
    <div className='register'>
      <h1>Register</h1>
      {!success &&
        <form onSubmit={handleSubmit}>
          <input type="text" id="username" name="username" placeholder='Username' required minLength={4} />
          <br />
          <input type="text" id="email" name="email" placeholder='Email' required minLength={6} />
          <br />
          <input type="password" id="password" name="password" placeholder='Password' required minLength={6} />          
          <br />
          <br />
          <button type="submit" className='buttonGreen' disabled={loading && !success} >Register</button>
          <br />
          <br />
          <Link to="/tos" className='buttonYellow'>Please read before registering !</Link>
        </form>
      }
      {responseMessage && <p style={{"color":success ? "green" : "red"}}>{responseMessage}</p>}
      <br />
      <Link to="/login" className='buttonBlue'>{success ? "Login" : "Already have an account? Login"}</Link>
    </div>
  )

}

export default Register