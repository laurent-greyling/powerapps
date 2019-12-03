using System;

namespace ServiceDeskTickets.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class BindingAttribute : Attribute
    {
        public BindingAttribute(BindingType bindingType) : this(bindingType, null)
        {
        }

        public BindingAttribute(BindingType bindingType, Type typeToBind)
        {
            BindingType = bindingType;
            TypeToBind = typeToBind;
        }
        public BindingType BindingType { get; }
        public Type TypeToBind { get; }
    }
}
