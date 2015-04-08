using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpaceshipGen
{
    public static class PartAttacher
    {
		public static IEnumerable<Part> AttachParts(ParallelSpaceshipParameters param, IEnumerable<Part> parts, IEnumerable<Part> prefabs)
        {
            var attached = new List<Part>();         

            foreach (var part in parts)
            {
                foreach (var point in part.attachmentPoints)
                {
                    if (!point.Blocked && param.attachmentProbability >= Random.value)
                    {
                        var modules = new List<Part>(prefabs.Where(p => p.type == PartType.Attachment));
                        var i = Random.Range(0, modules.Count);
                        var module = modules.ElementAt(i);
                        var rotation = part.transform.rotation * point.rotation;
                        var position = part.transform.TransformPoint(point.position); 

                        var attachment = Object.Instantiate(module, position, rotation) as Part;
                        attached.Add(attachment);

                        if (point.symmetric)
                        {
                            var negPos = point.position;
                            negPos.x = -negPos.x;
                            position = part.transform.TransformPoint(negPos);
                            attachment = Object.Instantiate(module, position, rotation) as Part;
                            attached.Add(attachment);
                        }
                    }
                }
            }
            return attached;
        }
    }
}