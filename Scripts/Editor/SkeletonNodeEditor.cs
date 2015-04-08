using SpaceshipGen;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkeletonNode))]
[CanEditMultipleObjects]
public class SkeletonNodeEditor : Editor
{
		void OnSceneGUI ()
		{
				var node = target as SkeletonNode;

				var rootNode = node.transform.root.GetComponent<SkeletonNode> ();

				RecursiveDrawNodeConnections (rootNode);

				node.scalefactor = Handles.RadiusHandle (Quaternion.identity, node.transform.position, node.scalefactor);
				Handles.Slider (node.transform.position, node.transform.forward);
		}

		void RecursiveDrawNodeConnections (SkeletonNode rootNode)
		{
				var rootPos = rootNode.transform.position;
				
				if (rootNode.frontNode != null) {
				
						Handles.DrawDottedLine (rootPos, rootNode.frontNode.transform.position, 4F);
				}
		
				foreach (var node in rootNode.wingNodes) {
						Handles.DrawLine (rootPos, node.transform.position);

						RecursiveDrawNodeConnections (node);
				}
		}
}
