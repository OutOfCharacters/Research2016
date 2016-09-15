using UnityEngine;
using System.Collections;

using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;

public class VoiceManager : MonoBehaviour {

    //Internal reference to a keywordRecognizer.
    public bool toContinue;
    private string response;
    private string[] responses;

    private KeywordRecognizer kr;
    AudioSource auSo;
    public DataStorage ds;
    public WaitForSeconds thirty;
    string addString;
    int minuteCounter;

    bool hasResponded = false; //used when more than one voice command has been recognized

    // Use this for initialization
    void Start () {

        toContinue = false;
        thirty = new WaitForSeconds(30f);

        responses = new string[107];
        auSo = GetComponent<AudioSource>();
        addString = "";
        minuteCounter = 0;

        CreateString();

		//Create a new keywordRecognizer, with the words from one to one hundred
		kr = new KeywordRecognizer(responses);

		//Register OnVoiceCommand to kr's onPhraseRecognized
		kr.OnPhraseRecognized += OnVoiceCommand;
    }

    /// <summary>
    /// starts coroutine so it can be called from another class
    /// </summary>
    public void StartListening()
    {
        StartCoroutine(OnStartListening());
    }

    /// <summary>
    /// listen for ten seconds
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnStartListening()
    {
        hasResponded = false;
        //increment a counter to make sure there is always an entry into the list
        minuteCounter++;
        addString = minuteCounter + ". ";
        kr.Start();
        
        yield return thirty;
        kr.Stop();
    }

    /// <summary>
    /// handles when a keyword is recognized
    /// </summary>
    /// <param name="args"></param>
	private void OnVoiceCommand(PhraseRecognizedEventArgs args)
	{
        int ignore;
        if (int.TryParse(args.text, out ignore) == true)
        {
            if (!hasResponded)
            {
                //add the responses to the directory
                MoveOn(addString, args.text);
                hasResponded = true;
            }


        }
        else if (args.text.Equals("ice cream"))
        //keyword was "try again", so we wait for a new response
        {
            showList();
        }
        else
        {
            ds.Add(args.text);
        }
            
	}

    /// <summary>
    /// adds repsonse to directory and compares it to previous ratings to see if user is ready to move on called by OnVoiceCommand
    /// </summary>
    /// <param name="response"></param>
    private void MoveOn(string addString, string response)
    {
        //check anxiety rating... 
        //compare anxiety rating to highest datapoint that has existed
        //if the newest anxiety rating is higher, update it, if it is some % lower, consider it to have peaked
        //set toContinue to true

        ds.Add(addString + response);

        if (ds.IsPeaked(response))
        {
            toContinue = true;
        }


    }

    /// <summary>
    /// creates an array of strings that has the numbers "one" to "one-hundred"
    /// </summary>
    private void CreateString()
    {
        //keyword recognizer will break if the added array has a value that is null
        responses[0] = "Bwuh";

        for (int i = 1; i < responses.Length - 7; i++)
        {
            responses[i] = i.ToString();
        }
        //it doesn't currently deal with the "try again" situation, I guess I can just add it to the end of the oneToOnehundredArray..
        responses[responses.Length - 7] = "very real";
        responses[responses.Length - 6] = "somewhat real";
        responses[responses.Length - 5] = "slightly real";
        responses[responses.Length - 4] = "not at all real";
        responses[responses.Length - 3] = "entirely real";
        responses[responses.Length - 2] = "try again";
        responses[responses.Length - 1] = "ice cream";
    }

    /// <summary>
    /// Loads the end scene which displays the data
    /// </summary>
    public void showList()
    {
        //put responseDirectory on the screen for the psychologist to record.
        //modify a gameobject with dont destroy onload to store all data and display it in a new scene
        //load new scene
        SceneManager.LoadScene("End");
    }

}
