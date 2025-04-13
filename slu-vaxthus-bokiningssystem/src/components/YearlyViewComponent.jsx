import React from 'react';
import '../css/YearlyViewComponent.css';

function YearlyViewComponent({ chambers }) {
  const weeksInYear = Array.from({ length: 52 }, (_, i) => i + 1);
  const bookings = Array.from({ length: 52 }, () =>
    chambers.map(() => ({
      booked: Math.random() < 0.5,
      subChambers: Array.from({ length: 10 }, () => Math.random() < 0.5)
    }))
  ); // Randomly mock booking status for demo

  const year = "2024"; 

  return (
    <div>
      <h2>Yearly View - {year}</h2>
      <div className="calendar-container">
        <div className="calendar-header">
          <div className="empty-cell"></div>
          {weeksInYear.map(week => (
            <div key={week} className="week-number">Week {week}</div>
          ))}
        </div>
        {chambers.map((chamber, chamberIndex) => (
          <React.Fragment key={chamber.id}>
            <div className="chamber-row">
              <div className="chamber-name">Chamber {chamber.id}</div>
              {chamber.subChambers.length === 0 && weeksInYear.map((week, weekIndex) => (
                <div
                  key={week}
                  className={`booking-status ${bookings[weekIndex][chamberIndex].booked ? 'booked' : 'available'}`}
                >
                  {bookings[weekIndex][chamberIndex].booked ? 'B' : 'A'}
                </div>
              ))}
            </div>
            {chamber.subChambers.length > 0 && chamber.subChambers.map((subChamber, subChamberIndex) => (
              <div key={`${chamber.id}-${subChamber}`} className="sub-chamber-row">
                <div className="sub-chamber-name">Table {subChamber}</div>
                {weeksInYear.map((week, weekIndex) => (
                  <div
                    key={`${week}-${subChamber}`}
                    className={`booking-status sub-booking-status ${bookings[weekIndex][chamberIndex].subChambers[subChamber - 1] ? 'booked' : 'available'}`}
                  >
                    {bookings[weekIndex][chamberIndex].subChambers[subChamber - 1] ? 'B' : 'A'}
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

export default YearlyViewComponent;
