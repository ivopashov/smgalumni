using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using System.IO;
using System.Reflection;

namespace SmgAlumni.Utils.Mapping
{
        public static class MapInitializer
        {
            public static void Initialize()
            {
                try
                {
                    var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToList();
                    LoadStandardMappings(types);
                    LoadCustomMappings(types);
                }
                catch (ReflectionTypeLoadException ex)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (Exception exSub in ex.LoaderExceptions)
                    {
                        sb.AppendLine(exSub.Message);
                        FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
                        if (exFileNotFound != null)
                        {
                            if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                            {
                                sb.AppendLine("Fusion Log:");
                                sb.AppendLine(exFileNotFound.FusionLog);
                            }
                        }
                        sb.AppendLine();
                    }
                    string errorMessage = sb.ToString();
                    //Display or log the error based on your application.
                    File.WriteAllText(@"c:\users\Ivo Pashov\Desktop\error.txt", errorMessage);
                }
            }

            private static void LoadCustomMappings(IEnumerable<Type> types)
            {
                var maps = (from t in types
                            from i in t.GetInterfaces()
                            where typeof(IHaveCustomMappings).IsAssignableFrom(t) &&
                                  !t.IsAbstract &&
                                  !t.IsInterface
                            select (IHaveCustomMappings)Activator.CreateInstance(t)).ToArray();

                foreach (var map in maps)
                {
                    map.CreateMappings(Mapper.Configuration);
                }
            }

            private static void LoadStandardMappings(IEnumerable<Type> types)
            {
                var maps = (from t in types
                            from i in t.GetInterfaces()
                            where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>) &&
                                  !t.IsAbstract &&
                                  !t.IsInterface
                            select new
                            {
                                Source = i.GetGenericArguments()[0],
                                Destination = t
                            }).ToArray();

                foreach (var map in maps)
                {
                    Mapper.CreateMap(map.Source, map.Destination);
                }
            }

        }
}
