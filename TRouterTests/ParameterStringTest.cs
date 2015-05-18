using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using TRouter;
using Xunit;

namespace TRouterTests
{
    public class ParameterStringTest
    {
        [Fact]
        public void InvalidUrlShouldThrowException()
        {
            Assert.Throws(typeof (Exception), () =>
            {
                ParameterString s = new ParameterString("test");
            });
        }

        [Fact]
        public void BasicUrlShouldHaveNoParameters()
        {
            ParameterString s = new ParameterString("/url");

            Assert.True(s.ParameterKeys.Count == 0);
        }

        [Fact]
        public void UrlWithOneParameterShouldHaveAParameterKey()
        {
            ParameterString s = new ParameterString("/url/:test");

            Assert.True(s.ParameterKeys.Count == 1);
            Assert.True(s.ParameterKeys.FirstOrDefault() == "test");
        }
    }
}
