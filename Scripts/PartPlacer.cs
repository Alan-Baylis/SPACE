using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using Random = UnityEngine.Random;

namespace SpaceshipGen
{
    public class PartPlacer
    {
        public static IEnumerable<Part> PlaceParts(ParallelSpaceshipParameters param, SkeletonNode rootNode, IEnumerable<Part> prefabs)
        {
            var parts = new List<Part>();

            parts.AddRange(PlaceSupports(rootNode, prefabs));
            parts.AddRange(PlaceStructurals(param, rootNode, prefabs));

            return parts;
        }

        static IEnumerable<Part> PlaceSupports(SkeletonNode parent, IEnumerable<Part> prefabs)
        {
            var placedParts = new List<Part>();
            // call actual supports creator
            placedParts.AddRange(RecursivePlaceSupports(parent, prefabs));

            if (parent.frontNode != null)
            {
                placedParts.AddRange(PlaceSupports(parent.frontNode, prefabs));
            }
            return placedParts;
        }

        static IEnumerable<Part> RecursivePlaceSupports(SkeletonNode parent, IEnumerable<Part> prefabs)
        {
            var placedParts = new List<Part>();

            var connector = GetRandomPart(prefabs, PartType.Connector);

	        if (connector == null)
		        return new Part[0];

            var start = parent.transform.position;
            var r = connector.radius;

            foreach (var wing in parent.wingNodes)
            {
                var v = wing.transform.position - start;
                // calculate scale factor
                var f = v.magnitude / (2 * r);
                var n = Mathf.Floor(f);
                n += (v.magnitude % (2 * r) > r) ? 1 : 0;
                var scale = f / n;

                var rotation = Quaternion.LookRotation(v);

                while (n > 0)
                {
                    var position = start + (v.normalized * r * scale) * (2 * n - 1);
                    Instantiate(connector, position, rotation, placedParts, scale);
                    n--;
                }
                // recursive call wings
                placedParts.AddRange(RecursivePlaceSupports(wing, prefabs));
            }

            return placedParts;
        }

        static IEnumerable<Part> PlaceStructurals(ParallelSpaceshipParameters param, SkeletonNode parent, IEnumerable<Part> prefabs)
        {
            var placedParts = new List<Part>();

            placedParts.AddRange(RecursivePlaceStructural(param, parent, prefabs));

            foreach (var wing in parent.wingNodes)
            {
                placedParts.AddRange(PlaceStructurals(param, wing, prefabs));
            }

            return placedParts;
        }

        static IEnumerable<Part> RecursivePlaceStructural(ParallelSpaceshipParameters param, SkeletonNode parent, IEnumerable<Part> prefabs, Part rear = null)
        {
            var placedParts = new List<Part>();

            var frontPosition = parent.frontNode.transform.position;
            var rearPosition = parent.transform.position;
            var vector = frontPosition - rearPosition;
            var rotation = parent.transform.rotation;

            // place rear connector & engine by probability
            if (rear == null)
            {
	            var intersection = GetRandomPart(prefabs, PartType.Intersection);

	            if (intersection == null)
		            return placedParts;

				rear = Instantiate(intersection, rearPosition, rotation, placedParts, parent.scalefactor);
                if (param.engineProbability > Random.value)
                {
                    // place engine
                    var engine = GetRandomPart(prefabs, PartType.Engine);

					if (engine == null)
						return placedParts;

                    var enginePos = rearPosition - vector.normalized * (rear.radius + engine.radius * parent.scalefactor);
                    Instantiate(engine, enginePos, rotation, placedParts, parent.scalefactor);
                }
            }

            // place front depending on type
            PartType frontType;
            if (parent.frontNode.type == PartType.Intersection)
            {
                frontType = parent.frontNode.type;
            }
            else if (param.cockpitProbability >= Random.value)
            {
                frontType = PartType.Cockpit;
            }
            else
            {
                frontType = PartType.Ending;
            }

	        var frontPart = GetRandomPart(prefabs, frontType);

	        if (frontPart == null)
		        return placedParts;

			var front = Instantiate(frontPart, frontPosition, rotation, placedParts, parent.frontNode.scalefactor);

            // update positions & vector
            rearPosition += vector.normalized * rear.radius;
            frontPosition -= vector.normalized * front.radius;
            vector = frontPosition - rearPosition;

            // calculate median scaling of vector parts
            var scale = (parent.scalefactor + parent.frontNode.scalefactor) / 2;

            // virtualize parts for vector
            var fillers = new List<Part>();
            var filled = 0F;
            do
            {
                var part = GetRandomPart(prefabs, PartType.Structural);

	            if (part == null)
	            {
		            return placedParts;
	            }

                fillers.Add(part);
                filled += 2 * part.radius * scale;
            } while (filled < (vector.magnitude - filled) * fillers.Count * 2); // median part radius > left space

            // calculate final scalefactor
            scale *= vector.magnitude / filled;

            // fill vector with parts
            foreach (var prefab in fillers)
            {
                var partPos = rearPosition + vector.normalized * prefab.radius * scale;
                var part = Instantiate(prefab, partPos, rotation, placedParts, scale);
                // reposition insert point
                rearPosition += vector.normalized * 2 * part.radius;
            }

            // call next
            if (parent.frontNode.frontNode != null)
            {
                placedParts.AddRange(RecursivePlaceStructural(param, parent.frontNode, prefabs, front));
            }
            return placedParts;
        }

        static Part Instantiate(Part original, Vector3 position, Quaternion rotation, List<Part> partList, float scale = 1F)
        {
            var part = UnityEngine.Object.Instantiate(original, position, rotation) as Part;
            part.transform.localScale *= scale;
            part.radius *= scale;
            partList.Add(part);
            return part;
        }

        static List<Part> FilterList(IEnumerable<Part> parts, PartType type)
        {
            var filters = parts.Where(p => p.type == type).ToList();
            
            return filters;
        }

		static Part GetRandomPart(IEnumerable<Part> parts, PartType type)
        {
	        var filteredParts = FilterList(parts, type);
			if (filteredParts.Count != 0)
	        {
				return filteredParts.ElementAt(Random.Range(0, filteredParts.Count()));
	        }
	        
			return null;
        }
    }
}