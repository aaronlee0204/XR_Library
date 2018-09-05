using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class MotionAI : MonoBehaviour
{
    [Serializable]
    public class Location
    {
        public Vector3 pos;
        public Quaternion rot;
        public Vector3 scale;
        public float elapsed;
    }

    [System.Serializable]
    public class LocationList
    {
        public List<Location> locations = new List<Location>();
    }

    public string log_file; // Name of the file to be saved in the streaming folder.
    public bool play_position = true;
    public bool play_rotation = true;
    public bool play_scale = false;

    LocationList trail;

    int curr;
    float elapsed = 0f;
    float duration = 0f;

    void NextPath()
    {
        if (trail.locations.Count > curr)
        {
            if ( play_position )
                transform.localPosition = trail.locations[curr].pos;
            if ( play_rotation )
                transform.localRotation = trail.locations[curr].rot;
            if ( play_scale )
                transform.localScale = trail.locations[curr].scale;

            duration = trail.locations[curr].elapsed - elapsed;
            curr++;
        } else
        {
            this.enabled = false;
        }
    }

    public LocationList Load(string log_file)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, log_file);
        Debug.Log("Loading JSON Data from:" + filePath);

        if (File.Exists(filePath))
        {
            // read existing json
            string jsonData = File.ReadAllText(filePath);
            return JsonUtility.FromJson<LocationList>(jsonData);
        }
        else
        {
            // create a new Gesture data struction and json file
            Debug.LogWarning("No LocationList found");
            return null;
        }
    }

    public void Save()
    {
        foreach( Location loc in trail.locations )
        {
            loc.pos = transform.InverseTransformPoint(loc.pos);
            loc.rot = Quaternion.Inverse(transform.localRotation) * loc.rot;
            loc.scale = new Vector3(
                loc.scale.x / transform.localScale.x,
                loc.scale.y / transform.localScale.y, 
                loc.scale.z / transform.localScale.z);
        }

        // Save JSON and finish.
        string filePath = Path.Combine(Application.streamingAssetsPath, "Slider_local");
        Debug.Log("Saving:" + filePath);

        string jsonData = JsonUtility.ToJson(trail);
        File.WriteAllText(filePath, jsonData);
    }

    private void Awake()
    {
        trail = Load(log_file);
        if (trail == null)
            this.enabled = false;

        //Save();

        NextPath();
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
        duration -= Time.deltaTime;
        if (duration <= 0f )
            NextPath();
    }
}
