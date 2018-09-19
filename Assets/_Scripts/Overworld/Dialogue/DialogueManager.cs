using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour {

	[SerializeField]
	private TextAsset inkJSONAsset;
	private Story story;

	[SerializeField]
	private Canvas canvas;
	[SerializeField]
	private RectTransform pcDialogueOptionsParent;
	[SerializeField]
	private OverworldDialogueDisplay npcDialogueDisplay;

	[SerializeField]
	private Button buttonPrefab;

	void Awake () {
		StartStory();
	}

	void StartStory () {
		story = new Story (inkJSONAsset.text);
		RefreshView();
	}

	void RefreshView () {
		RemoveChoices ();

		if (story.canContinue) {
			string text = story.Continue ().Trim();
			ShowNPCDialogue(text);
		}

		if(story.currentChoices.Count > 0) {
			for (int i = 0; i < story.currentChoices.Count; i++) {
				Choice choice = story.currentChoices [i];
				Button button = CreateChoiceView (choice.text.Trim ());
				button.onClick.AddListener (delegate {
					OnClickChoiceButton (choice);
				});
			}
		} else if(story.canContinue) {
			Button choice = CreateChoiceView("(Continue)");
			choice.onClick.AddListener(delegate{
				RefreshView();
			});
		} else {
			Button choice = CreateChoiceView("End of story.\nRestart?");
			choice.onClick.AddListener(delegate{
				StartStory();
			});
		}
	}

	void OnClickChoiceButton (Choice choice) {
		story.ChooseChoiceIndex (choice.index);
		RefreshView();
	}

	void ShowNPCDialogue (string text) {
		npcDialogueDisplay.SetText(text, null);
	}

	Button CreateChoiceView (string text) {
		Button choice = Instantiate (buttonPrefab) as Button;
		choice.transform.SetParent (pcDialogueOptionsParent, false);

		Text choiceText = choice.GetComponentInChildren<Text> ();
		choiceText.text = text;

		return choice;
	}

	void RemoveChoices () {
		int childCount = pcDialogueOptionsParent.transform.childCount;
		for (int i = childCount - 1; i >= 0; --i) {
			GameObject.Destroy (pcDialogueOptionsParent.transform.GetChild (i).gameObject);
		}
	}
}
