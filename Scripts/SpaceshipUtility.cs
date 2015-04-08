using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace SpaceshipGen
{
    public class SpaceshipUtility
    {
        public static List<Part> FilterList(List<Part> parts, PartType type)
        {
            List<Part> filters = new List<Part>(parts.Where(p => p.type == type));
            if (filters.Count == 0)
            {
                throw new ApplicationException(String.Format("No prefab of type {0} found.", type));
            }
            return filters;
        }
        
        public static Part GetRandomPart(List<Part> parts, PartType type)
        {
            parts = FilterList(parts, type);
            return parts.ElementAt(Random.Range(0, parts.Count));
        }
    }
}

