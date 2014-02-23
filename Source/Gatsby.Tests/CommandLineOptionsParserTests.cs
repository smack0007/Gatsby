using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace Gatsby.Tests
{
    [TestFixture]
    public class CommandLineOptionsParserTests
    {
        CommandLineOptionsParser parser;

        [SetUp]
        public void SetUp()
        {
            this.parser = new CommandLineOptionsParser();
        }

        [Test]
        public void Empty_Args_Is_Error()
        {
            var options = this.parser.Invoking(x => x.Parse(new string[] { })).ShouldThrow<GatsbyException>();
        }

        [Test]
        public void Unknown_Action_Is_Error()
        {
            var options = this.parser.Invoking(x => x.Parse(new string[] { "foo" })).ShouldThrow<GatsbyException>();
        }

        [Test]
        public void Generate_Written_In_Lower_Case_Is_Parsed_Correctly()
        {
            var options = this.parser.Parse(new string[] { "generate" });

            options.Action.Should().Be(GatsbyAction.Generate);
        }

        [Test]
        public void Generate_Written_In_Upper_Case_Is_Parsed_Correctly()
        {
            var options = this.parser.Parse(new string[] { "GENERATE" });

            options.Action.Should().Be(GatsbyAction.Generate);
        }

        [Test]
        public void Generate_Written_In_Mixed_Case_Is_Parsed_Correctly()
        {
            var options = this.parser.Parse(new string[] { "GeNeRaTe" });

            options.Action.Should().Be(GatsbyAction.Generate);
        }

        [Test]
        public void Build_Written_In_Lower_Case_Is_Parsed_Correctly()
        {
            var options = this.parser.Parse(new string[] { "build" });

            options.Action.Should().Be(GatsbyAction.Build);
        }

        [Test]
        public void Build_Written_In_Upper_Case_Is_Parsed_Correctly()
        {
            var options = this.parser.Parse(new string[] { "BUILD" });

            options.Action.Should().Be(GatsbyAction.Build);
        }

        [Test]
        public void Build_Written_In_Mixed_Case_Is_Parsed_Correctly()
        {
            var options = this.parser.Parse(new string[] { "BuIlD" });

            options.Action.Should().Be(GatsbyAction.Build);
        }

        [Test]
        public void Serve_Written_In_Lower_Case_Is_Parsed_Correctly()
        {
            var options = this.parser.Parse(new string[] { "serve" });

            options.Action.Should().Be(GatsbyAction.Serve);
        }

        [Test]
        public void Serve_Written_In_Upper_Case_Is_Parsed_Correctly()
        {
            var options = this.parser.Parse(new string[] { "SERVE" });

            options.Action.Should().Be(GatsbyAction.Serve);
        }

        [Test]
        public void Serve_Written_In_Mixed_Case_Is_Parsed_Correctly()
        {
            var options = this.parser.Parse(new string[] { "SeRvE" });

            options.Action.Should().Be(GatsbyAction.Serve);
        }

        [Test]
        public void When_Config_Flag_Not_Present_Defaut_Is_Set()
        {
            var options = this.parser.Parse(new string[] { "build" });

            options.ConfigPath.Should().Be(@"_Config.xml");
        }

        [Test]
        public void Config_Flag_Without_Value_Is_Error()
        {
            this.parser.Invoking(x => x.Parse(new string[] { "build", "--config" })).ShouldThrow<GatsbyException>();
        }

        [Test]
        public void Config_Flag_With_Value_Is_Set()
        {
            var options = this.parser.Parse(new string[] { "build", "--config", @"c:\temp\_Config.xml" });

            options.ConfigPath.Should().Be(@"c:\temp\_Config.xml");
        }
    }
}
