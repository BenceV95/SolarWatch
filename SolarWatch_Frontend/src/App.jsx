import { useState } from 'react'
import { Routes, Route } from 'react-router-dom'
import './App.css'

import Home from './components/Home/Home'
import Login from './components/Login/Login'
import Register from './components/Register/Register'
import ProtectedRoute from './components/ProtectedRoute/ProtectedRoute'
import Unauthorized from './components/ProtectedRoute/Unauthorized'
import SolarWatch from './components/SolarWatch/SolarWatch'
import NotFound from './components/NotFound/NotFound'
import Navbar from './components/Navbar/Navbar'
import Footer from './components/Footer/Footer'
import AdminPanel from './components/AdminPanel/AdminPanel'
import Tos from './components/Tos/Tos'

function App() {


  return (
    <>
      <Navbar />
      <div className="main-content">
        <Routes>
          <Route path='/' element={<Home />} />
          <Route path='/tos' element={<Tos />} />
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route path="/unauthorized" element={<Unauthorized />} />
          <Route
            path="/solar-watch"
            element={
              <ProtectedRoute role="user">
                <SolarWatch />
              </ProtectedRoute>
            }
          />
          <Route
            path='/admin'
            element={
              <ProtectedRoute role="admin">
                <AdminPanel />
              </ProtectedRoute>
          
            }
          />
          <Route path="*" element={<NotFound />} /> {/* Catch-all route */}
        </Routes>
      </div>
      <Footer />
    </>
  )
}

export default App
