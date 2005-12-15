using System;
using System.IO;

using MonoDevelop.Core;
using MonoDevelop.Projects;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Pads;
using MonoDevelop.Ide.Gui.Content;

namespace MonoDevelop.ChangeLogAddIn
{
	public enum ChangeLogCommands
	{
		InsertHeader,
		InsertEntry
	}

	public class InsertHeaderHandler : CommandHandler
	{
		protected override void Run()
		{
			Document document = ChangeLogAddInHelperMethods.GetActiveChangeLogDocument();
			
			if (document == null)
				return;
			
			IEditableTextBuffer textBuffer = document.Content as IEditableTextBuffer;			
			
			if (textBuffer == null)
				return;
		
			string name = Runtime.Properties.GetProperty("ChangeLogAddIn.Name", "Full Name");
			string email = Runtime.Properties.GetProperty("ChangeLogAddIn.Email", "Email Address");
			string date = DateTime.Now.ToString("yyyy-MM-dd");
			string text = date + "  " + name + " <" + email + "> " + Environment.NewLine + Environment.NewLine;

			textBuffer.InsertText(textBuffer.CursorPosition, text);			
		}

		protected override void Update(CommandInfo info)
		{
			info.Enabled = ChangeLogAddInHelperMethods.GetActiveChangeLogDocument() != null;
		}
	}

	public class InsertEntryHandler : CommandHandler
	{
		protected override void Run()
		{
			Document document = ChangeLogAddInHelperMethods.GetActiveChangeLogDocument();
			
			if (document == null)
				return;
			
			IEditableTextBuffer textBuffer = document.Content as IEditableTextBuffer;			
			
			if (textBuffer == null)
				return;

			ProjectFile projectFile = GetSelectedProjectFile();

			if (projectFile == null)
				return;

			string changeLogFileName = document.FileName;
			string changeLogFileNameDirectory = Path.GetDirectoryName(changeLogFileName);
			string selectedFileName = projectFile.Name;
			string selectedFileNameDirectory = Path.GetDirectoryName(selectedFileName);
	
			if (selectedFileNameDirectory.StartsWith(changeLogFileNameDirectory))
				textBuffer.InsertText(textBuffer.CursorPosition,
					"\t* " + selectedFileName.Substring(changeLogFileNameDirectory.Length + 1) + ":" + Environment.NewLine); 				
		}

		protected override void Update(CommandInfo info)
		{
			info.Enabled = (ChangeLogAddInHelperMethods.GetActiveChangeLogDocument() != null) && (GetSelectedProjectFile() != null);
		}

		private ProjectFile GetSelectedProjectFile()
		{
			Pad pad = IdeApp.Workbench.Pads[typeof(SolutionPad)];

			if (pad == null)
				return null;

			SolutionPad solutionPad = pad.Content as SolutionPad;

			if (solutionPad == null)
				return null;

			ITreeNavigator navigator = solutionPad.GetSelectedNode();

			if (navigator == null)
				return null;

			return navigator.DataItem as ProjectFile;			
		}
	}

	internal class ChangeLogAddInHelperMethods
	{
		public static Document GetActiveChangeLogDocument()
		{
			Document document = IdeApp.Workbench.ActiveDocument;

			if (document == null)
				return null;
			
			if (document.FileName.EndsWith("ChangeLog"))
				return document;
			
			return null;		
		}		
	}
}
	
