using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatsby.Tests
{
    [TestFixture]
    public class ProcessorPathFilterTests
    {
        ProcessorPathFilter filter;

        [SetUp]
        public void SetUp()
        {
            filter = new ProcessorPathFilter();
        }

        [Test]
        public void Path_Which_Does_Not_Begin_With_Underscore_Should_Be_Processed()
        {
            filter.ShouldProcess("foo.xml")
                .Should()
                .BeTrue();
        }

        [Test]
        public void Path_Which_Begins_With_Underscore_Should_Not_Be_Processed()
        {
            filter.ShouldProcess("_foo.xml")
                .Should()
                .BeFalse();
        }
    }
}
