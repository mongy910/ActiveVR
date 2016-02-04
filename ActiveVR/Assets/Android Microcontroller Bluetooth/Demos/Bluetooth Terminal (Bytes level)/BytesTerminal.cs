using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BytesTerminal : MonoBehaviour {
	public Vector2 scrollPosition = Vector2.zero;
	bool connected = false;// equals true when connected
	
	string messageToMC = "your message to MC";// string to sent to Microcontroller
	byte [] messageFromMC ;//temporary string to hold BtConnector.read() value
	string controlData = "";//will contain data from the plugin to check the status of the whole process
	
	List<string> messages = new List<string>();// messages from Microcontroller in bytes
	
	int labelHeight;//height of a single label inside the ScrollView
	int height;//ScrollView Height
	
	void Start () {
		//use one of the following two methods to change the default bluetooth module.
			//BtConnector.moduleMAC("00:13:12:09:55:17");
			BtConnector.moduleName ("HC-06");
		height = (int)(Screen.height * 0.8f);	
		labelHeight = (int)(0.06f*height);
	}

	
	

	void Update () {


		if (BtConnector.isConnected() && BtConnector.available()) {//check connection status
						messageFromMC = BtConnector.readBuffer();//read bytes till a new line or a max of 100 bytes
						if (messageFromMC.Length > 0) {
			
				messages.Add (System.Text.Encoding.UTF8.GetString(messageFromMC));//convert array of bytes into string
								if (labelHeight * messages.Count >= (height - labelHeight))
										scrollPosition.y += labelHeight;//slide the Scrollview down,when the screen filled with messages
				
								if (labelHeight * messages.Count >= height * 2)
										messages.RemoveAt (0);//remove old messages,when ScrollView filled
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
		
		if (!connected && GUI.Button (gRect, "Connect")) //the Connect button will disappear when connecttion done
			// and appear again if it disconnected
		{
			if (!BtConnector.isBluetoothEnabled ()){
				BtConnector.askEnableBluetooth();
			} else BtConnector.connect();
		}
		
		
		connected = BtConnector.isConnected ();//check connection status
		
		
		
		
		scrollPosition = GUI.BeginScrollView (new Rect (0, height* 0.1f, Screen.width, height), scrollPosition, new Rect (0, 0, Screen.width, height * 2));
		
		for (int i = 0; i< messages.Count; i++) //display the List of messages
		{
			GUI.Label (new Rect (0,  labelHeight* i, Screen.width, labelHeight), messages[i]);
		}
		
		GUI.EndScrollView();
		
		
		
		
		messageToMC = GUI.TextField (new Rect(0, Screen.height*0.9f, Screen.width*0.9f , Screen.height*0.1f),messageToMC);
		if (GUI.Button (new Rect (Screen.width*0.9f, Screen.height*0.9f, Screen.width * 0.1f, Screen.height * 0.1f),"Send")) {
			
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes (messageToMC + '\n'.ToString());//convert string to array of bytes and add a new line
			BtConnector.sendBytes(bytes );
			
		}
		
		
		if (GUI.Button (new Rect (Screen.width*0.9f, 0, Screen.width * 0.1f, Screen.height * 0.1f),"Close Connection")) {
			BtConnector.close ();
			connected = false;
		}
		
		GUI.Label (new Rect (0, 0, Screen.width*0.9f, Screen.height * 0.1f),"From Plugin : " + controlData);
	}


}

















