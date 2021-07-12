using System;
using Xunit;
using CommandAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Moq;
using AutoMapper;
using CommandAPI.Models;
using CommandAPI.Data;
using CommandAPI.Profiles;
using CommandAPI.Dtos;

namespace CommandAPI.Tests
{
    public class CommandsControllerTests : IDisposable
    {
        Mock<ICommandAPIRepo> mockRepo;
        CommandsProfile realProfile;
        MapperConfiguration configuration;
        IMapper mapper;
        public CommandsControllerTests()
        {
            mockRepo = new Mock<ICommandAPIRepo>();
            realProfile = new CommandsProfile();
            configuration = new MapperConfiguration(cfg => cfg.AddProfile(realProfile));
            mapper = new Mapper(configuration);
        }
        public void Dispose()
        {
            mockRepo = null;
            mapper = null;
            configuration = null;
            realProfile = null;
        }

        // Test 1.1 - check 200OK HTTP Response(Empty DB)
        [Fact]
        public void GetCommandItems_ReturnsZeroItem_WhenDBIsEmpty()
        {
            //arrange 
            var mockRepo = new Mock<ICommandAPIRepo>();
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(0));
            var realProfile = new CommandsProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(realProfile));
            IMapper mapper = new Mapper(configuration);
       
            var controller = new CommandsController(mockRepo.Object, mapper);
            var result = controller.GetAllCommands();
            Assert.IsType<OkObjectResult>(result.Result);
        
        }
        private List<Command> GetCommands(int num)
        {
            var commands = new List<Command>();
            if(num>0)
            {
                commands.Add(new Command{
                    Id = 0,
                    HowTo = "How to generate a migration",
                    CommandLine = "dotnet ef migrations add <Name of migration>",
                    Platform = ".Net Core EF"
                });
            }
            return commands;
        }
        //Test 1.2 Check Single Resource Returned
        [Fact] 
        public void GetAllCommands_ResturnsOneResult_WhenDBHasOneResource()
        {
            //arrange 
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));
            var controller = new CommandsController(mockRepo.Object, mapper);

            //act
            var result = controller.GetAllCommands();

            //assert


            var okResult = result.Result as OkObjectResult; 
            var commands = okResult.Value  as List<CommandReadDto>;
            Assert.Single(commands);
        }

        //Test 1.3 Check 200OK HTTP Response
        [Fact]
        public void GetAllCommands_Returns200OK_WhenDBHasOneResource()
        {
            //Arrange 
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));
            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.GetAllCommands();

            //assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        //Test 1.4 - Check the correct object type returned

        [Fact]
        public void GetAllCommands_ReturnedCorrectType_WhenDBHasOneResource()
        {
            //Arrange
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));
            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.GetAllCommands();

            //Assert
            Assert.IsType<ActionResult<IEnumerable<CommandReadDto>>>(result);

        }

        //Test 2.1 Check 404 Not Found HTTP Response

        [Fact]
        public void GetCommandByID_Returns404NotFound_WhenNonExistIDProvided()
        {
            //Arrange 
            mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(() => null);
            var controller = new CommandsController(mockRepo.Object, mapper);

            //act
            var result = controller.GetCommandById(1);

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        // Test 2.2 Check 200 OK HTTP Response 

        [Fact]
        public void GetCommandByID_Returns200OK_WhenValidIDProvided()
        {
            //Arrange 
            mockRepo.Setup(repo =>repo.GetCommandById(1)).Returns(new Command { Id = 1, HowTo = "mock", Platform = "Mock", CommandLine ="Mock"});
            var controller = new CommandsController(mockRepo.Object, mapper);
            //act
            var result = controller.GetCommandById(1);

            //Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        //Test 2.3 Check the Correct Object Type Returned 

        [Fact]
        public void GetCommandByID_ReturnsCorrectResourceType_WhenValidIDProvided()
        {
        //Arrange 
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(new Command { Id = 1, HowTo = "mock", Platform = "Mock", CommandLine ="Mock"});
            var controller = new CommandsController(mockRepo.Object, mapper);
        
        //Act

            var result = controller.GetCommandById(1);

        
        //Assert
            Assert.IsType<ActionResult<CommandReadDto>>(result);
        }


        //Test 3.1 Check if the correct object type is returned

        [Fact]
        public void CreateCommand_ReturnsCorrectResourceType_WhenValidObjectSubmitted()
        {
        //Arrange 
            mockRepo.Setup( repo => repo.GetCommandById(1)).Returns(new Command { Id = 1, HowTo = "mock", Platform = "Mock", CommandLine ="Mock"});
            var controller = new CommandsController(mockRepo.Object, mapper);


        
        //When
            var result = controller.CreateCommand(new CommandCreateDto {});

        
        //Then
            Assert.IsType<ActionResult<CommandReadDto>>(result);

        }

        // Test 3.2 Check 201 HTTP Response 
        [Fact]
        public void CreateCommand_Result201Created_WhenValidObjectSubmitted()
        {
            //Arrange 
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(new Command { Id = 1, HowTo = "mock", Platform = "Mock", CommandLine ="Mock"});
            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.CreateCommand(new CommandCreateDto {});

            //Assert

            Assert.IsType<CreatedAtRouteResult>(result.Result);
        }

        //Test 4.1 Check 204 HTTP Response
        [Fact]
        public void UpdateCommand_Returns204NoContent_WhenValidObjectSubmitted()
        {
            //Arrange 
             mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(new Command { Id = 1, HowTo = "mock", Platform = "Mock", CommandLine ="Mock"});
             var controller = new CommandsController(mockRepo.Object, mapper);

             //act 
             var result = controller.UpdateCommand(1, new CommandUpdateDto {});

             //Assert

             Assert.IsType<NoContentResult>(result);
            /*  Here we ensure that the GetCommandById method will return a valid resource when
                we attempt to “update.” We then check to see that we get the success 204 No Content
                Response.
            */
        }

        // test 4.2 Check 404 HTTP response

        [Fact]
        public void UpdateCommand_Returns404NotFound_WhenNonExistResourceIDSubmitted()
        {
            //Arrange
            mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(() => null);
            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.UpdateCommand(0, new CommandUpdateDto {});
            // assert
            Assert.IsType<NotFoundResult>(result);
        }
        /*
            We setup our mock repository to return back null, which should trigger the 404 Not
            Found behavior.
        */

        [Fact]
        //test 5.1 Check 404 http response --- partialcommandupdate unit test
        public void PartialCommandUpdate_Returns404NotFound_WhenNonExistentResourceIDSubmitted()
        {
            //arrange 
            mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(() => null);
            var controller = new CommandsController(mockRepo.Object, mapper);

            //act
            var result = controller.PartialCommandUpdate(0, new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<CommandUpdateDto>{});

            //assert
            Assert.IsType<NotFoundResult>(result);
        }

        //test 6.1 Check for 204 No Content HTTP response ---partialdeletecontroller unit test

        [Fact]
        public void DeleteCommands_Returns204NoContent_WhenValidSourceIDSubmitted()
        {
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(new Command {Id =1, HowTo ="mock", Platform ="Mock", CommandLine ="mock"});
            var controller = new CommandsController(mockRepo.Object, mapper);
            //act
            var result = controller.DeleteCommand(1);

            //assert
            Assert.IsType<NoContentResult>(result);
        }
        //Test 6.2 check for 404 Not Found HTTP Response

        [Fact]
        public void DeleteCommand_Returns_404NotFound_WhenNonExistResourceIDSubmitted()
        {
            //Arrange 
            mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(() => null);

            
            var controller = new CommandsController(mockRepo.Object, mapper);
            //act
            var result = controller.DeleteCommand(0);

            //Assert
            Assert.IsType<NotFoundResult>(result);
            

        }


    }
}