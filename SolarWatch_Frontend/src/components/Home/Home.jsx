import React, { useEffect, useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import './Home.css'

const Home = () => {

    const [testMessage, setTestMessage] = useState('')
    const [displayOrderedList, setDisplayOrderedList] = useState(false)

    const displayHowTo = () => {
        setDisplayOrderedList(!displayOrderedList);
    }
    return (
        <div className='home'>
            <h1>Welcome to SolarWatch</h1>
            <div className='description'>
                <p>With <b>SolarWatch</b> you can track a given location's sunrise and sunset.</p>
                <br />
                <button className='buttonYellow' onClick={displayHowTo}>How to use the site?</button>
                {displayOrderedList && (
                    <ol style={{ textAlign: 'left' }}>
                        <li>Start by signing in or if you do not have an account then register.</li>
                        <li>Once logged in, navigate to SolarWatch.</li>
                        <li>Enter the location and the date.</li>
                        <li>???</li>
                        <li>Profit!</li>
                    </ol>
                )}

            </div>
        </div>
    )
}

export default Home