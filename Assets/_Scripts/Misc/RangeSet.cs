using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Misc
{
    public class RangeSet
    {
        public List<Range> Ranges => _ranges;

        private List<Range> _ranges = new List<Range>();

        public void Add(Range range)
        {
            var existingRange = CheckExistingRanges(range);
            if (existingRange == null)
            {
                _ranges.Add(range);
                return;
            }

            ExpandRange(existingRange, range);
            UnionExistingRanges();
        }

        public void Add(float minValue, float maxValue)
        {
            Add(new Range(minValue, maxValue));
        }

        public void Remove(Range range)
        {
            if (CheckExistingRanges(range) == null)
                return;

            var rangesToRemove = new List<Range>();
            Range rangeToAdd = null;
            foreach (var existingRange in _ranges)
            {
                if (existingRange.Equals(range))
                {
                    rangesToRemove.Add(existingRange);
                    break;
                }

                if (InRange(range.MinValue, existingRange) && InRange(range.MaxValue, existingRange)) // Inside of range
                {
                    rangeToAdd = new Range(range.MaxValue, existingRange.MaxValue);
                    existingRange.MaxValue = range.MinValue;
                    break;
                }

                if (InRange(range.MinValue, existingRange) && !InRange(range.MaxValue, existingRange))  // On right side of range
                {
                    existingRange.MaxValue = range.MinValue;
                }
                else if (!InRange(range.MinValue, existingRange) && InRange(range.MaxValue, existingRange)) // On left side of range
                {
                    existingRange.MinValue = range.MaxValue;
                }
                else if (existingRange.MinValue >= range.MinValue && existingRange.MaxValue <= range.MaxValue) // Covers range
                {
                    rangesToRemove.Add(existingRange);
                }
            }

            if (rangeToAdd != null)
            {
                _ranges.Add(rangeToAdd);
                return;
            }

            RemoveFromExistingRanges(rangesToRemove);
        }

        public void Remove(float minValue, float maxValue)
        {
            Remove(new Range(minValue, maxValue));
        }

        private Range CheckExistingRanges(Range range)
        {
            foreach (Range existingRange in _ranges)
            {
                if (existingRange.Equals(range))
                    return existingRange;

                if (HasIntersaction(existingRange, range))
                {
                    return existingRange;
                }
            }

            return null;
        }

        private void ExpandRange(Range existingRange, Range range)
        {
            existingRange.MinValue = Mathf.Min(existingRange.MinValue, range.MinValue);
            existingRange.MaxValue = Mathf.Max(existingRange.MaxValue, range.MaxValue);
        }

        private bool HasIntersaction(Range existing, Range checking)
        {
            return InRange(existing.MinValue, checking) || InRange(existing.MaxValue, checking) ||
                    InRange(checking.MinValue, existing) || InRange(checking.MaxValue, existing);
        }

        private bool InRange(float valueToCheck, Range range)
        {
            return InRange(valueToCheck, range.MinValue, range.MaxValue);
        }

        private bool InRange(float valueToCheck, float min, float max)
        {
            return min <= valueToCheck && valueToCheck <= max;
        }

        private void UnionExistingRanges()
        {
            var rangesToRemove = new List<int>();
            for (int i = 0; i < _ranges.Count; i++)
            {
                if (rangesToRemove.Contains(i))
                    continue;

                for (int j = i + 1; j < _ranges.Count; j++)
                {
                    if (rangesToRemove.Contains(j))
                        continue;

                    if (HasIntersaction(_ranges[i], _ranges[j]))
                    {
                        _ranges[i].MinValue = Mathf.Min(_ranges[i].MinValue, _ranges[j].MinValue);
                        _ranges[i].MaxValue = Mathf.Max(_ranges[i].MaxValue, _ranges[j].MaxValue);
                        rangesToRemove.Add(j);
                    }
                }
            }

            RemoveFromExistingRanges(rangesToRemove);
        }

        private void RemoveFromExistingRanges(List<int> rangesToRemove)
        {
            foreach (var i in rangesToRemove.OrderByDescending(q => q))
            {
                _ranges.RemoveAt(i);
            }
        }

        private void RemoveFromExistingRanges(List<Range> rangesToRemove)
        {
            foreach (var range in rangesToRemove)
            {
                _ranges.Remove(range);
            }
        }
    }
}