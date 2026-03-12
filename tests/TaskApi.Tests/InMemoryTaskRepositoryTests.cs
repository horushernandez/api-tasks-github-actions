using TaskApi.Repositories;
using TaskApi.Models;
using FluentAssertions;

namespace TaskApi.Tests.Repositories;

public class InMemoryTaskRepositoryTests {
    private readonly InMemoryTaskRepository _repo;

    public InMemoryTaskRepositoryTests(){
        _repo = new();
    }
    
    [Fact]
    public void Add_TareaValida_AsignaIdYRetornaTarea(){
        //Arrange        
        var tarea = new TaskItem {
            Title = "Comprar Guitarra",
            Description= "Comprar Guitarra para ser Feliz"
        };
        
        //Act
        var resultado = _repo.Add(tarea);
        
        //Arssert
        resultado.Id.Should().BeGreaterThan(0);
        resultado.Title.Should().Be("Comprar Guitarra");
    }

    [Fact]
    public void Add_DosTareas_IdsUnicos(){
        //Arrange        
        var tarea1 = new TaskItem {
            Title = "Comprar Juego",
            Description= "Comprar juego para ser Feliz"
        };
        var tarea2 = new TaskItem {
            Title = "Comprar control",
            Description= "Comprar control para ser Feliz"
        };

        //Act
        var resultado1 = _repo.Add(tarea1);
        var resultado2 = _repo.Add(tarea2);
        
        //Assert
        // resultado1.Id.Should().NotBe(resultado2.Id);
        resultado2.Id.Should().Be(resultado1.Id + 1);
    }

    //Get All
    [Fact]
    public void GetAll_RepositorioVacio_RetornaColeccionVacia(){
     
        //Act
        var resultado = _repo.GetAll();
        
        //Assert
        resultado.Should().BeEmpty();
    }

    [Fact]
    public void GetAll_ConDosTareas_RetornaDosTareas()
    {
        //Arrange        
        var tarea1 = new TaskItem {Title = "Comprar Juego",
            Description= "Comprar juego para ser Feliz"};
        var tarea2 = new TaskItem {Title = "Comprar control",
            Description= "Comprar control para ser Feliz"};

        var resultado1 = _repo.Add(tarea1);
        var resultado2 = _repo.Add(tarea2);

        //Act
        var resultado = _repo.GetAll();
        
        //Assert
        resultado.Should().HaveCount(2);
    }

    [Fact]
    public void GetById_TareaExistente_RetornaTarea(){
        
        //Arrange        
        var tarea1 = new TaskItem {Title = "Comprar Guitarra",
            Description= "Comprar Guitarra para ser Feliz"};

        var tareaAgregada = _repo.Add(tarea1);
        
        //Act
        var resultado = _repo.GetById(tareaAgregada.Id);
        
        //Assert
        resultado.Should().NotBeNull();
        resultado!.Title.Should().Be("Comprar Guitarra");
    } 

    //Prueba unitaria para una tarea que no existe
    [Fact]
    public void GetById_TareaNoExistente_RetornaNull(){
        
        //Act
        var resultado = _repo.GetById(1000);
        
        //Assert
        resultado.Should().BeNull();
    }

    [Fact]
    public void Update_TareaExistente_ActualizarPropiedades()
    {
        //Arrage
        var tareaOriginal = _repo.Add(new TaskItem{Title = "Tarea 1", Description = "Tarea 1"});
        var cambioTarea = new TaskItem{Title = "Actualizada", Description = "Actualizada"};
    
        //Act
        var resultado = _repo.Update(tareaOriginal.Id, cambioTarea);

        //Assert
        resultado.Should().NotBeNull();
        resultado!.Title.Should().Be("Actualizada");
    }

    //Actualizar una tarea que no existe
    [Fact]
    public void Update_TareaNoExistente_RetornaNull(){
        //Arrange
        var cambioTarea = new TaskItem{Title = "Actualizada", Description = "Actualizada"};
    
        //Act
        var resultado = _repo.Update(1000, cambioTarea);

        //Assert
        resultado.Should().BeNull();
    }

    //Eliminar una tarea existente
    [Fact]
    public void Delete_TareaExistente_RetornaTrue(){
        //Arrange
        var tareaAgregada = _repo.Add(new TaskItem{Title = "Eliminar tarea"});

        //Act
        var resultado = _repo.Delete(tareaAgregada.Id);

        //Assert
        resultado.Should().BeTrue();
        _repo.GetById(tareaAgregada.Id).Should().BeNull();        
    }

    //Eliminar una tarea que no existe
    [Fact]
    public void Delete_TareaNoExistente_RetornaFalse(){
        
        //Act
        var resultado = _repo.Delete(1000);

        //Assert
        resultado.Should().BeFalse();        
    }


}