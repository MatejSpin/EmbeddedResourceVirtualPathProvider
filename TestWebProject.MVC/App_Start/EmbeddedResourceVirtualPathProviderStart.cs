using System.Linq;
using System.Reflection;
using System.Web.Optimization;

//[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(NugetTestWebProject.EmbeddedResourceVirtualPathProviderStart), "Start")]

namespace TestWebProject.MVC
{
    public static class EmbeddedResourceVirtualPathProviderStart
    {
        public static void Start()
        {

            var virtualPathprovider = new EmbeddedResourceVirtualPathProvider.Vpp(typeof(TestWebProject.MVC.MvcApplication).Assembly, typeof(TestResourceLibrary.Marker).Assembly);

            System.Web.Hosting.HostingEnvironment.RegisterVirtualPathProvider(virtualPathprovider);

            BundleTable.VirtualPathProvider = virtualPathprovider;
        }
    }
}