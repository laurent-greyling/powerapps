using SimpleInjector;
using System.Linq;
using System.Reflection;

namespace ServiceDeskTickets.DependencyInjection
{
    public static class DependencyInjectionInitializer
    {
        public static Container Initialize()
        {
            var container = new Container();

            container.Options.LifestyleSelectionBehavior = new LifestyleSelectionBehavior();

            var typeResolver = new TypeCreator(container);
            container.RegisterSingleton<ITypeCreator>(typeResolver);

            var assembly = Assembly.GetExecutingAssembly();

            var types = assembly.GetTypes();

            var registrations = types
                .Where(t => t.GetCustomAttribute<BindingAttribute>() != null)
                .Select(t => new { Type = t, Attribute = t.GetCustomAttribute<BindingAttribute>() })
                .Select(t => new { ServiceType = t.Attribute.TypeToBind ?? t.Type, ImplementationType = t.Type, t.Attribute.BindingType });

            var groupedRegistrations = registrations.GroupBy(r => r.ServiceType);

            foreach (var registration in groupedRegistrations)
            {
                var typeRegistrations = registration.ToArray();
                if (typeRegistrations.Length == 1)
                {
                    var typeRegistration = typeRegistrations[0];

                    container.Register(typeRegistration.ServiceType, typeRegistration.ImplementationType);
                }
                else
                {
                    // multiple registrations for the same type are registered as a collection (implicitly)
                    container.RegisterCollection(registration.Key, registration.Select(r => r.ImplementationType));
                }
            }

            container.Verify();

            return container;
        }
    }
}
