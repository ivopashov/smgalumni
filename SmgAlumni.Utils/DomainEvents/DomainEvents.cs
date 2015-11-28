using SimpleInjector;
using SmgAlumni.Utils.DomainEvents.Models;
using System;

namespace SmgAlumni.Utils.DomainEvents
{
    public static class DomainEvents
    {
        private static Container Container { get; set; }
        private static NLog.Logger _logger;

        public static void SetContainer(Container container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            Container = container;
        }

        public static void Raise<T>(T eventArg) where T : IDomainEvent
        {
            if (Container != null)
            {
                var handlers = Container.GetAllInstances<IHandleDomainEvent<T>>();
                foreach (var handler in handlers)
                {
                    handler.Handle(eventArg);
                }
            }
        }
    }
}
