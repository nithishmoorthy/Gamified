using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public Transform player;
    public GameObject trackPrefab;

    private GameObject firstTrack;
    private GameObject secondTrack;

    private float trackLength = 1000f;

    void Start()
    {
        firstTrack = GameObject.Find("Track1");
        secondTrack = GameObject.Find("Track2");
    }

    void Update()
    {
        if ((player.position.z + 490) > firstTrack.transform.position.z + trackLength)
        {
            Destroy(firstTrack, 2f);

            firstTrack = secondTrack;

            secondTrack = Instantiate(
                trackPrefab,
                new Vector3(
                    0,
                    0,
                    firstTrack.transform.position.z + trackLength
                ),
                Quaternion.identity
            );
        }
    }
}