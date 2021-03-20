using HowToDevelop.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowToDevelop.HealthFood.Infraestrutura.Tests.Builders
{
    public interface ITestBuilder<T> 
    {
        T Build();
        void Reiniciar();
    }
}
