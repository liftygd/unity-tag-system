using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Lifty.TagSystem.Editor
{
    [CustomEditor(typeof(RichTag))]
    public class RichTag_Inspector : UnityEditor.Editor
    {
        public VisualTreeAsset InspectorXML;
        
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement newInspector = new VisualElement();
            InspectorXML.CloneTree(newInspector);

            var tagPreview = ((RichTag) serializedObject.targetObject).GetVisualTag();
            newInspector.Add(tagPreview);
            
            return newInspector;
        }
    }
}
