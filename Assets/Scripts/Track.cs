using UnityEngine;

public class Track : MonoBehaviour
{
    public SongData song;
    public AudioSource audioSource;

    private void Start()
    {
        transform.position = Vector3.forward * GameManager.instance.songStartTime * song.speed;
        Invoke("StartSong", GameManager.instance.songStartTime - song.startTime);
    }

    void StartSong()
    {
        audioSource.PlayOneShot(song.song);
    }

    void Update()
    {
        transform.position += Vector3.back * song.speed * Time.deltaTime;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        float beatLength = 60f / (float)song.bpm;
        float beatDist = beatLength * song.speed;

        for (float i = 0; i < 100; i ++)
        {
            Gizmos.DrawLine(transform.position + new Vector3(-1, 0, i * beatDist), transform.position + new Vector3(1, 0, i * beatDist));    
        }

    }
}
