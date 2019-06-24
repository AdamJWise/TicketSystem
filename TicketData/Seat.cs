using System;
using System.Collections.Generic;
using System.Text;

namespace TicketData
{
    public class Seat
    {
        public int SeatId { get; set; }
        public string SeatRow { get; set; }
        public string SeatNum { get; set; }
        public float Rank { get; set; }
        public int? SeatClaimId { get; set; }
        public SeatClaim SeatClaim { get; set; }
    }
}
