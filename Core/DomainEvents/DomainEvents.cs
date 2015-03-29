using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.Utils.DomainEvents
{
    public static class DomainEvents
    {
        //private static Container Container { get; set; }
        //private static NLog.Logger _logger;

        //public static void SetContainer(Container container)
        //{
        //    if (container == null) throw new ArgumentNullException("container");

        //    Container = container;
        //    _logger = container.GetInstance<ILogger>();

        //}
        //public static void Raise<T>(T eventArg) where T : IDomainEvent
        //{
        //    if (Container != null)
        //    {
        //        var handlers = Container.GetAllInstances<IHandleDomainEvent<T>>();
        //        var handlerText = handlers.Any()
        //            ? handlers.Select(h => h.GetType().Name).Aggregate((a, b) => a + "; " + b)
        //            : "None";
        //        ;
        //        _logger.Debug("Received event " + eventArg.GetType() + ". Following handlers will be called: " +
        //                      handlerText);

        //        foreach (var handler in handlers)
        //        {
        //            handler.Handle(eventArg);
        //        }
        //    }
        //}
    }
}
