using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace TicketSystemCLI
{
    [Verb("count", HelpText = "Count seats. -a available")]
    class CountOptionsCLI
    {
        [Option('a', "available", Required = false, HelpText = "Count available seats in venue.")]
        public bool Available { get; set; }

        [Option('h', "held", Required = false, HelpText = "Count seats that are currently being held in the venue.")]
        public bool Held { get; set; }

        [Option('r', "reserved", Required = false, HelpText = "Count seats that are already reserved in the venue.")]
        public bool Reserved { get; set; }
    }
}
