using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using Akka.Actor;
using Akka.Configuration.Hocon;
using Akka.Event;
using Akka.Routing;
using Hash.Shared;

namespace AkkaHash
{
    public class Program
    {
        private static ActorSystem _system;
        private static IActorRef _coordinator;

        static void Main(string[] args)
        {
            var section = (AkkaConfigurationSection)ConfigurationManager.GetSection("akka");
            _system = ActorSystem.Create("test", section.AkkaConfig);

            _coordinator = _system.ActorOf(Props.Create(() => new IdentityActor())
                .WithRouter(FromConfig.Instance), "fred");

            //_coordinator =
            //    _system.ActorOf(Props.Create(() => new IdentityActor()).WithRouter(new ConsistentHashingPool(5000)),
            //        "fred2");

            int routees = _coordinator.Ask<Routees>(new GetRoutees()).Result.Members.Count();
            Console.WriteLine(routees);

            // Lazy wait for the co-ordinator to deploy.
            Thread.Sleep(5000);

            for (int i = 1; i <= 5000; i++)
            {
                for (int x = 1; x <= 4; x++)
                {
                    _coordinator.Tell(new EntityMessage<long>(i));
                }
            }

            Thread.Sleep(500);

            Console.ReadLine();
        }
    }



}
