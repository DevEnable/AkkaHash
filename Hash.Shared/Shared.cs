using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Event;
using Akka.Routing;

namespace Hash.Shared
{

    public class IdentityActor : ReceiveActor
    {
        public long Identity { get; private set; }

        public IdentityActor()
        {
            Receive<EntityMessage<long>>(m =>
            {
                if (Identity == 0)
                {
                    Console.WriteLine("Loaded entity");
                }
                else
                {
                    if (Identity != m.Identity)
                    {
                        throw new ArgumentException(string.Format("This actors identity is {0}, received identity of {1}",
                            Identity, m.Identity));
                    }

                    Console.WriteLine("Processed message");
                }
                
                Identity = m.Identity;
            });

        }

    }

    public class EntityMessage<TIdentity> : IConsistentHashable
    {
        public TIdentity Identity { get; set; }

        public EntityMessage(TIdentity identity)
        {
            Identity = identity;
        }

        public object ConsistentHashKey
        {
            get
            {
                return Identity;
            }

        }
    }
}
