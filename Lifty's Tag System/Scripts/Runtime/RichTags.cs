using System.Collections.Generic;
using UnityEngine;

namespace Lifty.TagSystem
{
    [System.Serializable]
    public class RichTags
    {
        [SerializeField] private List<RichTag> _tags = new List<RichTag>();

        public RichTag[] GetTags()
        {
            return _tags.ToArray();
        }

        public void AddTag(RichTag richTag)
        {
            _tags.Add(richTag);
        }
        
        public void RemoveTag(RichTag richTag)
        {
            _tags.Remove(richTag);
        }

        public bool HasTags(params RichTag[] tagsToFind)
        {
            var hasAllTags = true;
            foreach (var richTag in tagsToFind)
            {
                if (_tags.Contains(richTag)) continue;
                
                hasAllTags = false;
                break;
            }

            return hasAllTags;
        }
        
        public bool HasTag(RichTag tagToFind)
        {
            return _tags.Contains(tagToFind);
        }
        
        public bool HasTags(params string[] tagsToFind)
        {
            var hasAllTags = true;
            foreach (var richTag in tagsToFind)
            {
                var foundTag = false;
                foreach (var tag in _tags)
                {
                    if (tag.name != richTag) continue;

                    foundTag = true;
                    break;
                }

                if (foundTag) continue;

                hasAllTags = false;
                break;
            }

            return hasAllTags;
        }
        
        public bool HasTag(string tagToFind)
        {
            var hasTag = false;
            foreach (var tag in _tags)
            {
                if (tag.name != tagToFind) continue;

                hasTag = true;
                break;
            }

            return hasTag;
        }
    }
}
