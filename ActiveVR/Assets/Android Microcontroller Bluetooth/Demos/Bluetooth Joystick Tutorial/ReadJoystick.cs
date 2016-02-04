using UnityEngine;
using System.Collections;

public class ReadJoystick : MonoBehaviour {

	int[] joystickxy = new int[2];
	Vector2 curAccess;

	string BT;
	
	void start () {
		
		joystickxy[0] = 4;
		
		joystickxy[1] = 4;
		
	}
	
	void update() {
		
		string temp = BtConnector.readLine();
		
		if( temp.Length > 0)
			
			BT = temp;
		
		if ( BT.Length >= 2 ){
			
			joystickxy[0] = BT[0] - 48; // -48 used to convert char to int
			
			joystickxy[1] = BT[1] - 48;
			
		} // do whatever you want with these data

		//// now lets try to convert these values into something 
		/// similar to Input.GetAxis()
		Vector2 tempVector = new Vector2 (joystickxy [0], joystickxy [1]);
		curAccess = Vector3.Lerp (curAccess, tempVector - new Vector2(4,4), Time.deltaTime / 0.5f);
		
		
		float VerticalAxis = Mathf.Clamp (curAccess.y , -1, 1);
		float HorizantalAxis = Mathf.Clamp (curAccess.x , -1, 1);


		Debug.Log (" Vertical Axis : " + VerticalAxis);
		Debug.Log (" Horizantal  Axis : " + HorizantalAxis);
		//now these two variables are from -1 to 1
		// to increase resolution you need to increase 
		// joystick reading scale 

		
	} 
}
