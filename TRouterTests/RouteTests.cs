using System.Net.Http;
using TRouter;
using Xunit;

namespace TRouterTests
{
    public class RouteTests
    {
        [Fact]
        public void PropertiesOnRouteShouldBeValid()
        {
            Route r = new Route(HttpVerb.Get, "/url", null);
            Assert.True(r.ParameterString != null);
        }

        [Fact]
        public void RouteShouldNotMatchIfInvalidUrlIsPassed()
        {
            Route r = new Route(HttpVerb.Get, "/url", null);
            Assert.False(r.DoesMatch(""));
            Assert.False(r.DoesMatch("/test"));
        }

        [Fact]
        public void RouteShouldMatchForCorrectRoute()
        {
            Route r = new Route(HttpVerb.Get, "/url", null);
            Assert.True(r.DoesMatch("/url"));
        }
    }
}
