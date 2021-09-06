using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManagerDebug : MonoBehaviour
{
    public Text text;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(audioSource.clip != null
            && audioSource.clip.name != text.text)
        {
            text.text = audioSource.clip.name;
        }
    }
}
