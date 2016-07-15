using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour {
    public GameObject thingWithTapScript;
    public GameObject beeScript;
    public GameObject openBoxAnimation;

    AudioSource anxietyRating;
    AudioSource stimulusRating;

    AudioSource[] aSources;

    public bool aIsPlaying, sIsPlaying = false;

    DateTime oldDate;
    DateTime currentDate;
    public bool setOldDate = true;
    int minutes;
    int currentMin;
    bool hasPlayedOnce = false;

    public int level = 0;

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
                    handleLevels();
                    break;
                case 2:
                    handleLevels();
                    break;
                case 3:
                    handleLevels();
                    break;



            }
        }
        //update minutes
        currentDate = System.DateTime.Now;
        //number of minutes since box was created
        minutes = currentDate.Minute - oldDate.Minute;

    }


    public void handleLevels()
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
                break;
            case 4:
                handleMinutes();
                break;
            case 5:
                handleMinutes();
                //Turn off the box and prepare for the next stage
                TurnOffBox();
                break;
        }



        //
        //
        //
        //Add switch statement for different animal types - mouse, bee, spider
        //
        //
        //
        //
        if (level == 1)
        {
        }
        else if (level == 2)
        {
            openBoxAnimation.GetComponent<Animator>().SetTrigger("Open");
            beeScript.GetComponent<MouseAI>().cWaypoint = 0;
            beeScript.GetComponent<MouseAI>().currentWaypoints = beeScript.GetComponent<MouseAI>().waypointsIntermediate;
            beeScript.GetComponent<MouseAI>().currentWaypoints[beeScript.GetComponent<MouseAI>().cWaypoint].gameObject.tag = "intermediate current";
            beeScript.GetComponent<MouseAI>().diff = LevelState.State.INTERMEDIATE;
            beeScript.GetComponent<MouseAI>().firstCollision = true;
        }
        else if (level == 3)
        {
            beeScript.GetComponent<MouseAI>().cWaypoint = 0;
            beeScript.GetComponent<MouseAI>().currentWaypoints = beeScript.GetComponent<MouseAI>().waypointsAdvanced;
            beeScript.GetComponent<MouseAI>().currentWaypoints[beeScript.GetComponent<MouseAI>().cWaypoint].gameObject.tag = "advanced current";
            beeScript.GetComponent<MouseAI>().diff = LevelState.State.ADVANCED;
            beeScript.GetComponent<MouseAI>().firstCollision = true;
        }


        //if (level == 1)
        //{
        //}
        //else if (level == 2)
        //{
        //    openBoxAnimation.GetComponent<Animator>().SetTrigger("Open");
        //    beeScript.GetComponent<beeAI>().cWaypoint = 0;
        //    beeScript.GetComponent<beeAI>().currentWaypoints = beeScript.GetComponent<beeAI>().waypointsIntermediate;
        //    beeScript.GetComponent<beeAI>().currentWaypoints[beeScript.GetComponent<beeAI>().cWaypoint].gameObject.tag = "intermediate current";
        //    beeScript.GetComponent<beeAI>().diff = LevelState.State.INTERMEDIATE;
        //    beeScript.GetComponent<beeAI>().firstCollision = true;
        //}
        //else if (level == 3)
        //{
        //    beeScript.GetComponent<beeAI>().cWaypoint = 0;
        //    beeScript.GetComponent<beeAI>().currentWaypoints = beeScript.GetComponent<beeAI>().waypointsAdvanced;
        //    beeScript.GetComponent<beeAI>().currentWaypoints[beeScript.GetComponent<beeAI>().cWaypoint].gameObject.tag = "advanced current";
        //    beeScript.GetComponent<beeAI>().diff = LevelState.State.ADVANCED;
        //    beeScript.GetComponent<beeAI>().firstCollision = true;
        //}



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
