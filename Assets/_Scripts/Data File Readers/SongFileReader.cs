using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Samples.Helpers;

public static class SongFileReader  {

	// The following classes are used for unarchiving the song data file
	// The loops can be of one of two formats:
	// - Contain a file and a numReps
	// - contain a totalNumBeats
	// In either case, it should contain a chord
	class Loop {
		public string file { get; set; }
		public string chord { get; set; }
		public int numReps { get; set; }
		public int totalNumBeats { get; set; }
	}

	class Section {
		public string name { get; set; }
		public List<Loop> loops { get; set; }
	}

	class Song {
		public List<Section> sections { get; set; }
		public List<string> structure { get; set; }
	}

	public static SongSection[] ReadSongFile(string filename) {
			// Get teh file asset and string
			var fileAsset = (TextAsset)Resources.Load(filename,typeof(TextAsset));

			// Setup the input
            var input = new StringReader(fileAsset.text);

			var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();

			// Get that song!
			var song = deserializer.Deserialize<Song>(input);

            // create the song section dictionary
			var sectionDictionary = new Dictionary<string, Section>();
			foreach(var section in song.sections) {
				sectionDictionary[section.name] = section;
			}

			var songPlayerSections = new List<SongSection>();
			foreach(var sectionName in song.structure) {
				var section = sectionDictionary[sectionName];

				// Create the correct song section
				var playerSongSection = new SongSection();
				playerSongSection.name = section.name;

				var phrases = new List<SongPhrase>();
				foreach(var loop in section.loops) {
					SongPhrase phrase;
					if(loop.totalNumBeats > 0) {
						phrase = new SongPhrase(loop.chord, loop.totalNumBeats);
					} else {
						phrase = new SongPhrase(loop.file,loop.chord,loop.numReps);
					}
					phrases.Add(phrase);
				}

				playerSongSection.phrases = phrases.ToArray();
				songPlayerSections.Add(playerSongSection);
			}

			return songPlayerSections.ToArray();
	}
}
