using System.Linq;
using System.Reflection;
using Autofac.Core;
using EvilDuck.Framework.Core.Utils;
using NLog;

namespace EvilDuck.Framework.Core.Logging
{
    public class LoggingModule : Autofac.Module
    {
        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            registration.Preparing += OnComponentPreparing;
            registration.Activated += OnComponentActivated;
        }

        private static void OnComponentActivated(object sender, ActivatedEventArgs<object> e)
        {
            sender
                .GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p =>
                    p.PropertyType == typeof(Logger) &&
                    p.CanWrite &&
                    p.GetIndexParameters().Length == 0)
                .Do(p => p.SetValue(sender, LogManager.GetLogger(sender.GetType().FullName)));
        }

        private static void OnComponentPreparing(object sender, PreparingEventArgs e)
        {
            var t = e.Component.Activator.LimitType;
            e.Parameters = e.Parameters.Union(new[]
            {
                new ResolvedParameter(
                    (p, i) => p.ParameterType == typeof (Logger),
                    (p, i) => LogManager.GetLogger(t.FullName))
            });
        }
    }
}