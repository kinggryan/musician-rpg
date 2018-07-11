using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Samples.Helpers;

public static class SongFileReader  {

	// TODO: Make this actually load from files lul....


	// public SongPlayer.SongPhrase[] ReadSongFile(string filename) {
	// 		// Get teh file asset and string
	// 		var fileAsset = (TextAsset)Resources.Load(filename,typeof(TextAsset));

	// 		// Setup the input
    //         var input = new StringReader(fileAsset.text);

    //         // Load the stream
    //         var yaml = new YamlStream();
    //         yaml.Load(input);

    //         // Examine the stream
    //         var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
	// 		var sectionsAndPhrases = mapping.Children;
	// 		foreach(var kvPair in sectionsAndPhrases) {
	// 			// If this is a phrase, append it as a phrase
	// 			if(kvPair.Key.)
	// 		}
	// }
}
