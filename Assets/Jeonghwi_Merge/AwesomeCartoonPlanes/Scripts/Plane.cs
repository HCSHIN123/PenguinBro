using UnityEngine;

public class Plane : MonoBehaviour
{
    public GameObject prop;
    public GameObject propBlured;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private AudioSource planeSound;
    private float maxVolumeDistance = 1000f;
    private float minVolumeDistance = 980f;

    public bool engineOn;

    private void Start()
    {
        AudioClip liftCraftSoundClip = Resources.Load<AudioClip>("LiftCraftSound");
        if (liftCraftSoundClip != null)
        {
            planeSound.clip = liftCraftSoundClip;
            planeSound.loop = true;
            planeSound.playOnAwake = false;
            planeSound.spatialBlend = 1.0f;
            planeSound.minDistance = minVolumeDistance;
            planeSound.maxDistance = maxVolumeDistance;
        }
        else
        {
            Debug.LogError("LiftCraftSound not found in Resources folder.");
        }
    }

    private void Update()
    {
        if (engineOn)
        {
            prop.SetActive(false);
            propBlured.SetActive(true);
            propBlured.transform.Rotate(1000 * Time.deltaTime, 0, 0);
        }
        else
        {
            prop.SetActive(true);
            propBlured.SetActive(false);
        }

        CheckSoundRange();
    }

    private void CheckSoundRange()
    {
        if (planeSound == null)
        {
            return;
        }

        float distanceToCamera = Vector3.Distance(transform.position, playerCamera.transform.position);

        if (distanceToCamera <= maxVolumeDistance)
        {
            if (!planeSound.isPlaying)
            {
                planeSound.Play();
            }

            if (distanceToCamera <= minVolumeDistance)
            {
                planeSound.volume = 1.0f;
            }
            else
            {
                float volume = 1.0f - ((distanceToCamera - minVolumeDistance) / (maxVolumeDistance - minVolumeDistance));
                planeSound.volume = Mathf.Clamp(volume, 0.0f, 1.0f);
            }
        }
        else
        {
            if (planeSound.isPlaying)
            {
                planeSound.Stop();
            }
        }
    }
}
