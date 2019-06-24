using System;
using TicketData;
using System.Linq;
using System.Security.Cryptography;

namespace TicketSystemService
{
    public class TicketService : ITicketService
    {
        /// <summary>
        /// Default 5 minute hold timeout. NOTE: could be loaded from setting or db table
        /// </summary>
        private readonly int kSecondsHoldTimeout = 300;

        public TimeSpan HoldTimeout
        { 
            get
            {
                TimeSpan ret = new TimeSpan(0, 0, kSecondsHoldTimeout);
                return ret;
            }
        }

        public void PopulateInitialDB()
        {
            using (var dbContext = new TicketSystemDbContext())
            {
                dbContext.DatabaseMigrate();
            }
        }

        private int GenerateUniqueHoldId()
        {
            int ret;
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var data = new byte[sizeof(int)];
                rngCryptoServiceProvider.GetBytes(data);
                int rnd = BitConverter.ToInt32(data, 0);
                // positive numbers are a little easier to serialize and otherwise handle
                ret = Math.Abs(rnd);
            }
            return ret;
        }

        /// <summary>
        /// Find and hold the best available seats for a customer
        /// </summary>
        /// <param name="numSeats">count of seats to find and hold</param>
        /// <param name="customerEmail">unique identifier for the customer</param>
        /// <returns>a SeatHold object identifying the specific seats and related information</returns>
        public SeatHold findAndHoldSeats(int numSeats, string customerEmail)
        {
            SeatHold ret = null;

            DateTime dateTimeExpiration = DateTime.UtcNow.AddSeconds(-kSecondsHoldTimeout);

            using (var dbContext = new TicketSystemDbContext())
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                var seats =
                (from seat in dbContext.Seats
                 where
                   seat.SeatClaimId == null ||
                   (
                     seat.SeatClaim.TimeStampReserved == null
                       &&
                     seat.SeatClaim.TimeStampHeld != null
                       &&
                     seat.SeatClaim.TimeStampHeld < dateTimeExpiration
                   )
                 orderby seat.Rank descending
                 select seat)
                 .Take(numSeats)
                 .ToList();

                SeatClaim seatClaim = null;
                if (seats.Any())
                {
                    // cleanup seats so the existing invalid claim is removed
                    seats.ForEach(seat => seat.SeatClaimId = null);

                    // create new claim, in a 'hold' state as indicated by TimeStampHeld
                    seatClaim = new SeatClaim
                    {
                        Email = customerEmail,
                        TimeStampHeld = DateTime.UtcNow,
                        TimeStampReserved = null,
                        Seats = seats,
                        HoldId = GenerateUniqueHoldId()
                    };
                }

                dbContext.SeatClaims.Add(seatClaim);

                seats.ForEach(seat =>
                {
                    seat.SeatClaim = seatClaim;
                });

                dbContext.SaveChanges();
                transaction.Commit();

                ret = new SeatHold(numSeats, seatClaim, HoldTimeout);
            }
            return ret;
        }


        /// <summary>
        /// The number of seats in the venue that are neither held nor reserved
        /// </summary>
        /// <returns>count of tickets available in the venue</returns>
        public int numSeatsAvailable()
        {
            int ret;

            DateTime dateTimeExpiration = DateTime.UtcNow.AddSeconds(-kSecondsHoldTimeout);

            using (var dbContext = new TicketSystemDbContext())
            {
                // unclaimed seats are seats without claims, or with an expired hold
                ret = (from o in dbContext.Seats
                       where 
                         o.SeatClaimId == null ||
                         (
                           o.SeatClaim.TimeStampReserved == null
                             &&
                           o.SeatClaim.TimeStampHeld != null 
                             && 
                           o.SeatClaim.TimeStampHeld < dateTimeExpiration
                         )
                       select o).Count();
            }

            return ret;
        }

        /// <summary>
        /// Commit seats held for a specific customer
        /// </summary>
        /// <param name="holdId">the seat hold identifier</param>
        /// <param name="customerEmail">unique identifier for the customer, the email address of the customer to which the seat hold is assigned</param>
        /// <returns>a reservation confirmation code</returns>
        public string reserveSeats(int holdId, string customerEmail)
        {
            String ret;
            using (var dbContext = new TicketSystemDbContext())
            {
                var seatClaim = dbContext.SeatClaims
                    .FirstOrDefault(s => s.HoldId == holdId &&
                                         s.TimeStampReserved == null &&
                                         0 == String.Compare(s.Email, customerEmail, true));
                seatClaim.TimeStampReserved = DateTime.UtcNow;
                dbContext.SaveChanges();

                // reservation id is a concatenation of the hold id and the time it was reserved
                ret = seatClaim.HoldId + "-" + seatClaim.TimeStampReserved.Value.Ticks;
            }
            return ret;
        }

        public void ClearAllClaims()
        {
            using (var dbContext = new TicketSystemDbContext())
            {
                dbContext.Seats.ToList().ForEach(seat => seat.SeatClaimId = null);
                dbContext.SeatClaims.RemoveRange(dbContext.SeatClaims);
                dbContext.SaveChanges();
            }
        }
    }
}
