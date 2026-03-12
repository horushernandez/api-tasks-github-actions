using TaskApi.Controllers;
using TaskApi.Models;
using TaskApi.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TaskApi.Tests.Controllers;

public class TaskControllerTest
{
    
    private readonly TasksController _ctrl;
    private readonly Mock<ITaskRepository> _mockRepo; 

    public TaskControllerTest()
    {
        _mockRepo = new Mock<ITaskRepository>();
        _ctrl = new TasksController(_mockRepo.Object);
    }

    //GetAll
    [Fact]
    public void GetAll_HayTareas_RetornarOkConListaDeTareas()
    {
        _mockRepo.Setup(repo => repo.GetAll()).Returns(
            new List<TaskItem>
            {
                new TaskItem { Id = 1, Title = "Tarea 1"},
                new TaskItem { Id = 2, Title = "Tarea 2"}
            }
        );
        
        //Assert
        _ctrl.GetAll()
            .Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<IEnumerable<TaskItem>>()
                .Which.Should().HaveCount(2);
    }

    //GetById
    [Fact]
    public void GetById_TareaExiste_RetornarOkConTarea()
    {
        _mockRepo.Setup(r => r.GetById(1)).Returns(
            new TaskItem { Id = 1, Title = "Tarea 1"}
        );
        
        //Assert
        _ctrl.GetById(1)
            .Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<TaskItem>()
                .Which.Title.Should().Be("Tarea 1");
    }

    //Cuando no existe el Id
    [Fact]
    public void GetById_IdNoExiste_RetornarNotFound()
    {
        _mockRepo.Setup(r => r.GetById(1)).Returns((TaskItem?)null);
        
        //Assert
        _ctrl.GetById(1)
            .Should().BeOfType<NotFoundResult>();
    }

}