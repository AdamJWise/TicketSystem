using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace TicketSystemCLI
{
    [Verb("reserve", HelpText = "Request to reserve seats")]
    class ReserveOptionsCLI
    {
        [Option('i', "id", Required = true, HelpText = "Hold Id")]
        public int HoldId { get; set; }

        [Option('e', "email", Required = true, HelpText = "E-mail address of the customer.")]
        public string Email { get; set; }
    }
}
