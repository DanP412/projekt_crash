using CrashApp.Data;
using System.Threading.Tasks;

namespace CrashApp.Services
{
    public class TestService
    {
        public async Task ClearAllTheDataAsync()
        {
            await StaticContext.Context.Database.EnsureDeletedAsync();
            await StaticContext.Context.Database.EnsureCreatedAsync();
        }
    }
}