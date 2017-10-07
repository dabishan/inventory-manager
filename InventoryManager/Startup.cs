using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(InventoryManager.Startup))]
namespace InventoryManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
