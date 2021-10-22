using LeilaoOnline.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace LeilaoOnline.Core.Tests
{
    public class LeilaoTerminaPregao
    {
        [Theory]
        [InlineData(1400, new double[] { 800, 900, 1000, 990, 1400 })]
        [InlineData(1000, new double[] { 800, 900, 990, 1000 })]
        [InlineData(800, new double[] { 800 })]
        public void RetornaMaiorValorDadoLeilãoComPeloMenosUmLance(double valorEsperado, double[] ofertas)
        {
            // Arrange
            var modalidade = new OfertaMaiorValor();
            var leilao = new Leilao("Van Gogh", modalidade);
            var fulano = new Interessada("Fulano", leilao);
            var maria = new Interessada("Maria", leilao);

            leilao.IniciaPregao();

            for (int i = 0; i < ofertas.Length; i++)
            {
                if ((i % 2) == 0) leilao.RecebeLance(fulano, ofertas[i]);
                else leilao.RecebeLance(maria, ofertas[i]);
            }

            // Act
            leilao.TerminaPregao();

            // Assert            
            var valorObtido = leilao.Ganhador.Valor;

            Assert.Equal(valorEsperado, valorObtido);
        }

        [Fact] void LancaInvalidOperationExceptionDadoQueOPregaoNaoFoiIniciado()
        {
            // Arrange
            var modalidade = new OfertaMaiorValor();
            var leilao = new Leilao("Van Gogh", modalidade);

            var excecaoObtida = Assert.Throws<InvalidOperationException>(
                // Act
                leilao.TerminaPregao
            );

            // Assert
            var msgEsperada = "Não é possível terminal o pregão sem que ele tenha começado. Para isso utilize o método IniciaPregao";

            Assert.Equal(msgEsperada, excecaoObtida.Message);
        }

        [Fact]
        public void RetornaZeroDadoLeilaoSemLances()
        {
            // Arrange
            var modalidade = new OfertaMaiorValor();
            var leilao = new Leilao("Van Gogh", modalidade);

            // Act
            leilao.IniciaPregao();

            leilao.TerminaPregao();

            // Assert
            double valorEsperado = 0;
            Assert.Equal(valorEsperado, leilao.Ganhador.Valor);
        }

        [Theory]
        [InlineData(1200, 1250, new double[] {800, 900, 1000, 1100, 1190, 1250, 1500})]
        public void RetornaValorSuperiorMaisProximoDadoLeilaoNessaModalidade(double valorDestino, double valorEsperado, double[] ofertas)
        {
            var modalidade = new OfertaSuperiorMaisProxima(valorDestino);
            var leilao = new Leilao("Van Gogh", modalidade);
            var fulano = new Interessada("Fulano", leilao);
            var maria = new Interessada("Maria", leilao);

            leilao.IniciaPregao();

            for (int i = 0; i < ofertas.Length; i++)
            {
                if ((i % 2) == 0) leilao.RecebeLance(fulano, ofertas[i]);
                else leilao.RecebeLance(maria, ofertas[i]);
            }

            // Act
            leilao.TerminaPregao();

            // Assert            
            var valorObtido = leilao.Ganhador.Valor;

            Assert.Equal(valorEsperado, valorObtido);
        }
    }
}
