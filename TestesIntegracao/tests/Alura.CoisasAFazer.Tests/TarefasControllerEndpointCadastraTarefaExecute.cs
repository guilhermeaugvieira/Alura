using Alura.CoisasAFazer.Infrastructure;
using Alura.CoisasAFazer.WebApp.Controllers;
using Alura.CoisasAFazer.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;
using Moq;
using Alura.CoisasAFazer.Services.Handlers;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Alura.CoisasAFazer.Core.Models;

namespace Alura.CoisasAFazer.Tests
{
    public class TarefasControllerEndpointCadastraTarefaExecute
    {
        [Fact]
        public async void DadaTarefaComInfoValidasDeveRetornar200()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CadastraTarefaHandler>>();

            var options = new DbContextOptionsBuilder<DbTarefasContext>()
                .UseInMemoryDatabase("DbTarefasContext3").Options;
            var context = new DbTarefasContext(options);
            var repoTarefas = new RepositorioTarefa(context);

            context.Categorias.Add(new Categoria(20, "Estudo"));
            await context.SaveChangesAsync();


            var controlador = new TarefasController(repoTarefas);
            var model = new CadastraTarefaVM();

            model.IdCategoria = 20;
            model.Titulo = "Estudar XUnit";
            model.Prazo = new DateTime(2021, 11, 01);

            // Act
            var retorno = controlador.EndpointCadastraTarefa(model);

            // Assert
            Assert.IsType<OkResult>(retorno);
        }

        [Fact]
        public async void QuandoExcecaoForLancadaDeveRetornar500()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CadastraTarefaHandler>>();

            var mockRepositorio = new Mock<IRepositorioTarefas>();
            mockRepositorio.Setup(r => r.ObtemCategoriaPorId(20)).Returns(new Categoria(20, "Estudo"));
            mockRepositorio.Setup(r => r.IncluirTarefas(It.IsAny<Tarefa[]>())).Throws(new Exception("Houve um erro"));
            var repositorio = mockRepositorio.Object;

            var controlador = new TarefasController(repositorio);
            var model = new CadastraTarefaVM();

            model.IdCategoria = 20;
            model.Titulo = "Estudar XUnit";
            model.Prazo = new DateTime(2021, 11, 01);

            // Act
            var retorno = controlador.EndpointCadastraTarefa(model);

            // Assert
            var statusCodeRetornado = (retorno as StatusCodeResult).StatusCode;
            Assert.IsType<StatusCodeResult>(retorno);
            Assert.Equal(500, statusCodeRetornado);
        }
    }
}
