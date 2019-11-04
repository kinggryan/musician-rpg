using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Samples.Helpers;

/// <summary>
/// This class is responsible for reading loop YAML files. 
/// </summary>
public static class LoopFileReader {

	class Loop {
		public string name { get; set; }
		public string filename { get; set; }
		public int beatDuration { get; set; }
		public string rhythmString { get; set; }
		public string[] emotionTags {get; set; }
	}

	class LoopFile {
		public List<Loop> loops { get; set; }
	}

	public static List<AudioLoop> ReadLoopFile(TextAsset fileAsset) {
		// Setup the input
		var input = new StringReader(fileAsset.text);

		var deserializer = new DeserializerBuilder()
			.WithNamingConvention(new CamelCaseNamingConvention())
			.Build();

		// Get that song!
		var loopFile = deserializer.Deserialize<LoopFile>(input);
		var audioLoops = new List<AudioLoop>();

		foreach(var loop in loopFile.loops) {
			var newLoop = new AudioLoop(loop.name,loop.filename,loop.beatDuration,loop.rhythmString,loop.emotionTags);
			audioLoops.Add(newLoop);
		}

		return audioLoops;
	}
}
