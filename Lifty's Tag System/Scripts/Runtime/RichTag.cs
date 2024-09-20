using System;
using UnityEngine;

namespace Lifty.TagSystem
{
    [CreateAssetMenu(fileName = "New Tag", menuName = "Rich Tags/New Tag")]
    public class RichTag : ScriptableObject
    {
        public Sprite tagIcon;
        public string tagName;
        public Color tagColor = Color.white;
        public Color tagNameColor = Color.black;
    }
}
