// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version value="$version"/>
// </file>

using System;
using System.Collections;
using System.Diagnostics;
using System.ComponentModel;

using Mono.Addins;

namespace MonoDevelop.Ide.Codons
{
	[Description ("A file template.")]
	internal class FileTemplateCodon : ExtensionNode
	{
		[Description ("Resource name where the template is stored.")]
		[NodeAttribute("resource", true)]
		string resource;
		
		public string Resource {
			get {
				return resource;
			}
			set {
				resource = value;
			}
		}
	}
}
