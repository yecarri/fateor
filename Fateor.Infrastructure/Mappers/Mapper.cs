using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Infrastructure.Mappers
{
    public class Mapper : IMapper
    {
        private AutoMapper.IMapper _Mapper;

        /// <summary>
        /// Creates a Mapper Instance
        /// </summary>
        /// <param name="mapper">A pre-initialized instance of AutoMapper mapper.</param>
        public Mapper(AutoMapper.IMapper mapper)
        {
            _Mapper = mapper;
        }

        /// <summary>
        /// Maps an object of type TSOURCE to an object of type TDEST
        /// </summary>
        /// <typeparam name="TSOURCE"></typeparam>
        /// <typeparam name="TDEST"></typeparam>
        /// <param name="source">Object to be mapped</param>
        /// <returns>An object of type TDEST mapped from source</returns>
        public TDEST Map<TSOURCE, TDEST>(TSOURCE source)
        {
            return _Mapper.Map<TSOURCE, TDEST>(source);
        }

        public void Map<TSOURCE, TDEST>(TSOURCE source, TDEST dest)
        {
            _Mapper.Map(source, dest);
        }

        /// <summary>
        /// Maps an enumerable of TSOURCE to an enumerable of type TDEST
        /// </summary>
        /// <typeparam name="TSOURCE"></typeparam>
        /// <typeparam name="TDEST"></typeparam>
        /// <param name="source">IEnumerable contining objects to be mapped</param>
        /// <returns></returns>
        public IEnumerable<TDEST> MapEnumerable<TSOURCE, TDEST>(IEnumerable<TSOURCE> source)
        {
            return source.Select(s => Map<TSOURCE, TDEST>(s)).ToList();
        }
    }
}
