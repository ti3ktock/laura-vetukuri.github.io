import { BrowserRouter, Routes, Route } from "react-router-dom";
import Navigation from "./Navigation.jsx";
import HorticumBookingPageComponent from "../pages/HorticumBookingPageComponent.jsx";
import VegetumPageComponent from "../pages/VegetumPageComponent.jsx";

function Router() {
  return (
    <BrowserRouter>
      <Navigation />
      <Routes>
        {/* Our route definitions (controller) */}
        <Route path="/" element={<HorticumBookingPageComponent />} />
        <Route path="/other" element={<VegetumPageComponent />} />
      </Routes>
    </BrowserRouter>
  );
}

export default Router;
