using PackageCopycat.Controllers;
using PackageCopycat.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PackageCopycat.Factories
{
    public class QueueMessagesFactory : IFactory
    {
        private readonly Dictionary<Type, IHandler> _handlers;

        public QueueMessagesFactory(IEnumerable<IHandler> handlers)
        {
            _handlers = new Dictionary<Type, IHandler>();

            foreach(var handler in handlers)
            {
                _handlers[handler.Key] = handler;
            }
        }

        public IHandler GetHandler(Type key)
        {
            return _handlers[key];
        }

    }
}
