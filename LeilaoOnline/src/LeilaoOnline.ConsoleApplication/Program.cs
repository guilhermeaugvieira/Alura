using LeilaoOnline.Core;
using System;

namespace LeilaoOnline.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            // Arrange
            var modalidade = new OfertaMaiorValor();
            var leilao = new Leilao("Van Gogh", modalidade);
            var fulano = new Interessada("Fulano", leilao);
            var maria = new Interessada("Maria", leilao);

            leilao.RecebeLance(fulano, 800);
            leilao.RecebeLance(maria, 900);
            leilao.RecebeLance(fulano, 1000);
            leilao.RecebeLance(maria, 990);

            // Act
            leilao.TerminaPregao();

            // Assert
            var valorEsperado = 1000;
            var valorObtido = leilao.Ganhador.Valor;
            
           if (valorEsperado == valorObtido)
            {
                Console.WriteLine("Teste Ok");
            } else
            {
                Console.WriteLine("Teste falhou");
            }
        }
    }
}
