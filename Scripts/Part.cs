using UnityEngine;

namespace SpaceshipGen
{
    public class Part : MonoBehaviour
    {
        public float radius;
        public PartType type;
        public AttachmentPoint[] attachmentPoints;

	    private Material mat;

	    private void Start()
	    {
		    mat = GetComponentInChildren<Renderer>().sharedMaterial;

			UpdateShader();
	    }

	    public void Update()
	    {
		    UpdateShader();
	    }

	    public void UpdateShader()
	    {
			float generate = Mathf.Clamp01((Time.time - Spaceship.generationTime) / Spaceship.GenerationDuration);

			mat.SetFloat("_Generate", generate);
			mat.SetFloat("_SpaceshipRadius", Spaceship.spaceshipSize);
	    }
    }
}	