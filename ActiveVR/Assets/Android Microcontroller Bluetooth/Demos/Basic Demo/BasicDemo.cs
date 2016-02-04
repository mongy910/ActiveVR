using UnityEngine;
using System.Collections;


public class BasicDemo : MonoBehaviour {

	string fromArduino = "" ;
	string stringToEdit = "HC-05";


	void Start () {
		//use one of the following two methods to change the default bluetooth module.
		//BtConnector.moduleMAC("00:13:12:09:55:17");
		//BtConnector.moduleName ("HC-05");
	}

	void OnGUI(){
		GUI.Label(new Rect(0, 0, Screen.width*0.15f, Screen.height*0.1f),"Module Name ");

		stringToEdit = GUI.TextField(new Rect(Screen.width*0.15f, 0, Screen.width*0.8f, Screen.height*0.1f), stringToEdit);
		GUI.Label(new Rect(0,Screen.height*0.2f,Screen.width,Screen.height*0.1f),"Arduino Says : " +  fromArduino);
		GUI.Label(new Rect(0,Screen.height*0.3f,Screen.width,Screen.height*0.1f),"from PlugIn : " + BtConnector.readControlData ());

		if(GUI.Button(new Rect(0,Screen.height*0.4f,Screen.width,Screen.height*0.1f), "Connect")) 
		{

				if (!BtConnector.isBluetoothEnabled ()){
					BtConnector.askEnableBluetooth();
				} else{

					BtConnector.moduleName(stringToEdit); //incase User Changed the Bluetooth Name
					BtConnector.connect();
				}
		}

	
		///the hidden code here let you connect directly without askin the user
		/// if you want to use it, make sure to hide the code from line 23 to lin 33
		/*
		if( GUILayout.Button ("Connect")){ 
			
			startConnection = true; 
			
		} 
		
		if(GUI.Button(new Rect(0,Screen.height*0.4f,Screen.width,Screen.height*0.1f), "Connect")) 
		{
			if (!BtConnector.isBluetoothEnabled ()){ 
				BtConnector.enableBluetooth(); 
				
			} else {  
				
				BtConnector.connect(); 
				
				startConnection = false; 
				
			} 
			
		} 
		*/
		/////////////
		if(GUI.Button(new Rect(0,Screen.height*0.6f,Screen.width,Screen.height*0.1f), "sendChar")) {
			 if(BtConnector.isConnected()){
				BtConnector.sendChar('h');
				BtConnector.sendChar('e');
				BtConnector.sendChar('l');
				BtConnector.sendChar('l');
				BtConnector.sendChar('o');
				BtConnector.sendChar ('\n');//because we are going to read it using .readLine() which reads lines.

				//don't call the send method multiple times unless you really need to, because it will kill performance.
		
			}
				
		}
		if(GUI.Button(new Rect(0,Screen.height*0.5f,Screen.width,Screen.height*0.1f), "sendString")) {
			
			if(BtConnector.isConnected()){
				BtConnector.sendString("Hii");
				BtConnector.sendString("you can do this");
				//BtConnector.sendBytes(new byte[] {55,55,55,10});

				//don't call the send method multiple times unless you really need to, because it will kill performance.
			}
		}


	
		if(GUI.Button(new Rect(0,Screen.height*0.7f,Screen.width,Screen.height*0.1f), "Close")) {
			BtConnector.close();
		}

		if(GUI.Button(new Rect(0,Screen.height*0.8f,Screen.width,Screen.height*0.1f), "readData")) {
			if (BtConnector.available()) {
			fromArduino =  BtConnector.readLine ();
			}

		}
		if (GUI.Button (new Rect (0, Screen.height * 0.9f, Screen.width, Screen.height * 0.1f), "change ModuleName")) {
			BtConnector.moduleName(stringToEdit);

		}




	}
}
