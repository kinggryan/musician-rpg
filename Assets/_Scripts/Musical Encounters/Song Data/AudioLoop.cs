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
	public RhythmString rhythmString { get; private set; }
	public string[] emotionTags { get; private set; }

	/// <summary>
	/// This dictionary maps from a name to an audio loop.
	/// It is lazily initialized
	/// </summary>
	private static Dictionary<string,AudioLoop> loadedLoops;
	private const string loopsFileDirectory = "audioLoops";

	// Static Methods

	public static AudioLoop GetLoopForName(string name) {
		if(name == null || name == "") {
			Debug.LogError("Cannot load loop with null/empty name.");
			return null;
		}

		// Initialize the loaded loops if needed
		if(loadedLoops == null) {
			loadedLoops = AddLoopsToLoadedLoopsDictionary(LoadAllLoopFilesInDirectory(loopsFileDirectory), new Dictionary<string, AudioLoop>());
		}

		if(!loadedLoops.ContainsKey(name)) {
			Debug.LogError("ERROR: No Loop Found With Name " + name);
			return null;
		}

		return loadedLoops[name];
	}

	public static List<string> GetMostCommonEmotionTagsInLoops(List<AudioLoop> loops) {
		var tempDict = new Dictionary<string,int>();
		var maxCount = 0;
		foreach(var loop in loops) {
			foreach(var tag in loop.emotionTags) {
				if(tempDict.ContainsKey(tag)) {
					tempDict[tag] = tempDict[tag] + 1;
				} else {
					tempDict[tag] = 1;
				}
				maxCount = Mathf.Max(maxCount,tempDict[tag]);
			}
		}

		var listOfTags = new List<string>();

		foreach(var kvPair in tempDict) {
			if(kvPair.Value == maxCount) {
				listOfTags.Add(kvPair.Key);
			}
		}

		return listOfTags;
	}

	public static HashSet<string> GetAllEmotionsInLoops(List<AudioLoop> loops) {
		var set = new HashSet<string>();
		foreach(var loop in loops) {
			foreach(var emotion in loop.emotionTags) {
				set.Add(emotion);
			}
		}

		return set;
	}

	// Appends the rhythm strings of a set of lops
	public static RhythmString AppendRhythmStrings(List<AudioLoop> loops) {
		var str = new RhythmString("");
		foreach(var loop in loops) {
			str = str.AppendRhythmString(loop.rhythmString);
		}
		return str;
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
		this.rhythmString = new RhythmString(rhythmString);
		this.emotionTags = emotionTags;
	}

	/// <summary>
	/// This method returns the rhythm string of this loop in the given beat range
	/// If start and end beat are the same, it will include exactly one beat's worth of rhythm string
	/// The method assumes that the loop starts on beat 0 and repeats forever
	/// </summary>
	public RhythmString GetRhythmStringForBeat(int beat) {
		return rhythmString.GetRhythmStringForBeat(beat);
	}

	public int NumMatchedEmotions(List<string> emotions) {
		var count = 0;
		foreach(var emotion in emotionTags) {
			if(emotions.Contains(emotion))
				count++;
		}
		return count;
	}

	/// <summary>
	/// This method returns the number of characters that match between the two rhythm strings
	/// It is assumed that they start at the same time. If the two strings are of different lengths, the shorter one will be appended to itself to extend itself.
	/// </summary>
	public int GetRhythmStringScore(RhythmString otherRhythmString) {
		return rhythmString.GetNumRhythmStringMatches(otherRhythmString);
	}
}
