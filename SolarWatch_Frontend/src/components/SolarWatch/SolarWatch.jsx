import React, { useState } from 'react'
import './SolarWatch.css'
import sunrisePic from '../../assets/sunrise.svg';
import sunsetPic from '../../assets/sunset.svg';
import noonPic from '../../assets/noon.svg';
import timePic from '../../assets/time.svg';

const SolarWatch = () => {

    const [location, setLocation] = useState('');
    const [solarData, setSolarData] = useState({});
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');

    const handleSubmit = async (e) => {

        e.preventDefault();

        // Reset the state
        if (solarData) {
            setSolarData({});
        }
        setLoading(true);

        const formLocation = e.target.location.value;
        setLocation(formLocation);
        const date = e.target.date.value;
        //console.log(formLocation, date);

        const fetchString = `/api/solarwatch/solardata?date=${date}&location=${formLocation}`;
        const token = localStorage.getItem('token');
        const response = await fetch(fetchString, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            }
        });

        if (response.ok) {
            const result = await response.json();
            setSolarData(result);
            console.log("Response result: ", result);
        }
        else {
            const result = await response.text();
            setError(result);
            console.error('Failed to fetch data');
        }
        setLoading(false);
    }

    function formatTimestamp(isoString) {
        const utcDate = new Date(isoString);
        const localDateStr = utcDate.toLocaleString(undefined, {
            year: "numeric",
            month: "2-digit",
            day: "2-digit",
            hour: "2-digit",
            minute: "2-digit",
            second: "2-digit",
            hour12: false
        })

        return localDateStr.split(",")[1];
    }

    function formatDate(string) {
        return string.replace(/-/g, "/");
    }

    return (
        <div className='solar-watch'>

            <form onSubmit={handleSubmit} className='solarForm'>
                <input type='text' name='location' placeholder='Enter your location' required />
                <br />
                <input type='date' name='date' required value={new Date().toISOString().split("T")[0]}/>
                <br />
                <button className='buttonBlue'>Search</button>
            </form>

            {loading ?
                (<div className='loading'>Loading...</div>)
                :
                (
                    <div className='solarDataResult'>
                        {error && <h1>{error}</h1>}
                        {solarData.sunrise && (
                            <div className='solarData'>
                                <div className='solarDataLocation'>
                                    <h1>{location}</h1>
                                    <h3>Date: {formatDate(solarData.date)}</h3>
                                </div>
                                <div className='solarDataInfo'>
                                    <div>
                                        <img src={sunrisePic} height={32} alt='Sunrise' title='Sunrise. From: https://www.svgrepo.com/' />
                                        <p>{formatTimestamp(solarData.sunrise)}</p>
                                    </div>
                                    <div>
                                        <img src={noonPic} height={32} alt='Solar Noon' title='Noon. From: https://www.svgrepo.com/' />
                                        <p>{formatTimestamp(solarData.solarNoon)}</p>
                                    </div>
                                    <div>
                                        <img src={sunsetPic} height={32} alt='Sunset' title='Sunset. From: https://www.svgrepo.com/' />
                                        <p>{formatTimestamp(solarData.sunset)}</p>
                                    </div>
                                    <div>
                                        <img src={timePic} height={32} alt='Day Length' title='Day Length. From: https://www.svgrepo.com/' />
                                        <p>{solarData.dayLengthFormatted}</p>
                                    </div>                                    
                                </div>

                                    <i>
                                        These times are adjusted for your local time
                                    </i>

                            </div>
                        )}
                    </div>

                )
            }

        </div>
    )
}

export default SolarWatch