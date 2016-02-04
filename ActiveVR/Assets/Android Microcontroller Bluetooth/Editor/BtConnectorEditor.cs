using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(BtConnector))]
public class BtConnectorEditor : Editor 
{
	string [] modes = new string [3] {"lines" , "buffer" ,"don't read"};
	int index;

	public override void OnInspectorGUI()
	{
		BtConnector MyTarget = (BtConnector)target;
		//DrawDefaultInspector();
		if (MyTarget.mode0)
						index = 0;
				else if (MyTarget.stopReading)
						index = 2;
				else 
						index = 1;

		index = EditorGUILayout.Popup("reading mode " ,index, modes);
		if (index == 1) {
						MyTarget.mode0 = false;
						MyTarget.stopReading = false;
						
						MyTarget.length = EditorGUILayout.IntField ("buffer length", MyTarget.length);
						
						MyTarget.mode3 = EditorGUILayout.Toggle ("return data every " + MyTarget.length + " bytes", MyTarget.mode3);
						if(!MyTarget.mode3){
						MyTarget.mode2 = EditorGUILayout.Toggle ("use terminal byte", MyTarget.mode2);
						
						if (MyTarget.mode2)
								MyTarget.terminalByte = (byte)EditorGUILayout.IntField ("terminal byte", MyTarget.terminalByte);
						}else MyTarget.mode2 = false;
				} else if (index == 0) {
						
						MyTarget.mode0 = true;
						MyTarget.stopReading = false;
						MyTarget.mode3 = false;
						MyTarget.mode2 = false;
				} else if (index == 2) {
						MyTarget.mode0 = false;
						MyTarget.mode3 = false;
						MyTarget.mode2 = false;
						MyTarget.stopReading = true;
				}
		if(index == 0)
			EditorGUILayout.HelpBox("reading a line of string, a line is represented by zero or more characters followed by '\\n', '\\r' or \"\\r\\n\"", MessageType.Info);
		else if (index == 1 && !MyTarget.mode2 && !MyTarget.mode3)
			EditorGUILayout.HelpBox("reading any available data (bytes) in the buffer", MessageType.Info);
		else if (index == 1 && MyTarget.mode2)
			EditorGUILayout.HelpBox("reading bytes  until the terminal byte ( " + MyTarget.terminalByte.ToString() + " : " +
			                        ((char)MyTarget.terminalByte) + " ) is detected, or the buffer is full", MessageType.Info);
		else if (index == 1 && MyTarget.mode3)
			EditorGUILayout.HelpBox("reading bytes  until the buffer is full", MessageType.Info);
		else if(index == 2)
			EditorGUILayout.HelpBox("don't read", MessageType.Info);

		if(index == 0)
			EditorGUILayout.HelpBox("you should use BtConnector.readLine()", MessageType.Info);
		else if (index == 1 )
			EditorGUILayout.HelpBox("you should use BtConnector.readBuffer()", MessageType.Info);



						EditorUtility.SetDirty (MyTarget);
	}
}