using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration.Hocon;

namespace AkkaHash.WorkerRole
{
    class Program
    {
        private static ActorSystem _system;

        static void Main(string[] args)
        {
            var section = (AkkaConfigurationSection) ConfigurationManager.GetSection("akka");
            _system = ActorSystem.Create("test", section.AkkaConfig);

            Console.ReadLine();
        }
    }
}
