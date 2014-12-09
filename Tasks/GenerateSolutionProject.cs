using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Build.BuildEngine;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace SmartReferences.Tasks
{
	public class GenerateSolutionProject : Task
	{
		[Required]
		public string SolutionPath { get; set; }

		[Required]
		public string TargetFile { get; set; }

		public override bool Execute ()
		{
			if (File.Exists(TargetFile))
				File.Delete(TargetFile);
	
			Log.LogMessage("SolutionPath: {0}", SolutionPath);
			Log.LogMessage("TargetFile: {0}", TargetFile);			
	
			File.WriteAllLines(TargetFile,
				SolutionWrapperProject
					.Generate(SolutionPath, null, new BuildEventContext(0, 0, 0, 0))
					// First line contains the XML encoding, and I couldn't get it right.
					.Split(new [] { Environment.NewLine }, StringSplitOptions.None)
					.Skip(1), 
				Encoding.Default);

			new FileInfo(TargetFile).Attributes = FileAttributes.Hidden;

			return true;
		}
	}
}
