using SimpleInjector;
using SimpleInjector.Advanced;
using System;
using System.Reflection;

namespace ServiceDeskTickets.DependencyInjection
{
    public class LifestyleSelectionBehavior : ILifestyleSelectionBehavior
    {
        public Lifestyle SelectLifestyle(Type implementationType)
        {
            var attribute = implementationType.GetCustomAttribute<BindingAttribute>();

            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (attribute?.BindingType)
            {
                case BindingType.Singleton:
                    return Lifestyle.Singleton;
                default:
                    return Lifestyle.Transient;
            }
        }
    }
}
