using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Lifty.TagSystem.Editor
{
    [CustomPropertyDrawer(typeof(RichTags))]
    public class RichTagger_Inspector : PropertyDrawer
    {
        public VisualTreeAsset InspectorXML;

        private VisualElement _inspector;
        private List<RichTag> _currentTags;

        private SerializedProperty _property;
        private RichTags _tagger;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            _inspector = new VisualElement();
            InspectorXML.CloneTree(_inspector);

            //Get all elements
            _currentTags = Resources.LoadAll<RichTag>("Tags/").ToList();
            _property = property;
            _tagger = (RichTags) property.boxedValue;

            var searchField = (ToolbarSearchField) _inspector.Q("SearchField");
            var searchList = _inspector.Q("SearchList");

            //Create tags that the object has
            PopulateSelectedTags();

            //Show search field when selected
            searchField.RegisterCallback<FocusInEvent>(evt =>
            {
                searchList.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
                PopulateGrid(searchField.value);
            });
            
            //Hide search field when deselected
            searchField.RegisterCallback<FocusOutEvent>(evt =>
            {
                searchList.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            });
            
            //Populate grid with tags
            searchField.RegisterValueChangedCallback(evt =>
            {
                PopulateGrid(searchField.value);
            });

            return _inspector;
        }

        private void PopulateSelectedTags()
        {
            _property.boxedValue = _tagger;
            _property.serializedObject.ApplyModifiedProperties();
            _property.serializedObject.Update();
            
            var tagsGrid = _inspector.Q("TagsGrid");
            tagsGrid.hierarchy.Clear();

            var objectTags = _tagger.GetTags();
            foreach (var tag in objectTags)
            {
                var newTag = tag.GetVisualTag();
                
                //Delete button
                var deleteButton = new VisualElement();
                deleteButton.name = "TagPreviewIcon";

                var deleteIcon = Resources.Load<Sprite>("Icons/DeleteTag_Icon");
                deleteButton.style.backgroundImage = new StyleBackground(Background.FromSprite(deleteIcon));
                deleteButton.style.marginLeft = new StyleLength(10);
                
                deleteButton.RegisterCallback<MouseDownEvent>(evt =>
                {
                    _tagger.RemoveTag(tag);
                    _currentTags.Add(tag);
                    tagsGrid.hierarchy.Remove(newTag);
                    PopulateGrid("");

                }, TrickleDown.TrickleDown);
                
                newTag.Add(deleteButton);
                tagsGrid.hierarchy.Add(newTag);

                //Make sure search only shows not selected tags
                if (_currentTags.Contains(tag))
                    _currentTags.Remove(tag);
            }
        }

        private void PopulateGrid(string value)
        {
            _property.boxedValue = _tagger;
            _property.serializedObject.ApplyModifiedProperties();
            _property.serializedObject.Update();
            
            var searchListGrid = _inspector.Q("SearchListGrid");
            searchListGrid.hierarchy.Clear();

            //Eliminate tags without substring
            var foundTags = new List<RichTag>(_currentTags);
            if (!string.IsNullOrEmpty(value))
            {
                for (int i = foundTags.Count - 1; i >= 0; i--)
                {
                    var tagString = foundTags[i].tagName.ToLower();
                    if (tagString.Contains(value.ToLower())) continue;

                    foundTags.Remove(foundTags[i]);
                }
            }
                
            //Create visual elements
            foundTags.ForEach(tag =>
            {
                var newTag = tag.GetVisualTag();

                //Add the ability to select tag
                newTag.RegisterCallback<MouseDownEvent>(evt =>
                {
                    _currentTags.Remove(tag);
                    _tagger.AddTag(tag);
                    searchListGrid.hierarchy.Remove(newTag);
                    PopulateSelectedTags();

                }, TrickleDown.TrickleDown);
                
                searchListGrid.hierarchy.Add(newTag);
            });
        }
    }
}
