using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace TicketSystemCLI
{
    [Verb("reset", HelpText = "Reset the database - useful for testing")]
    class ResetOptionsCLI
    {
        [Option('a', "all", Required = false, Default = true, HelpText = "Reset all holds and reservations.")]
        public bool all { get; set; }
    }
}
