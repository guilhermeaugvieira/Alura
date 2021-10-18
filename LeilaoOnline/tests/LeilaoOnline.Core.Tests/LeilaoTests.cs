using LeilaoOnline.Core;
using System;
using System.Linq;
using Xunit;

namespace LeilaoOnline.Core.Tests
{
    public class LeilaoTests
    {
        [Fact]
        public void LeilaoComLances()
        {
            // Arrange
            var leilao = new Leilao("Van Gogh");
            var fulano = new Interessada("Fulano", leilao);
            var maria = new Interessada("Maria", leilao);
            var beltrano = new Interessada("Beltrano", leilao);

            leilao.RecebeLance(fulano, 800);
            leilao.RecebeLance(maria, 900);
            leilao.RecebeLance(fulano, 1000);
            leilao.RecebeLance(maria, 990);
            leilao.RecebeLance(beltrano, 1400);

            // Act
            leilao.TerminaPregao();

            // Assert
            var valorEsperado = 1400;
            
            var valorObtido = leilao.Ganhador.Valor;

            Assert.Equal(valorEsperado, valorObtido);
            Assert.Equal(beltrano, leilao.Ganhador.Cliente);
        }

        [Fact]
        public void LeilaoSemLances()
        {
            // Arrange
            var leilao = new Leilao("Van Gogh");

            // Act
            leilao.TerminaPregao();

            // Assert
            double valorEsperado = 0;
            Assert.Equal(valorEsperado, leilao.Ganhador.Valor);
        }
    }
}
