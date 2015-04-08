using System;

namespace SpaceshipGen
{
    [Serializable]
    public class ParallelSpaceshipParameters : SpaceshipParameters
    {
		[SliderValue(labelText: "Min X Angle", minValue: -45F, maxValue: 45F, wholeNumbers: false, keepSmallerThan: "maxXAngle")]
        public float minXAngle = -30F;
		[SliderValue(labelText: "Max X Angle", minValue: -45F, maxValue: 45F, wholeNumbers: false, keepBiggerThan: "minXAngle")]
	    public float maxXAngle = 30F;
		[SliderValue(labelText: "Min Y Angle", minValue: -60F, maxValue: 60F, wholeNumbers: false, keepSmallerThan: "maxYAngle")]
	    public float minYAngle = -30F;
		[SliderValue(labelText: "Max Y Angle", minValue: -60F, maxValue: 60F, wholeNumbers: false, keepBiggerThan: "minYAngle")]
	    public float maxYAngle = 30F;
		[SliderValue(labelText: "Min Distance", minValue: 10F, maxValue: 100F, wholeNumbers: false, keepSmallerThan: "maxDistance")]
	    public float minDistance = 20F;
		[SliderValue(labelText: "Max Distance", minValue: 10F, maxValue: 100F, wholeNumbers: false, keepBiggerThan: "minDistance")]
	    public float maxDistance = 40F;

        [SliderValue(labelText: "Scale Offset", minValue: 0F, maxValue: 0.9F, wholeNumbers: false)]
        public float scalefactor = 0.2F;
		
		[SliderValue(labelText: "Wing Probability", minValue: 0, maxValue: 1, wholeNumbers: false)]
        public float wingProbability = .8F;
		[SliderValue(labelText: "Structure Probability", minValue: 0, maxValue: 1, wholeNumbers: false)]
        public float structuralProbability = .8F;
		[SliderValue(labelText: "Cockpit Probability", minValue: 0, maxValue: 1, wholeNumbers: false)]
        public float cockpitProbability = .4F;
        [SliderValue(labelText: "Engine Probability", minValue: 0, maxValue: 1, wholeNumbers: false)]
        public float engineProbability = .8F;
		[SliderValue(labelText: "Attachment Probability", minValue: 0, maxValue: 1, wholeNumbers: false)]
        public float attachmentProbability = .5F;

		[SliderValue(labelText: "Max Length", minValue: 1, maxValue: 10, wholeNumbers: true)]
		public int maxLength = 4;
		[SliderValue(labelText: "Max Width", minValue: 0, maxValue: 10, wholeNumbers: true)]
		public int maxWidth = 3;
		[SliderValue(labelText: "Max Wings per Intersection", minValue: 1, maxValue: 8, wholeNumbers: true)]
		public int maxWings = 2;
    }
}