using Alura.CoisasAFazer.Core.Commands;
using Alura.CoisasAFazer.Core.Models;
using Alura.CoisasAFazer.Infrastructure;
using Alura.CoisasAFazer.Services.Handlers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;
using Moq;

namespace Alura.CoisasAFazer.Tests
{
    public class CadastraTarefaHandlerExecute
    {
        [Fact]
        public void DadaTarefaComInfoValidasDeveIncluirNoBD()
        {
            // Arrange
            var comando = new CadastraTarefa("Estudar XUnit", new Categoria("Estudo"), new DateTime(2019,12,31));

            IRepositorioTarefas repositorio = new RepositorioFake();

            var handler = new CadastraTarefaHandler(repositorio);

            // Act
            handler.Execute(comando);

            // Assert
            var tarefa = repositorio.ObtemTarefas(t => t.Titulo == "Estudar XUnit").FirstOrDefault();

            Assert.NotNull(tarefa);

        }

        [Fact]
        public void QuandoExceptionForLancadaResultadoDeveSerFalse()
        {
            // Arrange
            var comando = new CadastraTarefa("Estudar XUnit", new Categoria("Estudo"), new DateTime(2019, 12, 31));

            var mock = new Mock<IRepositorioTarefas>();

            mock.Setup(r => r.IncluirTarefas(It.IsAny<Tarefa[]>()))
                .Throws(new Exception("Houve um erro na inclusão de tarefas"));

            var repositorio = mock.Object;

            var handler = new CadastraTarefaHandler(repositorio);

            // Act
            CommandResult resultado = handler.Execute(comando);

            // Assert
            Assert.False(resultado.IsSuccess);
        }
    }
}
