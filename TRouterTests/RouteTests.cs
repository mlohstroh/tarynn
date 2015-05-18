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

        [Fact]
        public void RouteShouldNotMatchIfInvalidUrlIsPassed()
        {
            Route r = new Route("/url", (request, response) => null);
            Assert.False(r.DoesMatch(""));
            Assert.False(r.DoesMatch("/test"));
        }

        [Fact]
        public void RouteShouldMatchForCorrectRoute()
        {
            Route r = new Route("/url", (request, response) => null);
            Assert.True(r.DoesMatch("/url"));
        }
    }
}
