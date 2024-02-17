using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace practy_code2
{
    internal class Selector
    {
        public string Id { get; set; }
        public string TagName { get; set; }
        public List<string> Classes { get; set; } = new List<string>();
        public Selector Parent { get; set; }
        public Selector Child { get; set; }
        public Selector()
        {
            
        }
        public static Selector ParseSelector(string selectorString)
        {
            Selector rootSelector = new Selector();
            Selector currentSelector = rootSelector;
            string[] selectorLevels = selectorString.Split(' ');
            foreach (string selectorLevel in selectorLevels)
            {
                string[] selectors = new Regex("(?=[#\\.])").Split(selectorLevel).Where(s => s.Length > 0).ToArray();
                foreach (string selector in selectors)
                {
                    if (selector.StartsWith("#"))
                        currentSelector.Id = selector.Split('#').Skip(1).ToString();
                    else if (selector.StartsWith("."))
                        currentSelector.Classes.Add(selector.Split('#').Skip(1).ToString());
                    else if (HtmlHelper.Instance.HtmlTags.Contains(selector))
                        currentSelector.TagName = selector;
                    else
                        throw new ArgumentException($"Invalid HTML tag name: {selector}");
                }
                Selector childSelector = new Selector();
                currentSelector.Child = childSelector;
                childSelector.Parent = currentSelector;
                currentSelector = childSelector;
            }
            currentSelector.Parent.Child = null;
            return rootSelector;
        }
        public override string ToString()
        {
            String s = "";
            if (Id != null)
                s += "Id: " + Id + " ";
            if (TagName != null)
                s += "TagName: " + TagName + " ";
            if (this.Classes.Count() > 0)
            {
                s += "classes:  ";
                foreach (var clas in this.Classes)               
                    s += "class: " + clas + " ";
            }         
            return s;
        }
    } 
}
