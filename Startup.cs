using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Owin;
using WebLightNovel.Interfaces;
using WebLightNovel.Service.Stories;

[assembly: OwinStartupAttribute(typeof(WebLightNovel.Startup))]
namespace WebLightNovel
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
       
    }
}
