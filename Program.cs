// See https://aka.ms/new-console-template for more information
using practy_code2;
using System.Text.RegularExpressions;
using System.Xml.Linq;

Console.WriteLine("Hello, World!");

var html = await Load("https://hebrewbooks.org/beis");
//var html = await Load("https://mail.google.com/mail/u/0/#inbox");

var cleanHtml = new Regex("\\s").Replace(html, "");

var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s=>s.Length > 0).ToArray();

////var htmlElement = htmlLines.FirstOrDefault();

var htmlElement = "<div id=\"my-id\" class=\"my-class-1 my-class-2\" width=\"100%\">text</div>";

var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(htmlElement);
//var attributes = new Dictionary<string, string>();
//var attributes = new Regex(htmlElement, RegexOptions.Compiled); 


HtmlElement root = CreateChild(htmlLines[1].Split(' ')[0], null, htmlLines[1]);
root = BuildTree(root, htmlLines.Skip(2).ToList());


//print

var list = root.FindingElementBySelector("form div div.YhhY8");


Console.ReadLine();

//public 
async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}
static HtmlElement BuildTree(HtmlElement root, List<string> htmlLines) 
{
    HtmlElement currentParent = root;
    foreach (var line in htmlLines)
    {
        if (line.StartsWith("/html"))
            break;
        if (line.StartsWith("/"))
        {
            currentParent = currentParent.Parent;
            continue;
        }
        string tagName = line.Split(' ')[0];
        if (HtmlHelper.Instance.HtmlTags.Contains(tagName))
        {            
            if (HtmlHelper.Instance.HtmlVoidTags.Contains(tagName) || line.EndsWith("/"))
                currentParent.Children.Add(CreateChild(tagName, currentParent, line));
            else
                currentParent = CreateChild(tagName, currentParent, line);
            continue;
        }
        currentParent.InnerHtml += line;
    }
    return root;
}
static HtmlElement CreateChild(string tagName, HtmlElement currentParent, string line)
{
    HtmlElement element = new HtmlElement { Name = tagName, Parent = currentParent };
    var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(line);
    foreach (var attr in attributes)
    {
        string attrName = attr.ToString().Split('=')[0];
        string attrValue = attr.ToString().Split('=')[1].Replace("\"", "");
        if (attrName.ToLower().Equals("class"))
            element.Classes.AddRange(attrValue.Split(" "));
        else if(attrName.ToLower().Equals("id"))
            element.Id = attrValue;
        else
            element.Attributes.Add(attrName, attrValue);
    }
    return element;
}
static void PrintTree(HtmlElement root, string indentation)
{
    Console.WriteLine($"{indentation}{root}");
    foreach (var child in root.Children)
        PrintTree(child, indentation + "  ");
}