import React, { useEffect, useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import './Navbar.css'
import { jwtDecode } from "jwt-decode";

const Navbar = () => {

    const [username, setUsername] = useState('')
    const [tokenExpiry, setTokenExpiry] = useState(null)
    const [timeLeft, setTimeLeft] = useState('')
    const [isAdmin, setIsAdmin] = useState(false);

    const navigate = useNavigate()

    const token = localStorage.getItem('token');

    useEffect(() => {

        if (token) {
            const decodedToken = jwtDecode(token)
            setUsername(localStorage.getItem('username'))
            setTokenExpiry(decodedToken.exp * 1000) // Convert to milliseconds

            fetch('/api/auth/testAdmin', {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            })
                .then(response => {
                    if (response.ok) {
                        setIsAdmin(true);
                    } else {
                        setIsAdmin(false);
                    }
                })
            .catch(() => setIsAdmin(false));

            const updateTimer = () => {
                const now = new Date().getTime()
                const timeLeft = decodedToken.exp * 1000 - now
                if (timeLeft <= 0) {
                    logout()
                } else {
                    setTimeLeft(formatTimeLeft(timeLeft))
                }
            }

            updateTimer() // Update immediately
            const interval = setInterval(updateTimer, 1000)

            return () => clearInterval(interval)
        }
    }, [token])

    const logout = () => {
        localStorage.removeItem('username')
        localStorage.removeItem('email')
        localStorage.removeItem('token')
        setUsername('')
        setTokenExpiry(null)
        navigate('/login')
    }

    const renewToken = async () => {
        const response = await fetch('/api/auth/renew-token', {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('token')}`
            }
        })

        if (response.ok) {
            const result = await response.json()
            localStorage.setItem('token', result.token)
            const decodedToken = jwtDecode(result.token)
            setTokenExpiry(decodedToken.exp * 1000) // Convert to milliseconds
        } else {
            logout()
            const result = await response.text()
            console.error(result)
        }
    }

    const formatTimeLeft = (timeLeft) => {
        const minutes = Math.floor((timeLeft % (1000 * 60 * 60)) / (1000 * 60))
        const seconds = Math.floor((timeLeft % (1000 * 60)) / 1000)
        return `${minutes}m ${seconds}s`
    }

    return (
        <nav className='navbar'>
            <Link to="/" className='buttonBlue'>Home</Link>
            <Link to="/solar-watch" className='buttonBlue'>SolarWatch</Link>
            {isAdmin && <Link to="/admin" className='buttonYellow'>Admin Panel</Link>}
            {username ? (
                <>
                    <span className='nav-user'>Hello, <b>{username.slice(0, 1).toUpperCase() + username.slice(1)}</b></span>
                    <button onClick={logout} className='buttonRed'>Logout</button>
                    {tokenExpiry && (
                        <span className='nav-timer'>
                            Token expires in: {timeLeft}
                            {tokenExpiry - new Date().getTime() < 3 * 60 * 1000 && (
                                <button onClick={renewToken} className='buttonYellow'>Stay logged in?</button>
                            )}
                        </span>
                    )}
                </>
            ) : (
                <Link to="/login" className='buttonGreen'>Login</Link>
            )}
        </nav>
    )
}

export default Navbar