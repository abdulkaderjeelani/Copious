using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace Copious.Infrastructure.Mappers
{
    public class AutoMapper : Interface.IMapper
    {
        private readonly IMapper _autoMapper;
        public AutoMapper(IMapper autoMapper)
        {
            _autoMapper = autoMapper;

        }

        public TDestination Map<TSource, TDestination>(TSource source)
            => _autoMapper.Map<TSource, TDestination>(source);

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
           => _autoMapper.Map<TSource, TDestination>(source, destination);
    }
}
