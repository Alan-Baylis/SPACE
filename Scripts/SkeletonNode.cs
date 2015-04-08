using System.Collections.Generic;
using UnityEngine;

namespace SpaceshipGen
{
    public class SkeletonNode : MonoBehaviour
    {
        public PartType type;
        public float scalefactor;
        public SkeletonNode frontNode;
        public List<SkeletonNode> wingNodes;
        
        public static SkeletonNode Create(Vector3 position, float scalefactor, PartType type)
        {
            var node = new GameObject("SNode (" + type + ")");
            node.transform.position = position;
            node.transform.rotation = Quaternion.LookRotation(Vector3.forward);
            var sn = node.AddComponent<SkeletonNode>();
            sn.scalefactor = scalefactor;
            sn.type = type;
            sn.wingNodes = new List<SkeletonNode>();
            
            return sn;
        }
        
        public void AddFront(SkeletonNode front)
        {
                        
            frontNode = front;
        }

        public void AddWing(SkeletonNode wing)
        {
            if (wingNodes == null)
            {
                wingNodes = new List<SkeletonNode>();
            }
            
            wingNodes.Add(wing);
        }
                
        public SkeletonNode Mirror()
        {
            SkeletonNode clone;
            List<SkeletonNode> clones = new List<SkeletonNode>();
            var origin = transform.position;
            if (origin.x == 0)
            {
                clone = this;
                if (frontNode != null)
                {
                    clones.Add(frontNode.Mirror());
                }
            } else
            {
                var pos = origin;
                pos.x = -pos.x;
                clone = Create(pos, scalefactor, type);
            }
                        
                        
                        
            foreach (var originChild in wingNodes)
            {
                clones.Add(originChild.Mirror());
            }
            foreach (var child in clones)
            {
                clone.AddWing(child);
            }
            return clone;
        }

        public void KillAllNodes()
        {
            if (frontNode != null)
            {
                frontNode.KillAllNodes();
            }
            
            foreach (var c in wingNodes)
            {
                c.KillAllNodes();
            }
            
            Destroy(gameObject);
        }
    }
}