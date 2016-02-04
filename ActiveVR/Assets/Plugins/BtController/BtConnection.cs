
using UnityEngine;

public   class  BtConnection : MonoBehaviour  {

	private  const string CONNECTED = "Connected";
	private  const string DISCONNECTED = "Disconnected";
	private  const string UNABLE_TO_CONNECT = "found your Bluetooth Module but unable to connect to it";
	private  const string NOT_FOUND = "Bluetooth module with the name or the MAC you provided can't be found";
	private  const string MODULE_OFF = "Connection Failed, usually because your Bluetooth module is off ";
	private  const string CLOSING_ERROR = "error while closing";
	private  const string WRITING_ERROR = "error while writing";
	private  const string READING_ERROR = "error while reading";
	
	private const string NOT_INITIALIZED = "Plugin is not initialized";
	private const string DEFAULT = "Starting...";


	private static readonly AndroidJavaObject ajc ;
	private static readonly bool PluginReady ;
	private const string BridgePackage = "com.badran.bluetoothcontroller.Bridge";

	static BtConnection(){

		ajc = null;
		PluginReady = false;
	

		#if !UNITY_EDITOR
		if (Application.platform == RuntimePlatform.Android) {
			try {
				ajc = new AndroidJavaObject(BridgePackage);
					if (!IsAndroidJavaObjectNull(ajc)) {
						
						PluginReady = true;
					} 

			} catch {
				Debug.LogError("Bluetooth initialization failed. Probably .jar not present");

			}
		}
				
	
		#endif
	
				
}
	

	private static bool IsAndroidJavaClassNull(AndroidJavaClass androidJavaClass) {
		return androidJavaClass == null || 
			androidJavaClass.GetRawClass().ToInt32() == 0;
	}	

	private static bool IsAndroidJavaObjectNull(AndroidJavaObject androidJavaObject) {
		return androidJavaObject == null || 
			androidJavaObject.GetRawClass().ToInt32() == 0;
	}	



	//public GameObject = GameObject.
	public static void askEnableBluetooth(){
		if (!PluginReady)
						return;
		ajc.CallStatic ("askEnableBluetooth");
		}


	public static int connect(){
		if (!PluginReady)
			return -2;
				return ajc.CallStatic<int> ("connect",100);
		}
	public static int connect(int tries){
		if (!PluginReady)
			return -2;
		return ajc.CallStatic<int> ("connect",tries);
	}
	public static bool test () {
		if (!PluginReady)
						return false;
		return ajc.CallStatic<bool> ("TESTING");
		}
	//close connection
	public static void close(){
		if (!PluginReady)
			return ;
				ajc.CallStatic  ("close");
		}
	//returns true if data there's a data to read
	public static bool available (){
		if (!PluginReady)
			return false;
		return ajc.CallStatic <bool> ("available");
		}


	//read from Microcontroller
	public static string read(){
		if (!PluginReady)
						return "";
		return ajc.CallStatic<string> ("read");
		}
	//read Control data, for testing
	public static int controlData(){
		if (!PluginReady)
			return -7;
		return ajc.CallStatic<int>("controlData");
		}
	//plugin method, should not be used in your game
	public static byte [] readBuffer(){
		if (!PluginReady)
			return new byte[] {};
		return ajc.CallStatic<byte []>("readBuffer");
	}
	public static byte [] readBuffer(int length){
		if (!PluginReady)
			return new byte[] {};
		return ajc.CallStatic<byte []>("readBuffer",length);
		}
	public static byte [] readBuffer(int length,byte stopByte){
		if (!PluginReady)
			return new byte[] {};
		return ajc.CallStatic<byte []>("readBuffer",length,stopByte);
		}
	
	public static void sendBytes(byte [] message){
		if (!PluginReady)
			return ;
		ajc.CallStatic ("sendBytes", message);
		}

	//send string
	public static void sendString(string message){
		if (!PluginReady)
			return ;
		ajc.CallStatic("sendString",message);
		}
	//send 1 char
	public static void sendChar(char message){
		if (!PluginReady)
			return ;
		ajc.CallStatic ("sendChar", message);
		}
	//change the default Bluetooth Module name 
	public static void moduleName(string name){
		if (!PluginReady)
			return ;
		ajc.CallStatic ("moduleName", name);
		}

	public static void listen(bool start){
		if (!PluginReady)
			return ;
		ajc.CallStatic ("listen", start);
		modeNumber = 0;
		}
	public static void listen(bool start,int length,bool byteLimit){
		if (!PluginReady)
			return ;
		ajc.CallStatic ("listen", start,length,byteLimit);
		modeNumber = 1;
		}
	public static void listen(bool start,int length,byte terminalByte){
		if (!PluginReady)
			return ;
		ajc.CallStatic ("listen", start,length,terminalByte);
		modeNumber = 2;
	}
	public static void stopListen(){
		if (!PluginReady)
			return ;
		ajc.CallStatic ("stopListen");
		}
	public static void moduleMAC(string name){
		if (!PluginReady)
			return ;//Plugin isn't ready
		ajc.CallStatic ("moduleMac", name);
		}

	public static bool isConnected (){
		if (!PluginReady)
			return false;
		return ajc.CallStatic<bool> ("isConnected");
		}
	public static bool isSending (){
		if (!PluginReady)
			return false;
		return ajc.CallStatic<bool>("isSending");
		}

	public static bool enableBluetooth(){
		if (!PluginReady)
			return false;
		return ajc.CallStatic<bool>("enableBluetooth");
		}
	public static bool disableBluetooth(){
		if (!PluginReady)
			return false;
		return ajc.CallStatic<bool>("disableBluetooth");
	}

	public static bool isBluetoothEnabled() {
		if (!PluginReady)
			return false;
		return ajc.CallStatic<bool>("isBluetoothEnabled");
		}

	public static string readControlData(){
		
		switch(BtConnection.controlData()){
		case 1 : return CONNECTED; 
		case 2 : return DISCONNECTED; 
		case -1 : return UNABLE_TO_CONNECT;
		case -2 : return NOT_FOUND;
		case -3 : return MODULE_OFF;
		case -4 : return CLOSING_ERROR;
		case -5 : return WRITING_ERROR;
		case -6 : return READING_ERROR;
		case -7 : return NOT_INITIALIZED;
		default : return DEFAULT;


		}






	}
	// device picker

	public static AndroidJavaObject getPickedDevice (){
		if (!PluginReady)
			return null; // not ready
		return ajc.CallStatic<AndroidJavaObject>("getPickedDevice");
	}

	public static string BluetoothDeviceName (AndroidJavaObject j){
		if (!PluginReady)
			return null; // not ready
		return ajc.CallStatic<string>("BluetoothDeviceName",j);
	}


	public static string BluetoothDeviceMac (AndroidJavaObject j){
		if (!PluginReady)
			return null; // not ready
		return ajc.CallStatic<string>("BluetoothDeviceMac",j);
	}
	public static bool setPickedDevice (AndroidJavaObject j){
		if (!PluginReady)
			return false; // not ready
		return ajc.CallStatic<bool>("setBluetoothDevice",j);
	}

	public static void showDevices (){
		if (!PluginReady)
			return ; // not ready
		 ajc.CallStatic("showDevices");
	}
	////////////////BtConnector
	/// 
	/// 
	private static int modeNumber = 0;
	public static void doneReading() {
		ajc.CallStatic("doneReading");
	}
	public static int mode(){
		return modeNumber;
	}




}
