import React from 'react'

const DeleteLocationData = ({handleDelete}) => {

  return (
    <div>
        <h1>Delete Location Data</h1>
        <form onSubmit={(e) => handleDelete(e)}>
            <input type='number' placeholder='Location Name ( int )' name='id'/>
            <br />
            <button className='buttonRed' type='submit'>Delete</button>
        </form>
    </div>
  )
}

export default DeleteLocationData