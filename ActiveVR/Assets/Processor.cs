using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Processor : MonoBehaviour
{

    public Text text;
    public Text controlText;
    public Text accelText;
    public Text gyroText;
    public Text tempText;

    public GameObject cylinder;


    private string controlData = "";
    private string message = "";
    private double gyroX;
    private double gyroY;
    private double gyroZ;

    private double accelX;
    private double accelY;
    private double accelZ;
     

    private double prevX;
    private double prevY;
    private double prevZ;
    private int steps;

    // Use this for initialization
    void Start()
    {
        BtConnector.moduleName("HC-06");
         text.text = "" + BtConnector.connect();

    }

    // Update is called once per frame
    void Update()
    {
        controlData = BtConnector.readControlData();
        controlText.text = controlData;
        if (BtConnector.isConnected())
        {
          
                message = BtConnector.readLine();//hold the data in messageFromMC
                                                 //cause BtConnector.read () will delete the buffer

             if (message.Length > 0)
             {
                if(message.Substring(0,1) == "t")
                {
                   // tempText.text = message.Substring(1, message.Length-1);
                } else if(message.Substring(0, 1) == "a")
                {
                    accelText.text = message.Substring(1, message.Length - 1);
                    accelX = Double.Parse(message.Substring(1, message.IndexOf(",")));
                    string messageTemp = message.Substring(message.IndexOf(",") + 1);
                    accelY = Double.Parse(messageTemp.Substring(0, messageTemp.IndexOf(",")));
                    messageTemp = messageTemp.Substring(messageTemp.IndexOf(",") + 1);
                    accelZ = Double.Parse(messageTemp);

                    accelX = accelX / 32607 * 360;
                    accelY = accelY / 32607 * 360;
                    accelZ = accelZ / 32607 * 360;

                    cylinder.transform.rotation = Quaternion.Euler((float)accelX, (float)accelY, (float)accelZ);

                } else if(message.Substring(0, 1) == "g")
                {
                    prevX = gyroX;
                    prevY = gyroY;
                    prevZ = gyroZ;



                    gyroText.text = message.Substring(1, message.Length - 1);
                    gyroX = Double.Parse(message.Substring(1, message.IndexOf(",")));
                    string messageTemp = message.Substring(message.IndexOf(",") + 1);
                    gyroY = Double.Parse(messageTemp.Substring(0, messageTemp.IndexOf(",")));
                    messageTemp = messageTemp.Substring(messageTemp.IndexOf(",") + 1);
                    gyroZ = Double.Parse(messageTemp);

                    if((prevX < -5000 && gyroX > 5000) || (prevX > 5000 && gyroX < -5000) || (prevY < -5000 && gyroY > 5000) || (prevY > 5000 && gyroY < -5000) || (prevZ < -5000 && gyroZ > 5000) || (prevZ > 5000 && gyroZ < -5000))
                    {
                        steps++;
                    }

                    // transform.rotation = Quaternion.Euler((float)gyroX, (float)gyroY, (float)gyroZ);
                    tempText.text = "Steps: " + steps;
                }


                //tempText.text = message;
             }

            
        }
        
    }
}