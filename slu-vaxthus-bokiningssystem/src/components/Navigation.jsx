import { Link } from "react-router-dom"

function Navigation() {

  return (
    <nav>
      <Link to="/">HorticumBookingPageComponent</Link> |&nbsp;
      <Link to="/other">VegetumPageComponent</Link> |&nbsp;
    </nav>
  )

}

export default Navigation