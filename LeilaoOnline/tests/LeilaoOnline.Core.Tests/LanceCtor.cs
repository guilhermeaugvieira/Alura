using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LeilaoOnline.Core.Tests
{
    public class LanceCtor
    {
        [Fact]
        public void LanceArgumentExceptionDadoValorNegativo()
        {
            // Arrange
            var valorNegativo = -100;

            var excecaoObtida =  Assert.Throws<ArgumentException>(
                // Act
                () => new Lance(null, valorNegativo)
            );

            // Assert
            var excecaoEsperada = "O valor do lance deve ser positivo";

            Assert.Equal(excecaoEsperada, excecaoObtida.Message);
        }
    }
}
