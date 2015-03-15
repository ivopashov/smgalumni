using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace SmgAlumni.Utils.Mapping
{
    public static class Extensions
    {
        public static TDestination Map<TSource, TDestination>(
    this TDestination destination, TSource source)
        {
            return Mapper.Map(source, destination);
        }
    }
}
