using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TicketData;

namespace TicketSystemService
{
    public class SeatHold : ISeatHold
    {
        private TimeSpan HoldTimeout { get; set; }

        public SeatHold(int countRequested, SeatClaim seatClaim, TimeSpan holdTimeout)
        {
            SeatLocations = seatClaim.Seats
                .Select(seat => new SeatLocation { Row = seat.SeatRow, SeatNum = seat.SeatNum })
                .ToList();

            CountHeld = SeatLocations.Count;
            CountRequested = countRequested;
            HoldId = seatClaim.HoldId;
            HoldTimeout = holdTimeout;
        }

        public int HoldId { get; set; }
        public int CountRequested { get; set; }
        public int CountHeld { get; set; }
        public IList<SeatLocation> SeatLocations { get; set; }

        private string note;
        public string Note
        {
            get
            {
                if (note == null)
                {
                    note = $"This hold is temporary and will expire in {HoldTimeout} (hh:mm:ss). Please reserve these seats to finalize.";
                    if (SeatLocations.Count <= 0)
                    {
                        note += "\nWarning: No more seats are available at this venue.";
                    }
                    if (CountRequested > SeatLocations.Count)
                    {
                        note += $"\nWarning: not all requested seats could be reserved. The number of requested tickets: {CountRequested}. Actual seats being held: {SeatLocations.Count()}";
                    }
                }
                return note;
            }
        }


    }
}
