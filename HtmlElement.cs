using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practy_code2
{
    internal class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
        public List<string> Classes { get; set; } = new List<string>();
        public string InnerHtml { get; set; } = "";
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; } = new List<HtmlElement>();

        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> q = new Queue<HtmlElement>();
            q.Enqueue(this);
            while (q.Count > 0)
            {
                HtmlElement el = q.Dequeue();
                if (this != el)//???????????????????????????
                    yield return el;

                foreach (HtmlElement child in el.Children)
                    q.Enqueue(child);
            }
        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement current = this.Parent;
            while (current.Parent != null)
            {
                yield return current;
                current = current.Parent;
            }
        }
        public IEnumerable<HtmlElement> FindingElementBySelector(string selector)
        {
            //this.Descendants();
            //ParseSelector(selector);
            HashSet<HtmlElement> elementsList = new HashSet<HtmlElement>();
            foreach (HtmlElement child in this.Descendants())
                child.FindingElementsBySelectorRecursiely(Selector.ParseSelector(selector), elementsList);
            yield return this;
        }
        private void FindingElementsBySelectorRecursiely(Selector selector, HashSet<HtmlElement> elementsList)
        {

            //if()
            if (this.IsMatch(selector))
            {
                if (selector.Child != null)
                    foreach (HtmlElement child in this.Descendants())
                        child.FindingElementsBySelectorRecursiely(selector.Child, elementsList);
                //  selector = selector.Child;
                else
                {
                    elementsList.Add(this);
                    Console.WriteLine("this ");
                }
            }
            else
                return;
        }
        private bool IsMatch(Selector selector)
        {
            return ((selector.Id == null || selector.Id.Equals(this.Id))
                && (selector.Classes.Count() == 0 ||
                (this.Classes != null && selector.Classes != null && selector.Classes.Intersect(this.Classes).Count() == selector.Classes.Count()))
                && (selector.TagName == null || selector.TagName.Equals(this.Name)));
        }
        public override string ToString()
        {
            String s = "";
            if (Id != null)
                s += "Id: " + Id + " ";
            if (Name != null)
                s += "Name: " + Name + " ";
            if (this.Classes.Count() > 0)
            {
                s += "classes:  ";
                foreach (var clas in this.Classes)
                    s += "class: " + clas + " ";
            }
            //if (InnerHtml != null)
            //    s += "InnerHtml: " + InnerHtml + " ";
            return s;
        }
    }

}
