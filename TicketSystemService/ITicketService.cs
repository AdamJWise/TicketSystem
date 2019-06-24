using System;
using System.Collections.Generic;
using System.Text;

namespace TicketSystemService
{
    /// <summary>
    /// Main ITicketService interface for holding and reserving seats. 
    /// Note: This interface was specified per requirements. 
    /// (Future considerations include standard PascalCase for method names and using a string for the seatHoldId)
    /// </summary>
    interface ITicketService
    {
        /// <summary>
        /// The number of seats in the venue that are neither held nor reserved
        /// </summary>
        /// <returns>count of tickets available in the venue</returns>
        int numSeatsAvailable();

        /// <summary>
        /// Find and hold the best available seats for a customer
        /// </summary>
        /// <param name="numSeats">count of seats to find and hold</param>
        /// <param name="customerEmail">unique identifier for the customer</param>
        /// <returns>a SeatHold object identifying the specific seats and related information</returns>
        SeatHold findAndHoldSeats(int numSeats, String customerEmail);

        /// <summary>
        /// Commit seats held for a specific customer
        /// </summary>
        /// <param name="seatHoldId">the seat hold identifier</param>
        /// <param name="customerEmail">unique identifier for the customer, the email address of the customer to which the seat hold is assigned</param>
        /// <returns>a reservation confirmation code</returns>
        String reserveSeats(int seatHoldId, String customerEmail);
    }
}

