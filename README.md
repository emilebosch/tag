tag
===

Simple HTML tag building for Mono and .NET runtime.

````csharp
//- creating an simple A link, with id and class

var sample1 = new Tag("a","Click for oblivion!", href:"http://somewhere/far/away", id:"starlink", @class:"starred");
Console.WriteLine (sample1);

//- the same thing but now via shorthand - the . denotes an class and the # a id

var sample2 = new Tag("a.starlink#starred","Click for oblivion!", href:"http://somewhere/far/away");
Console.WriteLine (sample2);

//- creating an UL with child LI

var sample3 = new Tag("ul",
    new Tag("li", "Look im an li!", @class:"hello", id:"hello"),
	new Tag("li", "Look im another li!", @class:"awesome"));

Console.WriteLine (sample3);

//- creating two sibling breaks

var sample4 = new Tag("br");
sample4 += new Tag("br");

Console.WriteLine (sample4);

// doing the same thing but lazy via string implicit string conversion

Tag sample5 = "br"; 
sample5 += "br";
sample5 += (Tag)"hr.rainbow"+(Tag)"hr.doublerainbow";

//- or

var sample6 = (Tag)"hr.pretty#main";
Console.WriteLine (sample6);

//- some more samples

var stimulants = new []{ "Coffee","Crack", "Energy drink"};

var 
html = 
	new Tag("ul.CLASS#ID",
	   	new Tag("li.CLASS#ID",  content: "CONTENT", name: "MYNAME", type: "MYTYPE"),
		new Tag("li.CLASS#ID", "CONTENT", name: "MYNAME"));

html += 
	new Tag("div.CLASS#ID",content: "CONTENT", id: "myid", name:"NAME", href:"HREF", 
	        target:"TARGET", title:"TITLE", style:"asd", alt:"ALT", src:"SRC");

html += "hr.CLASS";
html += new Tag("ul#toplist", 
                from 
                	stimulant 
                in 
                	stimulants 
                select 
                	new Tag("li",stimulant, @class:(stimulant=="Crack"?"illegal":"ok")));

Console.WriteLine (html.ToString());
````