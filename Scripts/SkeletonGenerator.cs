using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpaceshipGen
{
    public class SkeletonGenerator
    {
        public static SkeletonNode GenerateSkeleton(ParallelSpaceshipParameters param)
        {
            // base node
            var scalefactor = Random.Range(1 - param.scalefactor, 1 + param.scalefactor);
            var rootNode = SkeletonNode.Create(new Vector3(0, 0, 0), scalefactor, PartType.Engine);

            // rear wing shape
            RecursiveCreateWings(param, rootNode, param.maxWidth);
            // front nodes
            RecursiveCreateFronts(param, rootNode, param.maxLength);

            // create mirrored skeleton for visualisation
            // rootNode.Mirror ();

            return rootNode;
        }

        /// <summary>
        /// Creates the shape of the wings at the rear section of the skeleton
        /// </summary>
        /// <param name="param">Parameter.</param>
        /// <param name="parent">Parent.</param>
        static void RecursiveCreateWings(ParallelSpaceshipParameters param, SkeletonNode parent, int leftWings)
        {
            var nWings = param.maxWings;
            while (nWings > 0 && leftWings > 0 && param.wingProbability >= Random.value)
            {
                var scalefactor = Random.Range(1 - param.scalefactor, 1 + param.scalefactor);

                var distance = Random.Range(param.minDistance, param.maxDistance);

                var xRot = Random.Range(param.minXAngle, param.maxXAngle);
                var yRot = Random.Range(param.minYAngle, param.maxYAngle) + 90F;
                var zRot = 0F; // !

                var position = parent.transform.position + (Quaternion.Euler(xRot, yRot, zRot) * parent.transform.forward * distance);
                parent.AddWing(SkeletonNode.Create(position, scalefactor, PartType.Engine));
                nWings--;
            }
            foreach (var wing in parent.wingNodes)
            {
                RecursiveCreateWings(param, wing, leftWings - 1);
            }
        }

        /// <summary>
        /// Creates the central structure and duplicates the rear shape of the wings
        /// </summary>
        /// <param name="param">Parameter.</param>
        /// <param name="parent">Parent.</param>
        static void RecursiveCreateFronts(ParallelSpaceshipParameters param, SkeletonNode parent, int leftLength)
        {
            // create new SkeletonNode
            var scalefactor = Random.Range(1 - param.scalefactor, 1 + param.scalefactor);
            var distance = Random.Range(param.minDistance, param.maxDistance);
            var position = parent.transform.position + parent.transform.forward * distance;

            // select node type
            PartType type;
            if (leftLength > 0
                && parent.wingNodes.Count > 0
                && param.structuralProbability >= Random.value)
            {
                type = PartType.Intersection;
            }
            else
            {
                type = PartType.Cockpit;
            }

            var frontNode = SkeletonNode.Create(position, scalefactor, type);
            parent.AddFront(frontNode);

            // create subwing structure
            foreach (var wing in parent.wingNodes)
            {
                if (type == PartType.Cockpit)
                {
                    distance = Random.Range(param.minDistance, param.maxDistance);
                }
                RecursiveCreateSubwings(param, wing, parent, distance);
            }

            // prevent unwinged intersections
            if (parent.wingNodes.Count == 0)
            {
                frontNode.type = PartType.Cockpit;
            }
            // recursive create next frontNode
            else if (type == PartType.Intersection)
            {
                RecursiveCreateFronts(param, frontNode, leftLength - 1);
            }
        }

        /// <summary>
        /// Duplicates the shape of the wings for each row
        /// </summary>
        /// <param name="param">Parameter.</param>
        /// <param name="parent">Parent.</param>
        /// <param name="gParent">G parent.</param>
        /// <param name="distance">Distance.</param>
        static void RecursiveCreateSubwings(ParallelSpaceshipParameters param, SkeletonNode parent, SkeletonNode gParent, float distance)
        {
            var scalefactor = Random.Range(1 - param.scalefactor, 1 + param.scalefactor);
            var position = parent.transform.position + parent.transform.forward * distance;

            // select node type
            PartType type;
            if (gParent.frontNode.type != PartType.Intersection
                || param.structuralProbability <= Random.value)
            {
                type = PartType.Cockpit;
            }
            else
            {
                type = PartType.Intersection;
            }

            var subwingNode = SkeletonNode.Create(position, scalefactor, type);
            parent.AddFront(subwingNode);

            // add connection
            if (type == PartType.Intersection)
            {
                gParent.frontNode.AddWing(subwingNode);
            }

            // recursive create subwings
            foreach (var wing in parent.wingNodes)
            {
                if (type == PartType.Cockpit)
                {
                    distance = Random.Range(param.minDistance, param.maxDistance);
                }
                RecursiveCreateSubwings(param, wing, parent, distance);
            }
        }
    }
}

