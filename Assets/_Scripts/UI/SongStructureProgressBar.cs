using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongStructureProgressBar : MonoBehaviour {

	public GameObject songSectionPrefab;
	public UnityEngine.UI.Slider progressSlider;
	public float songProgressBarWidth;
	
	// For quick programming, let's just cycle through these colors
	private Color[] sectionColors = new Color[]{Color.yellow, Color.green};
	private float songProgressRate;
	private float songProgress;
	private bool songPlaying;

	// Creates the progress bar
	public void SetSongStructure(SongSection[] sections, float bpm) {
		var barTransform = GetComponent<RectTransform>();

		// We're going to cycle through colors
		var colorIndex = 0;

		// Determine how many beats the full song is
		var songLengthBeats = 0;
		foreach(var section in sections) {
			foreach(var phrase in section.phrases) {
				songLengthBeats += SongStructureUtilities.NumBeatsForLoop(phrase.loop) * phrase.numTimesToPlay;
			}
		}

		// Set the rate at which the slider should advance, where 0 is an incomplete song and 1 is a fully complete song.
		songProgressRate = 1f / (songLengthBeats / bpm * 60);
		var widthPerBeat = songProgressBarWidth / songLengthBeats;

		// Generate th esections
		var currentSectionPosition = (float)(-0.5*widthPerBeat*songLengthBeats);
		foreach(var section in sections) {
			var sectionObject = GameObject.Instantiate(songSectionPrefab);

			// Set the name of the section game object
			var sectionDisplay = sectionObject.GetComponent<SongSectionDisplay>();
			sectionDisplay.nameText.text = section.name;
			sectionDisplay.background.color = sectionColors[colorIndex];
			colorIndex = (colorIndex+1) % sectionColors.Length;

			var sectionTransform = sectionObject.GetComponent<RectTransform>();

			var beatsInThisSection = 0;
			foreach(var phrase in section.phrases) {
				beatsInThisSection += SongStructureUtilities.NumBeatsForLoop(phrase.loop) * phrase.numTimesToPlay;
			}

			// Position and scale the song section game object
			sectionTransform.SetParent(this.transform);
			sectionTransform.position = Vector3.zero;
			sectionTransform.SetSizeWithCurrentAnchors(UnityEngine.RectTransform.Axis.Horizontal, widthPerBeat*beatsInThisSection);
			sectionTransform.offsetMin = new Vector2(currentSectionPosition, barTransform.offsetMin.y);
			sectionTransform.offsetMax = new Vector2(currentSectionPosition + beatsInThisSection*widthPerBeat, barTransform.offsetMax.y);
			currentSectionPosition += beatsInThisSection*widthPerBeat;
		}

		progressSlider.transform.SetAsLastSibling();
	}

	public void StartSong() {
		songPlaying = true;
		songProgress = 0;
	}

	void Update() {
		if(songPlaying) {
			songProgress += songProgressRate * Time.deltaTime;
			if(songProgress >= 1) {
				songProgress = 1;
				EndSong();
			}
			progressSlider.value = songProgress;

		}
	}

	void EndSong() {
		songPlaying = false;
	}
}
