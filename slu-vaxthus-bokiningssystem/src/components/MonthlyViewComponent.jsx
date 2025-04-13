import React from 'react';
import '../css/MonthlyViewComponent.css';

function MonthlyViewComponent({ chambers }) {
  const daysInMonth = Array.from({ length: 30 }, (_, i) => i + 1);
  const bookings = Array.from({ length: 30 }, () =>
    chambers.map(() => ({
      booked: Math.random() < 0.5,
      subChambers: Array.from({ length: 10 }, () => Math.random() < 0.5)
    }))
  ); // Randomly mock booking status for demo

  const monthName = "July"; 
  
  return (
    <div>
      <h2>Monthly View - {monthName}</h2>
      <div className="calendar-container">
        <div className="calendar-header">
          <div className="empty-cell"></div>
          {daysInMonth.map(day => (
            <div key={day} className="day-number">{day}</div>
          ))}
        </div>
        {chambers.map((chamber, chamberIndex) => (
          <React.Fragment key={chamber.id}>
            <div className="chamber-row">
              <div className="chamber-name">Chamber {chamber.id}</div>
              {chamber.subChambers.length === 0 && daysInMonth.map((day, dayIndex) => (
                <div
                  key={day}
                  className={`booking-status ${bookings[dayIndex][chamberIndex].booked ? 'booked' : 'available'}`}
                >
                  {bookings[dayIndex][chamberIndex].booked ? 'B' : 'A'}
                </div>
              ))}
            </div>
            {chamber.subChambers.length > 0 && chamber.subChambers.map((subChamber, subChamberIndex) => (
              <div key={`${chamber.id}-${subChamber}`} className="sub-chamber-row">
                <div className="sub-chamber-name">Table {subChamber}</div>
                {daysInMonth.map((day, dayIndex) => (
                  <div
                    key={`${day}-${subChamber}`}
                    className={`booking-status sub-booking-status ${bookings[dayIndex][chamberIndex].subChambers[subChamber - 1] ? 'booked' : 'available'}`}
                  >
                    {bookings[dayIndex][chamberIndex].subChambers[subChamber - 1] ? 'B' : 'A'}
                  </div>
                ))}
              </div>
            ))}
          </React.Fragment>
        ))}
      </div>
    </div>
  );
}

export default MonthlyViewComponent;
