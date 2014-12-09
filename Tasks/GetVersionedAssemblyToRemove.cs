using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace SmartReferences
{
	public class GetVersionedAssemblyToRemove : Task
	{
		[Required]
		public ITaskItem[] VersionedAssemblies { get; set; }

		[Required]
		public ITaskItem[] ReferencePaths { get; set; }

		[Output]
		public ITaskItem[] ReferencesToRemove { get; set; }

		public override bool Execute ()
		{
			var toRemove = new List<Microsoft.Build.Framework.ITaskItem>();
			
			foreach (var versionedAssembly in VersionedAssemblies)
			{
				Log.LogMessage("Versioned assembly to process: {0}", versionedAssembly);
				
				var assemblyVersion = new Regex(
					string.Format(@"{0}.(?<Major>\d+)\.(?<Minor>\d+)$", versionedAssembly.ItemSpec),
					RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
				
				toRemove.AddRange(ReferencePaths
					.Select(r => new
					{
						Reference = r,
						Match = assemblyVersion.Match(r.GetMetadata("Filename"))
					})
					.Where(x => x.Match.Success)
					.Select(x => new
					{
						Reference = x.Reference,
						Version = new Version(x.Match.Groups["Major"].Value + "." + x.Match.Groups["Minor"].Value)
					})
					.OrderByDescending(x => x.Version)
					// We keep the greatest version
					.Skip(1)
					.Select(x => x.Reference));
			}
			
			ReferencesToRemove = toRemove.ToArray();

			return true;
		}
	}
}
