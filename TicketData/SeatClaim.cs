using System;
using System.Collections.Generic;
using System.Text;

namespace TicketData
{
    public class SeatClaim
    {
        public int SeatClaimId { get; set; }
        public string Email { get; set; }
        public DateTime? TimeStampHeld { get; set; }
        public DateTime? TimeStampReserved { get; set; }
        public ICollection<Seat> Seats { get; set; }
        public int HoldId { get; set; }
    }
}
