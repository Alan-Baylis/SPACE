using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SpaceshipGen
{
	public static class SpaceshipGenerator
	{
		public static Transform GenerateSpaceship(int seed, Part[] partPrefabs, ParallelSpaceshipParameters parameters)
		{
			Random.seed = seed;

			var rootNode = SkeletonGenerator.GenerateSkeleton(parameters);

			var parts = new List<Part>();
			parts.AddRange(PartPlacer.PlaceParts(parameters, rootNode, partPrefabs));
			parts.AddRange(PartAttacher.AttachParts(parameters, parts, partPrefabs));
			parts.AddRange(Mirror(parts));

			var ship = new GameObject("Spaceship");
			ship.transform.position = rootNode.transform.position;
			ship.transform.rotation = rootNode.transform.rotation;

			foreach (var p in parts)
			{
				p.transform.parent = ship.transform;
			}

			rootNode.KillAllNodes();

			return ship.transform;
		}

		static IEnumerable<Part> Mirror(IEnumerable<Part> origins)
		{
			var mirrored = new List<Part>();

			foreach (var origin in origins)
			{
				var pos = origin.transform.position;
				if (Mathf.Abs(pos.x) > 0.5)
				{
					var mirrorPos = new Vector3(-pos.x, pos.y, pos.z);
					var rot = origin.transform.rotation;
					var mirrorRot = Quaternion.Euler(rot.eulerAngles.x, -rot.eulerAngles.y, -rot.eulerAngles.z);
					var mirror = Object.Instantiate(origin, mirrorPos, mirrorRot) as Part;
					mirrored.Add(mirror);
				}
			}
			return mirrored;
		}
	}
}