using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour {
    //object that has tap to place parent script
    public GameObject thingWithTapScript;
    //bee mouse or spider object used for typeOfAnimal enum
    public GameObject scriptType;
    //used to tell the oben box animation to play
    public GameObject openBoxAnimation;

    //the anxiety and stimulus sound bits
    AudioSource anxietyRating;
    AudioSource stimulusRating;

    AudioSource[] aSources;

    public bool aIsPlaying, sIsPlaying = false;

    //time variables to play messages
    DateTime oldDate;
    DateTime currentDate;
    public bool setOldDate = true;
    int minutes;
    int currentMin;
    bool hasPlayedOnce = false;

    public int level = 0;

    public enum typeOfAnimal {ERROR = 0, BEE, MOUSE, SPIDER};
    public typeOfAnimal tyOfAn;

    // Use this for initialization
    void Start () {
        //gets the audio sources
        aSources = GetComponents<AudioSource>();

        //sets the audio sources
        anxietyRating = aSources[0];
        stimulusRating = aSources[1];

        PlayAnxietyRating();

        //one more to add for the "try again" recording
    }
	
	
	// Update is called once per frame
	void Update ()
    {

        //if there's no rating message playing
        if (!anxietyRating.isPlaying && !stimulusRating.isPlaying)
        {
            //controls what we do each level
            switch (level)
            {
                case 0:
                    //if we've created the box
                    if (setOldDate == true && thingWithTapScript.GetComponent<TapToPlaceParent>().firstPass == false)
                    {
                        //start calulating time since this moment
                        oldDate = System.DateTime.Now;
                        //move on
                        level++;
                    }
                    break;
                case 1:
                    handleLevels(tyOfAn);
                    break;
                case 2:
                    handleLevels(tyOfAn);
                    break;
                case 3:
                    handleLevels(tyOfAn);
                    break;



            }
        }
        //update minutes
        currentDate = System.DateTime.Now;
        //number of minutes since box was created
        minutes = currentDate.Minute - oldDate.Minute;

    }


    public void handleLevels(typeOfAnimal toa)
    {
        //each time the minute increments we ask for the subejects anxiety rating one time, for a total of 5 minutes
        switch (minutes)
        {
            case 1:               
                handleMinutes();
                break;
            case 2:
                handleMinutes();
                break;
            case 3:
                handleMinutes();
                //Turn off the box and prepare for the next stage
                TurnOffBox();
                break;
        }
        switch (toa)
        {
            case typeOfAnimal.ERROR:
                Debug.Log("There is a problem with typeOfAnimal");
                break;
            case typeOfAnimal.BEE:
                BeeAI();
                break;
            case typeOfAnimal.MOUSE:
                GroundAI();
                break;
            case typeOfAnimal.SPIDER:
                GroundAI();
                break;
        }

    }
    void BeeAI()
    {
        if (level == 1)
        {
        }
        else if (level == 2)
        {
            openBoxAnimation.GetComponent<Animator>().SetTrigger("Open");
            scriptType.GetComponent<beeAI>().cWaypoint = 0;
            scriptType.GetComponent<beeAI>().currentWaypoints = scriptType.GetComponent<beeAI>().waypointsIntermediate;
            scriptType.GetComponent<beeAI>().currentWaypoints[scriptType.GetComponent<beeAI>().cWaypoint].gameObject.tag = "intermediate current";
            scriptType.GetComponent<beeAI>().diff = LevelState.State.INTERMEDIATE;
            scriptType.GetComponent<beeAI>().firstCollision = true;
        }
        else if (level == 3)
        {
            scriptType.GetComponent<beeAI>().cWaypoint = 0;
            scriptType.GetComponent<beeAI>().currentWaypoints = scriptType.GetComponent<beeAI>().waypointsAdvanced;
            scriptType.GetComponent<beeAI>().currentWaypoints[scriptType.GetComponent<beeAI>().cWaypoint].gameObject.tag = "advanced current";
            scriptType.GetComponent<beeAI>().diff = LevelState.State.ADVANCED;
            scriptType.GetComponent<beeAI>().firstCollision = true;
        }
    }
    void GroundAI()
    {
        if (level == 2)
            openBoxAnimation.GetComponent<Animator>().SetTrigger("Open");
        else if (level == 3)
            scriptType.transform.localPosition = new Vector3(0, 0, -1.5f);
    }

    //if recording has not yet been played this minute, play it
    void handleMinutes()
    {
        //if the recording has been played and the previous minute is not the current minute
        if (hasPlayedOnce && currentMin != minutes)
        {
            hasPlayedOnce = false;
        }
        //if the anxiety rating has not been played this minute
        if (!hasPlayedOnce)
        {

            //play it
            PlayAnxietyRating();
            //it has now been played
            hasPlayedOnce = true;
            //record what minute it was played
            currentMin = minutes;
        }

    }


    void TurnOffBox()
    {
        //if the anxiety rating message is done playing
        if (!anxietyRating.isPlaying && !stimulusRating.isPlaying)
        {
            //allow us to go to the next level
            level++;
            //reset the minute counter so this is not called more than once
            oldDate = System.DateTime.Now;
            //play the stimulus rating message
            PlayStimulusRating();

            //turn off the box
            ////
            //
            //
            //
            //
            //
        }

    }


    //plays the anxiety rating message
    public void PlayAnxietyRating()
    {
        if (!anxietyRating.isPlaying && !stimulusRating.isPlaying)
            anxietyRating.Play();
    }
    //plays the stimulus rating message
    public void PlayStimulusRating()
    {
        if (!anxietyRating.isPlaying && !stimulusRating.isPlaying)
            stimulusRating.Play();
    }

}
