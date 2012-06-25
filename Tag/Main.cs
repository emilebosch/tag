using System.Text;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Demo
{
	public class Tag
	{
		List<Tag> innerTags = new List<Tag>();
		List<Tag> siblingTags = new List<Tag>();
		Dictionary<string,string> Attrs = new Dictionary<string, string>();
		string tag, content;

		public Tag(
			string tag, 
        	string content = null, 
			string name = null,
			string type = null,
			string href = null,
			string title = null, 
			string value = null,
			string src = null,
			string @class = null,
			string id = null,
			string target = null,
			string alt = null,
			string rel = null, 
			string style = null, 
			params Tag[] childTags) 
		{
			this.tag = tag;

			//Split shorthand

			if(this.tag[0]=='.'||this.tag[0]=='#') 
				this.tag = "div"+this.tag;
	
			var split = this.tag.Split (new char[] {'#'}, 2);
			if (split.Length > 1) 
			{
				this.tag = split [0];
				Attr("id", split [1]);
			}

			split = this.tag.Split (new char[] {'.'}, 2);
			if (split.Length > 1) 
			{	
				this.tag = split [0];
				Attr("class", split [1]);
			}

			this.content = content;
			Attr("name"		,name);
			Attr("type"		,type);
			Attr("class"	,@class);
			Attr("href"		,href);
			Attr("target"	,target);
			Attr("value"	,value);
			Attr("rel"		,rel);
			Attr("src"		,src);
			Attr("alt"		,alt);
			Attr("title"	,title);
			Attr("style"	,style);
			Attr("id"		,id);

			this.innerTags.AddRange(childTags);
		}

		public Tag(string tag, params Tag[] children) : this(tag , childTags:children, content:null, name:null)
		{
		}

		public Tag(string tag, IEnumerable<Tag> children) : this(tag , childTags:children.ToArray(), content:null, name:null)
		{
		}

		public Tag Attr(string name, object value) 
		{
			if(value!=null) 
			{
				if(!Attrs.ContainsKey(name))
					Attrs.Add(name,value.ToString());
				else
					Attrs[name]=value.ToString();
			}
			return this;
		}

		public override string ToString ()
		{
			var tagHtml = new StringBuilder();
			tagHtml.AppendFormat("<{0}",tag);

			if(Attrs!=null && Attrs.Count!=0) 
				foreach(var attr in Attrs) 
					tagHtml.AppendFormat(" {0}='{1}'",attr.Key,attr.Value);

			if(innerTags.Count()==0 && content==null) 
			{
				tagHtml.Append("/>");
				foreach(var nextTag in siblingTags) 	
					tagHtml.Append(nextTag.ToString());	

				return tagHtml.ToString();
			}

			tagHtml.Append(">");

			if(content!=null)
				tagHtml.Append(content);

			foreach(var currentTag in innerTags) 
				tagHtml.Append(currentTag.ToString());

			tagHtml.AppendFormat("</{0}>",tag);
			foreach(var nextTag in siblingTags) 	
				tagHtml.Append(nextTag.ToString());	

			return tagHtml.ToString();
		}

		public static implicit operator Tag(string shorthand) 
   		{
			return new Tag(shorthand);
  		}

		 public static Tag operator +(Tag t1, Tag t2) 
		 {
			t1.siblingTags.Add(t2);
			return t1;
		 }
	}

    class DemoApp
    {		
		static void Dump(Tag t) 
		{
			Console.WriteLine (t);
		}

        static void Main (string[] args)
		{		
			//- creating an simple A link, with id and class

			var link = new Tag("a","Click for oblivion!", href:"http://somewhere/far/away", id:"starlink", @class:"starred");

			Dump(link);

			//- the same thing but now via shorthand - the . denotes an class and the # a id

			var linkshorthand = new Tag("a.starlink#starred","Click for oblivion!", href:"http://somewhere/far/away");

			Dump(linkshorthand);

			//- creating an UL with child LI

			var nested = new Tag("ul",
			    new Tag("li", "Look im an li!", @class:"hello", id:"hello"),
				new Tag("li", "Look im another li!", @class:"awesome"));

			Dump(nested);

			//- creating two sibling breaks

			var siblings = new Tag("br");
			siblings += new Tag("br");

			Dump(siblings);

			// doing the same thing but lazy via string implicit string conversion

			Tag lazysiblings = "hr"; 
			lazysiblings += "hr";
			lazysiblings += (Tag)"hr.rainbow" + (Tag)"hr.doublerainbow";

			Dump(lazysiblings);

			//- or create a image with class pretty and id main, and set the src to google and add a rainbow

			var lazyWithAttributes = ((Tag)"img.pretty#main").Attr("src","http://www.lolcats.com") + "hr.rainbow";
			Dump(lazyWithAttributes);
	
			//- some more samples

			var linq = new []{ "Coffee","Crack", "Energy drink"};

			var 
			html = 
				new Tag("ul.starred#main",
				   	new Tag("li.fav",  content: "CONTENT", name: "MYNAME", type: "MYTYPE"),
					new Tag("li.fav", "CONTENT", name: "MYNAME"));

			html += 
				new Tag("div.CLASS#ID",content: "CONTENT", id: "myid", name:"NAME", href:"HREF", 
				        target:"TARGET", title:"TITLE", style:"asd", alt:"ALT", src:"SRC");

			html += "hr.CLASS";
			html += new Tag("ul#toplist", 
	                from 
	                	stimulant 
	                in 
	                	linq 
	                select 
	                	new Tag("li",stimulant, @class:(stimulant=="Crack"?"illegal":"ok")));
	
			Dump(html);

			Tag divwithclass = ".someclasss";
			Dump(divwithclass);

			Tag divwithid = "#someid";
			Dump(divwithid);
		}
	}
}