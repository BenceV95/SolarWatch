import './AdminPanel.css'
import React, { useState } from 'react'
import DeleteLocationData from './DeleteLocationData';
import DeleteSolarData from './DeleteSolarData';

const AdminPanel = () => {

    const [action, setAction] = useState('');
    const [display, setDisplay] = useState(false);
    const [responseMessage, setResponseMessage] = useState("");

    const token = localStorage.getItem('token');

    const displayAction = (e) => {
        if (e.target.id === action) {
            setAction('');
            setDisplay(false);
            return;
        }
        else {
            setAction(e.target.id);
            setDisplay(true);
            return;
        }
    }

    const handleDelete = (e) => {

        e.preventDefault();

        const whatToDelete = action === 'delLocData' ? `/api/SolarWatch/deleteCity?id=${e.target.id.value}` : `/api/SolarWatch/deleteSolarData?id=${e.target.id.value}`;
        
        fetch(whatToDelete, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`

            }
        }).then(setResponseMessage("Should be deleted."))
        .catch(error => console.log('Error:', error));


    }

    return (
        <div className='adminPanel'>
            <h1>Admin Panel</h1>
            <h3>Work in Progress !</h3>
            <div className='adminPanelActions'>
                <button className='buttonRed' onClick={(e) => displayAction(e)} id='delLocData'>Delete Location Data</button>
                <button className='buttonRed' onClick={(e) => displayAction(e)} id='delSolarData'>Delete Solar Data</button>
            </div>
            {display && <div className='adminPanelActionComponent'>
                {action === "delLocData" && <DeleteLocationData handleDelete={handleDelete} />}
                {action === "delSolarData" && <DeleteSolarData handleDelete={handleDelete} />}
                {responseMessage && <h3>{responseMessage}</h3>}
            </div>}
        </div>
    )
}

export default AdminPanel