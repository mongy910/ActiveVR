using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Terminal : MonoBehaviour {

	Vector2 scrollPosition = Vector2.zero;

	string messageToMC = "your message to MC";// string to sent to Microcontroller
	string messageFromMC ;//temporary string to hold BtConnector.read() value
	string controlData = "";//will contain data from the plugin to check the status of the whole process

	List<string> messages = new List<string>();// messages from Microcontroller

	int labelHeight;//height of a single label inside the ScrollView
	int height;//ScrollView Height


	void Start () {
		///use one of the following two methods to change the default bluetooth module.
		//BtConnector.moduleMAC("00:13:12:09:55:17");
		BtConnector.moduleName ("HC-06");
		height = (int)(Screen.height * 0.8f);	
		labelHeight = (int)(0.06f*height);

	}
	


	void Update () {

	
		if (BtConnector.isConnected ()) {
						if (BtConnector.available()) {//check if there's an available data

						messageFromMC = BtConnector.readLine ();//hold the data in messageFromMC
						//cause BtConnector.read () will delete the buffer
						
								if (messageFromMC.Length > 0) {// recheck if there's an available data
								//this check is more important than BtConnector.available ()
								//actually you could delete BtConnector.available () with no effect.

										messages.Add (messageFromMC);//add the string to the list
										
										if (labelHeight * messages.Count >= (height - labelHeight))
												scrollPosition.y += labelHeight;//it will slide the ScrollView down
												//if the whole screen filled with messages
				
										if (labelHeight * messages.Count >= height * 2)
												messages.RemoveAt (0);//when the ScrollView filled ,delet old messages
																		
												
								}
						}
				}
			
			//read control data from the Module.
		controlData = BtConnector.readControlData ();
				
	}



	 


	// GUI  // GUI // GUI

	Rect gRect = new Rect (Screen.width * 0.3f, Screen.height * 0.3f, Screen.width * 0.3f, Screen.height * 0.3f);

	void OnGUI(){

		GUIStyle style = GUI.skin.GetStyle ("label");
		style.fontSize = 20;

		if (!BtConnector.isConnected () && GUI.Button (gRect, "Connect")) //the Connect button will disappear when connecttion done
														// and appear again if it disconnected
		{
				if (!BtConnector.isBluetoothEnabled ()){
					BtConnector.askEnableBluetooth();
			} else BtConnector.connect();
		}




		scrollPosition = GUI.BeginScrollView (new Rect (0, height* 0.1f, Screen.width, height), scrollPosition, new Rect (0, 0, Screen.width, height * 2));

		for (int i = 0; i< messages.Count; i++) //display the List of messages
		{
			GUI.Label (new Rect (0,  labelHeight* i, Screen.width, labelHeight), messages[i]);
		}

		GUI.EndScrollView();


	

		messageToMC = GUI.TextField (new Rect(0, Screen.height*0.9f, Screen.width*0.9f , Screen.height*0.1f),messageToMC);
		if (GUI.Button (new Rect (Screen.width*0.9f, Screen.height*0.9f, Screen.width * 0.1f, Screen.height * 0.1f),"Send")) {
			BtConnector.sendString(messageToMC);

		}


		if (GUI.Button (new Rect (Screen.width*0.9f, 0, Screen.width * 0.1f, Screen.height * 0.1f),"Close Connection")) {
			BtConnector.close ();

		}

		GUI.Label (new Rect (0, 0, Screen.width*0.9f, Screen.height * 0.1f),"From Plugin : " + controlData);
	}


}











