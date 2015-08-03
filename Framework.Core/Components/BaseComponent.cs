using NLog;

namespace EvilDuck.Framework.Core.Components
{
    public abstract class BaseComponent
    {
        protected Logger Logger { get; private set; }

        protected BaseComponent(Logger logger)
        {
            Logger = logger;
        }
    }
}