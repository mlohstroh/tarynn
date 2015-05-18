using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRouter;
using Xunit;

namespace TRouterTests
{
    public class RouteTests
    {
        [Fact]
        public void PropertiesOnRouteShouldBeValid()
        {
            Route r = new Route("/url", (request, response) => null);
            Assert.True(r.ParameterString != null);
            Assert.True(r.DelegateFunction != null);
        }
    }
}
