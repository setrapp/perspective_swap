﻿using UnityEngine;
using System.Collections;

public class MusicVolSlider : MonoBehaviour {

	public float mouseXOrigin;
	public float percentage;

	public GameObject[] musicList;
	
	// Use this for initialization
	void Start () {
		musicList = GameObject.FindGameObjectsWithTag("Audio_Music");
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void OnMouseDown()
	{
		mouseXOrigin = Input.mousePosition.x;
	}
	
	void OnMouseUp(){
		mouseXOrigin = 0;
	}
	
	void OnMouseDrag()
	{
		//Debug.Log (transform.position.z);
		if(Input.mousePosition.x < mouseXOrigin)
		{
			//Debug.Log ("left");
			if(transform.position.z + .1f > -6)
			{
				transform.position = new Vector3(transform.position.x, transform.position.y, -6f);
				mouseXOrigin = Input.mousePosition.x;
			}else{
				transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + .1f);
				mouseXOrigin = Input.mousePosition.x;
			}
		}else if(Input.mousePosition.x > mouseXOrigin)
		{
			//Debug.Log ("right");
			if(transform.position.z - .1f < -9)
			{
				transform.position = new Vector3(transform.position.x, transform.position.y, -9f);
				mouseXOrigin = Input.mousePosition.x;
			}else{
				transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - .1f);
				mouseXOrigin = Input.mousePosition.x;
			}
		}
		percentage = Mathf.Abs(transform.position.z);
		percentage = percentage - 6;
		
		//update the volume
		for(int i = 0; i < musicList.Length; i++){
			musicList[i].GetComponent<ID_AudioMusic>().UpdateMusicVolumeLevel(percentage/3);
		}
	}

}