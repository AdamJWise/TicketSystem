using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketSystemService;

namespace TicketTests
{
    [TestClass]
    public class TicketServiceTests
    {
        [TestMethod]
        public void CountAvailableSeatsTest()
        {
            TicketService ticketService = new TicketService();
            ticketService.ClearAllClaims();
            int seatsAvailable = ticketService.numSeatsAvailable();
            Assert.IsTrue(seatsAvailable > 0);
            Assert.AreEqual(297, seatsAvailable);
        }

        [TestMethod]
        public void HoldSingleSeatTest()
        {
            TicketService ticketService = new TicketService();
            ticketService.ClearAllClaims();

            int seatsAvailableBeforeHold = ticketService.numSeatsAvailable();
            var hold = ticketService.findAndHoldSeats(1, "test@foo.com");
            int seatsAvailableAfterHold = ticketService.numSeatsAvailable();

            Assert.AreEqual(hold.CountHeld, 1);
            Assert.AreEqual(seatsAvailableBeforeHold, seatsAvailableAfterHold + 1);
        }

        [TestMethod]
        public void HoldMultipleSeatsTest()
        {
            TicketService ticketService = new TicketService();
            ticketService.ClearAllClaims();

            int seatsToHold = 5;
            int seatsAvailableBeforeHold = ticketService.numSeatsAvailable();
            var hold = ticketService.findAndHoldSeats(seatsToHold, "test5@foo.com");
            int seatsAvailableAfterHold = ticketService.numSeatsAvailable();

            Assert.AreEqual(hold.CountHeld, seatsToHold);
            Assert.AreEqual(seatsAvailableBeforeHold, seatsAvailableAfterHold + seatsToHold);
        }

        [TestMethod]
        public void ReserveSingleSeatTest()
        {
            TicketService ticketService = new TicketService();
            ticketService.ClearAllClaims();
            var email = "testReserve1@foo.com";
            int seatsToReserve = 1;
            var hold = ticketService.findAndHoldSeats(seatsToReserve, email);

            Assert.AreEqual(hold.CountHeld, seatsToReserve);

            var reservationCode = ticketService.reserveSeats(hold.HoldId, email);
            Assert.IsNotNull(reservationCode);
            Assert.IsTrue(reservationCode.Length >= hold.HoldId.ToString().Length);
        }

        [TestMethod]
        public void ReserveMutipleSeatsTest()
        {
            TicketService ticketService = new TicketService();
            ticketService.ClearAllClaims();
            var email = "testReserve11@foo.com";
            int seatsToReserve = 11;
            int seatsAvailableBeforeReserve = ticketService.numSeatsAvailable();
            var hold = ticketService.findAndHoldSeats(seatsToReserve, email);


            Assert.AreEqual(hold.CountHeld, seatsToReserve);

            var reservationCode = ticketService.reserveSeats(hold.HoldId, email);
            int seatsAvailableAfterReserve = ticketService.numSeatsAvailable();
            Assert.AreEqual(seatsAvailableBeforeReserve, seatsAvailableAfterReserve + seatsToReserve);
            Assert.IsNotNull(reservationCode);
            Assert.IsTrue(reservationCode.Length >= hold.HoldId.ToString().Length);
        }
    }
}
