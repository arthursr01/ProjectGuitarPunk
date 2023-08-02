using RhythmTool.Examples;
using RhythmTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatUIScript : MonoBehaviour
{
    public RhythmAnalyzer analyzer;
    public RhythmPlayer player;
    public RhythmEventProvider eventProvider;

    public Text textBPM;

    public Line linePrefab;

    private List<Line> lines;



    private List<Chroma> chromaFeatures;

    //private Note lastNote = Note.FSHARP;

    private float cameraPositionX;
    private float cameraPositionY;

    void Awake()
    {
        analyzer.Initialized += OnInitialized;
        player.Reset += OnReset;

        eventProvider.Register<Beat>(OnBeat);


        lines = new List<Line>();

        chromaFeatures = new List<Chroma>();

    }

    void Update()
    {
        cameraPositionX = Camera.main.transform.position.x;
        cameraPositionY = Camera.main.transform.position.y;
        if (!player.isPlaying)
            return;

        UpdateLines();


    }

    private void UpdateLines()
    {
        float time = player.time;

        //Destroy all lines with a timestamp less than the current playback time.
        List<Line> toRemove = new List<Line>();
        foreach (Line line in lines)
        {
            if (line.timestamp < time || line.timestamp > time + eventProvider.offset)
            {
                Destroy(line.gameObject);
                toRemove.Add(line);
            }
        }

        foreach (Line line in toRemove)
            lines.Remove(line);

        //Update all Line positions based on their timestamp and current playback time, 
        //so they will move as the song plays.
        foreach (Line line in lines)
        {
            Vector3 position = line.transform.position;

            position.x = line.timestamp - time + cameraPositionX - 8;
            position.y = cameraPositionY + 4;

            line.transform.position = position;
        }
    }

    private void OnInitialized(RhythmData rhythmData)
    {
        //Start playing the song.
        player.Play();
    }

    private void OnReset()
    {
        //Destroy all lines when playback is reset.
        foreach (Line line in lines)
            Destroy(line.gameObject);

        lines.Clear();
    }

    private void OnBeat(Beat beat)
    {
        //Instantiate a line to represent the Beat.
        CreateLine(beat.timestamp, 0, 1, Color.black, 1);

        //Update BPM text.
        float bpm = Mathf.Round(beat.bpm * 10) / 10;
        textBPM.text = "(" + bpm + " BPM)";
    }



    private void CreateLine(float timestamp, float position, float scale, Color color, float opacity)
    {
        Line line = Instantiate(linePrefab);

        line.transform.position = new Vector2(0, position);
        line.transform.localScale = new Vector2(.1f, scale);

        line.Init(color, opacity, timestamp);

        lines.Add(line);
    }
}

