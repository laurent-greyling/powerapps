using System.Threading.Tasks;

namespace ServiceDeskTickets
{
    public interface IRunAtStartup
    {
        Task RunAsync();
    }
}
