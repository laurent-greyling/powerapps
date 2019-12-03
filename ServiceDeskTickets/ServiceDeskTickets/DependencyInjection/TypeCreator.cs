using SimpleInjector;
using System;

namespace ServiceDeskTickets.DependencyInjection
{
    public class TypeCreator : ITypeCreator
    {
        private readonly Container _container;

        public TypeCreator(Container container)
        {
            _container = container;
        }

        public object Create(Type typeToCreate)
        {
            return _container.GetInstance(typeToCreate);
        }
    }
}
