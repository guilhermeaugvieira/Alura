using LeilaoOnline.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace LeilaoOnline.Core.Tests
{
    public class LeilaoRecebeOferta
    {
        [Theory]
        [InlineData(2, new double[] { 800, 900 }, 1000)]
        [InlineData(4, new double[] { 800, 900, 1000, 1400 }, 1500)]
        public void NaoPermiteNovosLancesDadoLeilaoFinalizado(int qtdLances, double[] ofertas, double posOferta)
        {
            // Arrange
            var leilao = new Leilao("Van Gogh");
            var fulano = new Interessada("Fulano", leilao);
            var maria = new Interessada("Maria", leilao);

            leilao.IniciaPregao();

            for (int i = 0; i < ofertas.Length; i++)
            {
                if ((i % 2) == 0) leilao.RecebeLance(fulano, ofertas[i]);
                else leilao.RecebeLance(maria, ofertas[i]);
            }

            leilao.TerminaPregao();

            // Act
            leilao.RecebeLance(fulano, posOferta);


            // Assert
            Assert.Equal(qtdLances, leilao.Lances.Count());
        }

        [Fact]
        public void NaoAceitaProximoLanceDadoQueMesmoClienteRealizouUltimoLance()
        {
            // Arrange
            var leilao = new Leilao("Van Gogh");
            var fulano = new Interessada("Fulano", leilao);

            leilao.IniciaPregao();

            leilao.RecebeLance(fulano, 800);

            // Act
            leilao.RecebeLance(fulano, 1000);

            leilao.TerminaPregao();

            // Assert
            var qtdLances = 1;
            Assert.Equal(qtdLances, leilao.Lances.Count());
        }
    }

}
