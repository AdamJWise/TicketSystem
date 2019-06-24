using System;
using CommandLine;
using TicketSystemCLI;
using TicketSystemService;

namespace TicketSystem
{
    /// <summary>
    /// CLI front end for TicketSystem
    /// Example usage for debugging:
    ///    Help: dotnet run -- -h
    ///    Count available seats: dotnet run -- count -available
    /// </summary>
    class Program
    {
        static TicketService ticketService;

        static void Main(string[] args)
        {
            ticketService = new TicketService();

            ticketService.PopulateInitialDB();

            String optionsSerialized = CommandLine.Parser.Default.ParseArguments<CountOptionsCLI, HoldOptionsCLI, ReserveOptionsCLI>(args)
              .MapResult(
                (CountOptionsCLI opts) => RetrieveCount(opts),
                (HoldOptionsCLI opts) => Hold(opts),
                (ReserveOptionsCLI opts) => Reserve(opts),
                errs => " ");

            Console.WriteLine(optionsSerialized);
        }

        private static string RetrieveCount(CountOptionsCLI opts)
        {
            String ret = "";
            if (opts.Reserved)
            {
                throw new NotImplementedException();
            } else if (opts.Held)
            {
                throw new NotImplementedException();
            } else if (opts.Available)
            {
                ret = ticketService.numSeatsAvailable().ToString();
            } else
            {
                // by default, show available seats
                ret = ticketService.numSeatsAvailable().ToString();
            }

            return ret;
        }

        private static string Hold(HoldOptionsCLI holdOptionsCLI)
        {
            var hold = ticketService.findAndHoldSeats(holdOptionsCLI.CountToHold, holdOptionsCLI.Email);
            String ret = $"HoldID = {hold.HoldId}\n" +
                         $"Holding {hold.CountHeld} tickets.\n" +
                         $"Note: {hold.Note}\n";

            ret += "Seats being held:";
            foreach (var seatLocation in hold.SeatLocations) {
                ret += $"\n" +
                       $"\n  Row:  {seatLocation.Row}" +
                       $"\n  Seat: {seatLocation.SeatNum}";
            }
            return ret;
        }

        private static string Reserve(ReserveOptionsCLI opts)
        {
            String ret;
            String reservationId = ticketService.reserveSeats(opts.HoldId, opts.Email);
            if (!String.IsNullOrEmpty(reservationId))
            {
                ret = "Reservation Successful. Reservation Id:" + reservationId;
            } else
            {
                ret = "Error in reservation. Hold time may have elapsed, please try again.";
            }
            return ret;
        }
    }
}
