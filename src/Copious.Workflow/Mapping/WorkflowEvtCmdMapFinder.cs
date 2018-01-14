namespace Copious.Workflow.Mapping
{
    using System.Collections.Concurrent;
    using System.Reflection;

    public class WorkflowEvtCmdMapFinder
    {
        static ConcurrentDictionary<string, IEventToCommandMapper> MappingComponentCache = new ConcurrentDictionary<string, IEventToCommandMapper>();

        /// <summary>
        /// Find the relevant component filtering based on source, target event types from the mapping assembly
        /// </summary>
        /// <param name="map"></param>
        /// <param name="notFoundBehavior">What to do if map is not found</param>
        /// <returns>NULL if not found</returns>
        public static IEventToCommandMapper FindMapper(EventToCommandMap map, MapNotFoundBehavior notFoundBehavior)
        {
            var cacheKey = $"{map.EventName}-TO-{map.CommandName}";

            IEventToCommandMapper stageMappingComponent = null;

            if (!MappingComponentCache.ContainsKey(cacheKey))
            {
                MappingComponentCache.AddOrUpdate(cacheKey, stageMappingComponent, (k, v) => v);
            }

            return MappingComponentCache.GetOrAdd(cacheKey, k => null);
        }
    }
}