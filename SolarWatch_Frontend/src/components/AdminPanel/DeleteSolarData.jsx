import React from 'react'

const DeleteSolarData = ({handleDelete}) => {
  return (
    <div>
        <h3>Delete Solar Data</h3>
        <form onSubmit={(e) => handleDelete(e)}>
            <input type='number' placeholder='SolarData ID ( int )' name='id'/>
            <br />
            <button className='buttonRed' type='submit'>Delete</button>
        </form>
    </div>
  )
}

export default DeleteSolarData