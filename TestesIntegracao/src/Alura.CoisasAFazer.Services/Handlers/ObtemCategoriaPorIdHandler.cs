using Alura.CoisasAFazer.Core.Commands;
using Alura.CoisasAFazer.Core.Models;
using Alura.CoisasAFazer.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Alura.CoisasAFazer.Services.Handlers
{
    public class ObtemCategoriaPorIdHandler
    {
        IRepositorioTarefas _repo;
        ILogger<ObtemCategoriaPorIdHandler> _logger;

        public ObtemCategoriaPorIdHandler(IRepositorioTarefas repo, ILogger<ObtemCategoriaPorIdHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public ObtemCategoriaPorIdHandler(IRepositorioTarefas repositorio)
        {
            _repo = repositorio;
            _logger = new LoggerFactory().CreateLogger<ObtemCategoriaPorIdHandler>();
        }

        public Categoria Execute(ObtemCategoriaPorId comando)
        {
            return _repo.ObtemCategoriaPorId(comando.IdCategoria);
        }
    }
}
