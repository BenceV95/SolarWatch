import React, { useEffect, useState } from 'react'
import { Navigate } from 'react-router-dom'
import { jwtDecode } from "jwt-decode";

const ProtectedRoute = ({ children, role }) => {

  const [isAuthorized, setIsAuthorized] = useState(null);

  const token = localStorage.getItem('token');

  useEffect(() => {

    if (!token) {
      setIsAuthorized(false);
      return;
    }

    const endpoint = role === 'admin' ? '/api/auth/testAdmin' : '/api/auth/testUser';
    fetch(endpoint, {
      headers: {
        'Authorization': `Bearer ${token}`
      }
    })
      .then(response => {
        if (response.ok) {
          setIsAuthorized(true);
        } else {
          setIsAuthorized(false);
        }
      })
      .catch(() => setIsAuthorized(false));
  }, [token, role])


  if (isAuthorized === null) {
    return <div className='loading'>Loading...</div>;
  }

  if (!isAuthorized) {
    return <Navigate to="/unauthorized" />;
  }

  return children;
}

export default ProtectedRoute