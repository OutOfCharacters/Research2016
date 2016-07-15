using UnityEngine;
using System.Collections;

using UnityEngine.Windows.Speech;

public class VoiceManager : MonoBehaviour {

    //Internal reference to a keywordRecognizer.
    public bool toContinue = false;
    private string response;
    private string[] oneToOneHundred = new string[100];
    private ArrayList responseDirectory = new ArrayList();
    private KeywordRecognizer kr;

    // Use this for initialization
    void Start () {
        createString();

		//Create a new keywordRecognizer, with the words from one to one hundred
		kr = new KeywordRecognizer(oneToOneHundred);

		//Register OnVoiceCommand to kr's onPhraseRecognized
		kr.OnPhraseRecognized += OnVoiceCommand;
    }
    public void startListening()
    {
        kr.Start();
    }
    public void stopListening()
    {
        kr.Stop();
    }

    //when a keyword is recognized
	private void OnVoiceCommand(PhraseRecognizedEventArgs args)
	{
        //if the keyword is anything but "try again"...
        if(!args.text.Equals("try again"))
        {
            //Go to the next level
            toContinue = true;
            //add the responses and confidence levels to their arraylists
            addToResponseDirectory(args.text);
        }
        //otherwise...
        else
        {
            //keyword was "try again", so we wait for a new response
            toContinue = false;
        }
	}

    //creates an array of strings that has the numbers "one" to "one-hundred"
    private void createString()
    {
        for (int i = 0; i < oneToOneHundred.Length; i++)
        {
            oneToOneHundred[i] = i.ToString();
        }
    }
    //it doesn't currently deal with the "try again" situation, I guess I can just add it to the end of the oneToOnehundredArray..

    private void addToResponseDirectory(string toAdd)
    {
        //adds string to arrayList
        responseDirectory.Add(toAdd);
    }

    private void showList()
    {
        //put responseDirectory on the screen for the psychologist to record.
    }
}
