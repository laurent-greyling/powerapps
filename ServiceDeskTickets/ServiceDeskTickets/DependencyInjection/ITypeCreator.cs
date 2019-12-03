using System;

namespace ServiceDeskTickets.DependencyInjection
{
    public interface ITypeCreator
    {
        object Create(Type typeToCreate);
    }
}
