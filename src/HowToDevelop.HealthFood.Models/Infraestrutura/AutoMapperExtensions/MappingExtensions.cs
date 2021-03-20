using System;
using System.Collections.Generic;
using System.Text;

namespace HowToDevelop.HealthFood.Infraestrutura.AutoMapperExtensions
{
    public static class MappingExtensions
    {

        /// <summary>
        /// Execute um mapeamento do objeto de origem para um novo objeto de destino. O tipo de fonte é inferido do objeto de origem
        /// </summary>
        /// <typeparam name="TDestination">Tipo de objeto de destino</typeparam>
        /// <param name="source">Source object to map from</param>
        /// <returns>Objeto de destino mapeado</returns>
        public static TDestination Map<TDestination>(this object source)
        {
            //usar AutoMapper para mapear objetos
            return AutoMapperConfiguration.Mapper.Map<TDestination>(source);
        }

        /// <summary>
        /// Execute um mapeamento do objeto de origem para o objeto de destino existente 
        /// </summary>
        /// <typeparam name="TSource">Tipo de objeto fonte</typeparam>
        /// <typeparam name="TDestination">Tipo de objeto de destino</typeparam>
        /// <param name="source">Objeto de origem para mapear</param>
        /// <param name="destination">Objeto de destino para mapear</param>
        /// <returns>Objeto de destino mapeado, mesma instância do objeto de destino passado</returns>
        public static TDestination Map<TSource, TDestination>(this TSource source, TDestination destination)
        {
            //usar AutoMapper para mapear objetos
            return AutoMapperConfiguration.Mapper.Map(source, destination);
        }

    }
}
