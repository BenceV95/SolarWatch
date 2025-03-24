import React from 'react'
import { Link } from 'react-router-dom'

const Tos = () => {
  return (
    <div>
        <h1>Terms & Conditions</h1>
        <p>
            <h3>Hello!</h3><br />
            <b>Please make sure to read the terms and conditions before using my website.</b><br />
            <ul>
                <li>All the data you generate, provide will be only used to make the site function. The data will be eventually deleted.</li>
                <li>For this reason you can register with <i>fake email.</i></li>
                <li>The project is <b>UNDER DEVELOPMENT</b> and is <b>NOT</b> the final version. Unoptimized for mobile and bugs are higly likely.</li>
                <li>This project is used to demonstrate my skills as a Junior Full Stack Developer.</li>
                <li><Link to="https://github.com/BenceV95/SolarWatch.git">GitHub Link</Link></li>
                <li><Link to="https://benceveres.com">Further Questions? Contact me here</Link></li>
                <li><Link to="https://www.svgrepo.com/">SVGs are downloaded from here.</Link></li>
                <li>Thank you for understanding !</li>
            </ul>
        </p>
    </div>
  )
}

export default Tos