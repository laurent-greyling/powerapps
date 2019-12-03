using ServiceDeskTickets.DependencyInjection;
using ServiceDeskTickets.MessageHandlers;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceDeskTickets
{
    public static class StartUp
    {
        private static readonly AsyncLock AsyncLock = new AsyncLock();

        private static Container _container;

        private static async Task<Container> GetContainerAsync()
        {
            if (_container != null) return _container;
            using (await AsyncLock.LockAsync())
            {
                var container = DependencyInjectionInitializer.Initialize();

                var startupTasks = container.GetAllInstances<IRunAtStartup>();
                foreach (var task in startupTasks)
                {
                    await task.RunAsync();
                }

                _container = container;
            }

            return _container;
        }

        public static async Task<IMessageHandler> GetMessageHandlerAsync()
        {
            var container = await GetContainerAsync();
            return container.GetInstance<IMessageHandler>();
        }
    }
}
