using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace TicketSystemCLI
{
    [Verb("hold", HelpText = "Request to hold seats")]
    class HoldOptionsCLI
    {       
        [Option('c', "count", Required = true, HelpText = "Number of seats requested.")]
        public int CountToHold { get; set; }

        [Option('e', "email", Required = true, HelpText = "E-mail address of the customer.")]
        public string Email { get; set; }
    }
}
