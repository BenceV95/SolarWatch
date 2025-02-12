import React, { useState } from 'react'
import './SolarWatch.css'

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

    function formatTimestamp(isoString, timeOnly = false) {
        const date = new Date(isoString);

        if (timeOnly) {
            const timeStr = isoString.split('T')[1].split('Z')[0];
            const options = {
                hour: "2-digit",
                hour12: false,
                hourCycle: "h24",
                minute: "2-digit",
                second: "2-digit"
            };
            return timeStr.toLocaleString(options);

        } else {            

            const options = {
                year: "numeric",
                month: "long",
                day: "numeric",
                hour: "2-digit",
                hour12: false,
                hourCycle: "h24",
                minute: "2-digit",
                second: "2-digit",
                timeZoneName: "short"
            };

            return date.toLocaleString(options);
        }
    }

    function formatDate(string) {
        return string.replace(/-/g, "/");
    }

    return (
        <div className='solar-watch'>

            <form onSubmit={handleSubmit} className='solarForm'>
                <input type='text' name='location' placeholder='Enter your location' />
                <br />
                <input type='date' name='date' />
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
                                    <p><b>Sunrise: </b> {formatTimestamp(solarData.sunrise,true)}</p>
                                    <p><b>Solar Noon: </b>{formatTimestamp(solarData.solarNoon,true)}</p>
                                    <p><b>Sunset: </b>{formatTimestamp(solarData.sunset,true)}</p>
                                    <p><b>Day Length: </b>{solarData.dayLengthFormatted}</p>
                                </div>

                            </div>
                        )}
                    </div>

                )
            }

        </div>
    )
}

export default SolarWatch