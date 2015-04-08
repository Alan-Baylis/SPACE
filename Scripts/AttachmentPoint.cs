using System;
using UnityEngine;

namespace SpaceshipGen
{
	[Serializable]
	public class AttachmentPoint
	{
		public Vector3 position;
		public Quaternion rotation;

		/// <summary>
		/// Symmetry of attachmentPoints relative to x axis
		/// </summary>
		public bool symmetric;

		private bool blocked;

		public bool Blocked
		{
			get
			{
				return blocked;
			}

			set
			{
				blocked = value;
			}
		}
	}
}