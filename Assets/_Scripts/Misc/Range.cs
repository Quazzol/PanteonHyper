using UnityEngine;

namespace Misc
{
    public class Range
    {
        public float MinValue;
        public float MaxValue;

        public Range(float minValue, float maxValue)
        {
            MinValue = Mathf.Min(minValue, maxValue);
            MaxValue = Mathf.Max(minValue, maxValue);
        }

        public float Gap()
        {
            return MaxValue - MinValue;
        }

        public float Mid()
        {
            return (MaxValue + MinValue) / 2;
        }

        public override bool Equals(object other)
        {
            if (other == null || !(other is Range))
                return false;

            var otherRange = (Range)other;
            return MinValue.AreEqual(otherRange.MinValue) && MaxValue.AreEqual(otherRange.MaxValue);
        }

        public override int GetHashCode()
        {
            return MinValue.GetHashCode() ^ MaxValue.GetHashCode();
        }

        public override string ToString()
        {
            return $"{MinValue} - {MaxValue}";
        }
    }
}