using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class contains one entry from an audio loop file.
/// Things using loops should refer to loops by name. 
/// Loops can be fetched by name with the static method LoopForName(string)
/// </summary>
public class AudioLoop {

	// None of the audio loop parameters are modifyable once the object has been initialized
	public string name { get; private set; }
	public string filename { get; private set; }
	public int beatDuration { get; private set; }
	public string rhythmString { get; private set; }
	public string[] emotionTags { get; private set; }

	/// <summary>
	/// This dictionary maps from a name to an audio loop.
	/// It is lazily initialized
	/// </summary>
	private static Dictionary<string,AudioLoop> loadedLoops;
	private const string loopsFileDirectory = "audioLoops";

	// Static Methods

	public static AudioLoop GetLoopForName(string name) {
		// Initialize the loaded loops if needed
		if(loadedLoops == null) {
			loadedLoops = AddLoopsToLoadedLoopsDictionary(LoadAllLoopFilesInDirectory(loopsFileDirectory), new Dictionary<string, AudioLoop>());
		}

		return loadedLoops[name];
	}

	private static Dictionary<string,AudioLoop> AddLoopsToLoadedLoopsDictionary(List<AudioLoop> loops, Dictionary<string,AudioLoop> dict) {
		foreach(var loop in loops) {
			if(dict.ContainsKey(loop.name)) {
				Debug.LogError("Tried to load multiple loops with the same name '" + loop.name + "'");
				continue;
			}
			dict[loop.name] = loop;
		}
		return dict;
	}

	private static List<AudioLoop> LoadAllLoopFilesInDirectory(string directory) {
		var loopFiles = Resources.LoadAll<TextAsset>(directory);
		var loops = new List<AudioLoop>();
		foreach(var file in loopFiles) {
			var partialLoops = LoopFileReader.ReadLoopFile(file);
			loops.AddRange(partialLoops);
		}
		return loops;
	}

	// Class Methods

	public AudioLoop(string name, string filename, int beatDuration, string rhythmString, string[] emotionTags) {
		this.name = name;
		this.filename = filename;
		this.beatDuration = beatDuration;
		this.rhythmString = rhythmString;
		this.emotionTags = emotionTags;
	}
}
