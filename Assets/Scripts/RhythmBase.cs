using RhythmTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmBase : MonoBehaviour
{

    public RhythmEventProvider eventProvider;

    public bool isOnBeat = false;
    // Start is called before the first frame update
    void Start()
    {
        eventProvider.Register<Beat>(OnBeat);
    }

    // Update is called once per frame
    void OnBeat(Beat beat)
    {
        
       // Debug.Log("A beat occurred at " + beat.timestamp);
        StartCoroutine(SetBeatBack());
    
    }

    public IEnumerator SetBeatBack()
    {
        isOnBeat = true;
        yield return new WaitForSeconds(0.3f); // waits 3 seconds
        isOnBeat = false; // will make the update method pick up 

    }
}
