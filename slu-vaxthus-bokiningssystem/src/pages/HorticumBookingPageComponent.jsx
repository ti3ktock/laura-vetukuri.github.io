import React, { useState } from 'react';
import { Link } from "react-router-dom";
import MonthlyViewComponent from "../components/MonthlyViewComponent.jsx";
import YearlyViewComponent from "../components/YearlyViewComponent.jsx";

function HorticumBookingPageComponent() {
  const [currentView, setCurrentView] = useState('monthly');

  const chambers = [
    { id: 2, subChambers: [] },
    { id: 3, subChambers: [] },
    { id: 4, subChambers: [] },
    { id: 5, subChambers: [] },
    { id: 6, subChambers: [] },
    { id: 7, subChambers: [] },
    { id: 8, subChambers: [] },
    { id: 9, subChambers: [] },
    { id: 10, subChambers: [] },
    { id: 11, subChambers: [] },
    { id: 12, subChambers: [] },
    { id: 13, subChambers: [] },
    { id: 14, subChambers: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
    { id: 15, subChambers: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
    { id: 16, subChambers: [] },
    { id: 17, subChambers: [] },
  ];

  return (
    <>
      <h1>My Page Component page</h1>
      <div>
        <ul>
          <li onClick={() => setCurrentView('monthly')} style={{ cursor: 'pointer', fontWeight: currentView === 'monthly' ? 'bold' : 'normal' }}>Monthly View</li>
          <li onClick={() => setCurrentView('yearly')} style={{ cursor: 'pointer', fontWeight: currentView === 'yearly' ? 'bold' : 'normal' }}>Yearly View</li>
        </ul>

        {currentView === 'monthly' ? <MonthlyViewComponent chambers={chambers} /> : <YearlyViewComponent chambers={chambers} />}
      </div>
    </>
  );
}

export default HorticumBookingPageComponent;
