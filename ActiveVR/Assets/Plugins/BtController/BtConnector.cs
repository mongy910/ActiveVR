using UnityEngine;
using System.Collections;

public class BtConnector : MonoBehaviour {
	private static string message ;
	private static byte [] byteMessage;
	[SerializeField]
	public bool stopReading;
	[SerializeField]
	public  bool mode0  ;
	[SerializeField]
	public  bool mode2 ;
	[SerializeField]
	public  bool mode3 ;
	[SerializeField]
	public  int length ;
	[SerializeField]
	[Range(0, 255)]
	public  byte terminalByte ;

	private static bool dataAvailable = false;
	private static bool connected = false;





	void Start () {


        
		if (mode2 ) {
						BtConnection.listen (true, length, terminalByte);
				} else if (!mode2 && !mode0 && !stopReading && !mode3) {
						BtConnection.listen (true, length,false);
				} else if (stopReading )
						BtConnection.stopListen ();
				else if ( mode0 ) {
						BtConnection.listen (true);
				}else if(mode3) BtConnection.listen (true, length,true);
						
		}
	//plugin methods
	void receiver (string s) {

		message = s;
		dataAvailable = true;

	}

	//plugin methods
	void startReading (string s) {
		

		byteMessage = BtConnection.readBuffer();

		dataAvailable = true;
		
	}

	void connection (string s){
		if(s.Equals("1"))
		connected = true;
		else connected = false;
		}

	public static bool isDevicePicked = false;
	void devicePicked (string s){
		BtConnector.isDevicePicked = true;

		}

	static public   string getPickedDeviceName(){
		if (!BtConnector.isDevicePicked)
						return "no device";
		else

				return BtConnection.BluetoothDeviceName(BtConnection.getPickedDevice());
		}

	static public void showDevices(){
		BtConnection.showDevices ();
		}

	//it will read as specified in the inspector
	public static string readLine  () {

		if (BtConnection.mode () != 0)
						BtConnection.listen (true);

						if (dataAvailable) {
							string tempMessage = message;
								dataAvailable = false;
								
								BtConnection.doneReading ();
								
							return  tempMessage;
						} else return "";

				
						
				
	}

	//it will read as specified in the inspector
	public static byte [] readBuffer(){

						if (dataAvailable) {
								dataAvailable = false;
								
								
								return byteMessage;

						} else
					return new byte[] {};
			

				}
		


	//check if there's data to read
	public static bool available(){
		return dataAvailable; 

		}

	//check connection
	public static bool isConnected(){
		return connected;
		}

	/////////////////// all the next methods are just calling the same BtConnection methods
	
	//private static AndroidJavaClass ajc = new AndroidJavaClass ("com.badran.bluetoothcontroller.Bridge") ;
	//public GameObject = GameObject.
	public static void askEnableBluetooth(){
		 BtConnection.askEnableBluetooth ();
	}
	public static int connect(){
		return BtConnection.connect ();
	}

	public static bool autoConnect(float time){
		int tmp = -3;


		while (!BtConnector.isConnected() && time > 0){
			time -= Time.deltaTime;
			if(tmp == -3 ) {
				BtConnector.connect();
			}
			
			 tmp = BtConnector.controlData();
			
		}
		return BtConnector.isConnected();

	}
	public static bool test () {
		return BtConnection.test ();
	}
	//close connection
	public static void close(){
		BtConnection.close ();
	}
	//returns true if data there's a data to read

	
	
	//read from Microcontroller
	public static string read(){
		return BtConnection.read ();
	}
	//read Control data, for testing
	public static int controlData(){
		return BtConnection.controlData ();
	}
	
	public static byte [] readBuffer(int length){
		if(BtConnection.mode() != 1)
		BtConnection.listen (true, length,false);

		return BtConnector.readBuffer ();
	}
	public static byte [] readBuffer(int length,byte stopByte){
		if (BtConnection.mode() !=2)
			BtConnection.listen (true, length,stopByte);

		return BtConnection.readBuffer (length, stopByte);
	}
	
	public static void sendBytes(byte [] message){
		BtConnection.sendBytes (message);
	}
	
	//send string
	public static void sendString(string message){
		BtConnection.sendString (message);
	}
	//send 1 char
	public static void sendChar(char message){
		BtConnection.sendChar (message);
	}
	//change the default Bluetooth Module name 
	public static void moduleName(string name){
		BtConnection.moduleName (name);
	}
	
	public static void listen(bool start){
		BtConnection.listen (start);
	}
	public static void listen(bool start,int length,bool byteLimit){
		BtConnection.listen (start, length,byteLimit);
	}

	public static void listen(bool start,int length){
		BtConnection.listen (start, length,false);
	}
	public static void listen(bool start,int length,byte terminalByte){
		BtConnection.listen (start, length, terminalByte);
	}
	public static void stopListen(){
		BtConnection.stopListen ();
	}
	public static void moduleMAC(string name){
		BtConnection.moduleMAC (name);
	}
	

	public static bool isSending (){
		return BtConnection.isSending ();
	}
	
	public static bool enableBluetooth(){
		return BtConnection.enableBluetooth ();
	}
	public static bool disableBluetooth(){
		return BtConnection.disableBluetooth ();
	}
	
	
	public static bool isBluetoothEnabled() {
		return BtConnection.isBluetoothEnabled ();
	}
	
	public static string readControlData(){
		
		return BtConnection.readControlData ();


	}
	

}
