using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Infrastructure.Mappers
{
    public interface IMapper
    {
        /// <summary>
        /// Maps an object of type TSOURCE to an object of type TDEST
        /// </summary>
        /// <typeparam name="TSOURCE"></typeparam>
        /// <typeparam name="TDEST"></typeparam>
        /// <param name="source">Object to be mapped</param>
        /// <returns>An object of type TDEST mapped from source</returns>
        TDEST Map<TSOURCE, TDEST>(TSOURCE source);

        void Map<TSOURCE, TDEST>(TSOURCE source, TDEST dest);

        /// <summary>
        /// Maps an enumerable of TSOURCE to an enumerable of type TDEST
        /// </summary>
        /// <typeparam name="TSOURCE"></typeparam>
        /// <typeparam name="TDEST"></typeparam>
        /// <param name="source">IEnumerable contining objects to be mapped</param>
        /// <returns>An IEnumerable of TDEST with the objects mapped</returns>
        IEnumerable<TDEST> MapEnumerable<TSOURCE, TDEST>(IEnumerable<TSOURCE> source);
    }
}
