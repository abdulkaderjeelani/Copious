using System;
using Mapster;

namespace Copious.Infrastructure.Mappers {
    public class Mapster : Interface.IMapper {
        public Mapster () {
            TypeAdapterConfig.GlobalSettings.Default.PreserveReference (true);
        }

        public TDestination Map<TSource, TDestination> (TSource source) => source.Adapt<TDestination> ();

        public TDestination Map<TSource, TDestination> (TSource source, TDestination destination) => source.Adapt<TSource, TDestination> (destination);
    }
}