using Alura.CoisasAFazer.Core.Commands;
using Alura.CoisasAFazer.Core.Models;
using Alura.CoisasAFazer.Infrastructure;
using Alura.CoisasAFazer.Services.Handlers;
using System;
using System.Linq;
using Xunit;

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
    }
}
