using SpaceshipGen;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Part))]
public class PartEditor : Editor
{
	private static bool editRotation;

	void OnSceneGUI()
	{
		var part = target as Part;
		var t = part.transform;

		part.radius = Handles.RadiusHandle(t.rotation, t.position, part.radius);

		if (part.attachmentPoints == null || part.attachmentPoints.Length == 0)
		{
			return;
		}

		foreach (var attachmentPoint in part.attachmentPoints)
		{
			if (attachmentPoint.rotation.Equals(new Quaternion()))
				attachmentPoint.rotation = Quaternion.identity;

			var rot = t.rotation * attachmentPoint.rotation;
			var pos = t.TransformPoint(attachmentPoint.position);

			if (editRotation)
			{
				rot = Handles.RotationHandle(rot, pos);

				attachmentPoint.rotation = Quaternion.Inverse(t.rotation) * rot;
				Handles.Slider(pos, rot * Vector3.forward);
			}
			else
			{
				attachmentPoint.position = t.InverseTransformPoint(Handles.PositionHandle(pos, rot));
			}
		}

		if(GUI.changed)
			EditorUtility.SetDirty(target);
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button(editRotation ? "Edit attachment point position" : "Edit attachment point rotation"))
		{
			editRotation = !editRotation;
		}

		if (GUI.changed)
			EditorUtility.SetDirty(target);
	}
}
