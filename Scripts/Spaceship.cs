using UnityEngine;

namespace SpaceshipGen
{
	public class Spaceship : MonoBehaviour
	{
		public int seed;
		public bool randomSeed;
		public Part[] partPrefabs;

		public ParallelSpaceshipParameters parameters;

		public float generationDuration;
		public static float GenerationDuration;
		public static float spaceshipSize;

		private Transform spaceship;

		public static float generationTime;

		public bool RandomSeed
		{
			get { return randomSeed; }
			set { randomSeed = value; }
		}

		public event ParameterProcedurallyChanged parameterChanged;

		public delegate void ParameterProcedurallyChanged(object sender, ParameterProcedurallyChangedArgs args);

		public void Start()
		{
			GenerateSpaceship();
		}

		public void GenerateSpaceship()
		{
			if (spaceship != null)
			{
				Destroy(spaceship.gameObject);
			}
			
			if (RandomSeed)
			{
				var r = new System.Random();
				seed = r.Next();
				parameterChanged.Invoke(this, new ParameterProcedurallyChangedArgs("seed", true));
			}

			GenerationDuration = generationDuration;

			spaceship = SpaceshipGenerator.GenerateSpaceship(seed, partPrefabs, parameters);
			
			var b = new Bounds(Vector3.zero, Vector3.zero);

			foreach (var r in spaceship.GetComponentsInChildren<Renderer>())
			{
				b.Encapsulate(r.bounds);
			}

			spaceship.position -= b.center;
			spaceshipSize = Mathf.Max(b.extents.x, b.extents.y, b.extents.z) + 5;

			generationTime = Time.time;
		}

		public class ParameterProcedurallyChangedArgs
		{
			private string name;
			private bool isGenerated;

			/// <summary>
			/// The name of the parameter that was changed.
			/// </summary>
			public string Name
			{
				get { return name; }
			}

			/// <summary>
			/// True if the parameter was newly generated and not only slightly changed to prevent bugs.
			/// This will become obsolete when we don't have a completely shitty PartPlacer any more.
			/// </summary>
			public bool IsGenerated
			{
				get { return isGenerated; }
			}

			public ParameterProcedurallyChangedArgs(string name, bool isGenerated)
			{
				this.name = name;
				this.isGenerated = isGenerated;
			}
		}
	}
}
