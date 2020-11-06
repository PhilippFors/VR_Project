using System;
using System.Collections.Generic;
using Ludiq.Peek;
using Ludiq.PeekCore;

[assembly: MapToPlugin(typeof(Changelog_1_2_0), PeekPlugin.ID)]

namespace Ludiq.Peek
{
	// ReSharper disable once RedundantUsingDirective
	using PeekCore;

	internal class Changelog_1_2_0 : PluginChangelog
	{
		public Changelog_1_2_0(Plugin plugin) : base(plugin) { }
		
		public override SemanticVersion version => "1.2.0";
		public override DateTime date => new DateTime(2020, 09, 19);

		public override IEnumerable<string> changes
		{
			get
			{
				yield return "[Added] Configurable Shortcuts";
			}
		}
	}
}