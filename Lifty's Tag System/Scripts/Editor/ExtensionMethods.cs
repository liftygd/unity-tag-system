using Unity.Properties;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Lifty.TagSystem.Editor
{
    public static class ExtensionMethods
    {
        public static VisualElement GetVisualTag(this RichTag tag)
        {
            //Tag body
            var body = new VisualElement();
            body.name = "TagPreview";
            body.AddToClassList("rich-tag");
            body.dataSource = tag;

            var backgroundColorBind = new DataBinding
            {
                dataSourcePath = new PropertyPath(nameof(tag.tagColor)),
                bindingMode = BindingMode.ToTarget
            };
            body.SetBinding("style." + nameof(body.style.backgroundColor), backgroundColorBind);
            
            
            //Tag icon
            if (tag.tagIcon != null)
            {
                var tagIcon = new VisualElement();
                tagIcon.name = "TagPreviewIcon";

                var tagSpriteBind = new DataBinding
                {
                    dataSourcePath = new PropertyPath(nameof(tag.tagIcon)),
                    bindingMode = BindingMode.ToTarget
                };
                tagIcon.SetBinding("style." + nameof(tagIcon.style.backgroundImage), tagSpriteBind);

                body.Add(tagIcon);
            }


            //Tag text
            var tagText = new Label();
            tagText.name = "TagPreviewText";

            var textBind = new DataBinding
            {
                dataSourcePath = new PropertyPath(nameof(tag.tagName)),
                bindingMode = BindingMode.ToTarget
            };
            tagText.SetBinding(nameof(tagText.text), textBind);
            
            var textColorBind = new DataBinding
            {
                dataSourcePath = new PropertyPath(nameof(tag.tagNameColor)),
                bindingMode = BindingMode.ToTarget
            };
            tagText.SetBinding("style." + nameof(tagText.style.color), textColorBind);
            
            body.Add(tagText);

            return body;
        }

        public static bool HasTags(this GameObject obj, params RichTag[] tags)
        {
            var tagger = obj.GetComponent<RichTagger>();
            if (tagger == null) return false;
            
            return tagger.tags.HasTags(tags);
        }
        
        public static bool HasTag(this GameObject obj, RichTag tag)
        {
            var tagger = obj.GetComponent<RichTagger>();
            if (tagger == null) return false;
            
            return tagger.tags.HasTag(tag);
        }
        
        public static bool HasTags(this GameObject obj, params string[] tags)
        {
            var tagger = obj.GetComponent<RichTagger>();
            if (tagger == null) return false;
            
            return tagger.tags.HasTags(tags);
        }
        
        public static bool HasTag(this GameObject obj, string tag)
        {
            var tagger = obj.GetComponent<RichTagger>();
            if (tagger == null) return false;
            
            return tagger.tags.HasTag(tag);
        }
    }
}
