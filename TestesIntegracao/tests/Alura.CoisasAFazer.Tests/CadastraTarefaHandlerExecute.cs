using Alura.CoisasAFazer.Core.Commands;
using Alura.CoisasAFazer.Core.Models;
using Alura.CoisasAFazer.Infrastructure;
using Alura.CoisasAFazer.Services.Handlers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;

namespace Alura.CoisasAFazer.Tests
{
    public class CadastraTarefaHandlerExecute
    {
        [Fact]
        public void DadaTarefaComInfoValidasDeveIncluirNoBD()
        {
            // Arrange
            var comando = new CadastraTarefa("Estudar XUnit", new Categoria("Estudo"), new DateTime(2019,12,31));

            var mock = new Mock<ILogger<CadastraTarefaHandler>>();
            
            var options = new DbContextOptionsBuilder<DbTarefasContext>()
                .UseInMemoryDatabase("DbTarefasContext")
                .Options;

            var contexto = new DbTarefasContext(options);

            IRepositorioTarefas repositorio = new RepositorioTarefa(contexto);

            var handler = new CadastraTarefaHandler(repositorio, mock.Object);

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

            var loggerMock = new Mock<ILogger<CadastraTarefaHandler>>();

            var mock = new Mock<IRepositorioTarefas>();

            mock.Setup(r => r.IncluirTarefas(It.IsAny<Tarefa[]>()))
                .Throws(new Exception("Houve um erro na inclusão de tarefas"));

            var repositorio = mock.Object;

            var handler = new CadastraTarefaHandler(repositorio, loggerMock.Object);

            // Act
            CommandResult resultado = handler.Execute(comando);

            // Assert
            Assert.False(resultado.IsSuccess);
        }

        [Fact]
        public void QuandoExceptionForLancadoDeveSerLogadaMensagemDaExcecao()
        {
            // Arrange
            var mensagemDeErroEsperada = "Houve um erro na inclusão de tarefas";
            var excecaoEsperada = new Exception(mensagemDeErroEsperada);
            var comando = new CadastraTarefa("Estudar XUnit", new Categoria("Estudo"), new DateTime(2019, 12, 31));

            var mockLogger = new Mock<ILogger<CadastraTarefaHandler>>();

            var mock = new Mock<IRepositorioTarefas>();

            mock.Setup(r => r.IncluirTarefas(It.IsAny<Tarefa[]>()))
                .Throws(excecaoEsperada);

            var repositorio = mock.Object;

            var handler = new CadastraTarefaHandler(repositorio, mockLogger.Object);

            // Act
            CommandResult resultado = handler.Execute(comando);

            // Assert
            mockLogger.Verify(l => 
                l.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<object>(),
                    excecaoEsperada,
                    It.IsAny<Func<object, Exception, string>>()
                ), 
                Times.Once());
        }

        delegate void CapturaMensagemLog(LogLevel level, EventId eventId, object state, Exception ex, Func<object, Exception, string> function);

        [Fact]
        public void DadaTarefaComInfoValidasDeveLogar()
        {
            var nomeTarefa = "Estudar XUnit";
            var comando = new CadastraTarefa(nomeTarefa, new Categoria("Estudo"), new DateTime(2019, 12, 31));
            
            var mockLogger = new Mock<ILogger<CadastraTarefaHandler>>();

            LogLevel levelCapturado = LogLevel.Debug;
            EventId eventIdCapturado;
            object stateCapturado;
            Exception exceptionCapturada;
            string retornoExcecao = string.Empty;


            CapturaMensagemLog captura = (level, eventId, state, ex, func) =>
            {
                levelCapturado = level;
                eventIdCapturado = eventId;
                stateCapturado = state;
                exceptionCapturada = ex;
                retornoExcecao = func(state, ex);
            };

            mockLogger.Setup(l =>
                l.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<object>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()
                )
            ).Callback(captura);

            var mockRepositorio = new Mock<IRepositorioTarefas>();

            var handler = new CadastraTarefaHandler(mockRepositorio.Object, mockLogger.Object);

            // Act
            CommandResult resultado = handler.Execute(comando);

            // Assert
            Assert.Equal(LogLevel.Debug, levelCapturado);
            Assert.Contains(nomeTarefa, retornoExcecao);
            
        }
    }
}
