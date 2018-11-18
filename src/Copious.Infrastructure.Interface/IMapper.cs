using System;
using System.Collections.Generic;
using System.Text;

namespace Copious.Infrastructure.Interface {
    public interface IMapper {
        TDestination Map<TSource, TDestination> (TSource source);
        TDestination Map<TSource, TDestination> (TSource source, TDestination destination);
    }

    public enum Mapper {
        Mapster,
        Automapper
    }
}