using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SongData", menuName = "New Song Data", order = 51)]
public class SongData : ScriptableObject
{
    public AudioClip song;
    public float bpm;
    public float startTime;
    public float speed;
}
