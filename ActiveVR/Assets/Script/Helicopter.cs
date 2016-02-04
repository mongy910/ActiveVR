using UnityEngine;
using System.Collections;

public class Helicopter : MonoBehaviour {
	private bool called = false;
	public AudioClip callSound;
	private AudioSource audioSource;
	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton ("CallHeli") && ! called) {
			called = true;
			audioSource.clip = callSound;
			Debug.Log ("helicopter called");
			audioSource.Play();

		} else {

		}
	
	}
}
