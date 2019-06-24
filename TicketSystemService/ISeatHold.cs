using System.Collections.Generic;

namespace TicketSystemService
{
    public interface ISeatHold
    {
        int HoldId { get; }
        int CountRequested { get; }
        int CountHeld { get; }
        string Note { get; }
        IList<SeatLocation> SeatLocations { get; }
    }

}