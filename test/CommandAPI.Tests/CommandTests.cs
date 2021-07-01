using System;
using Xunit;
using CommandAPI.Models;

namespace CommandAPI.Tests
{
    public class CommandTests : IDisposable
    {
        Command testCommand;
        public CommandTests()
        {
            testCommand = new Command
            {
                HowTo = "Do something",
                Platform = "Some Platform",
                CommandLine = "Some CommandLine"
            };
        }

        public void Dispose()
        {
            testCommand = null;
        }
        [Fact]
        public void CanChangeHowTo()
        {
       
        
        //When

        testCommand.HowTo = "Execute Unit Test";
        
        //Then
        Assert.Equal("Execute Unit Test", testCommand.HowTo);
        // 
        
        }
        [Fact]
        public void CanChangePlatform()
        {
       
        
        //When

        testCommand.Platform = "xUnit";
        
        //Then
        Assert.Equal("xUnit", testCommand.Platform);
        // 
        
        }
        [Fact]
        public void CanChangeCommandLine()
        {
       
        
        //When

        testCommand.CommandLine = "dotnet test";
        
        //Then
        Assert.Equal("dotnet test", testCommand.CommandLine);
           
        } 
    }
}