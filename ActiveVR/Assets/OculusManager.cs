using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO.Ports;

public class OculusManager : MonoBehaviour
{
    //Public variables assigned in Unity editor
    public GameObject player;
    public GameObject velocityObject;
    public Text speedText;
    public Text fpsText;
    public AudioClip bikePedaling;
    public AudioClip bikeSpinning;
    public string port = "COM3";
    /* The baudrate of the serial port. */
    [Tooltip("The baudrate of the serial port")]
    public int baudrate = 9600;

    //Audio
    //private AudioSource source;
    private AudioSource[] sources;
    private float lowPitchRange = .75f;
    private float highPitchRange = 1.5f;
    private float velToVol = .2f;
    private SerialPort stream;

    //FPS Counter
    /*private int fpsCounter;
    private float fpsTotal;
    */
    //Arduino data + recent data
    private string controlData = "";
    private string message = "";
    private double gyroX;
    private double gyroY;
    private double gyroZ;
    private double prevX;
    private double prevY;
    private double prevZ;
    /*
    //Not using accel data
    private double accelX;
    private double accelY;
    private double accelZ;
    */

    private double speed;
    private double velocity;
    private double prevVelocity;
    public int notPedalingCounter;

    private int steps;
    //private CardboardHead head = null;
    public GameObject head;

    // private int test;

    // Use this for initialization
    void Start()
    {

        sources = player.GetComponents<AudioSource>();
        sources[0].clip = bikePedaling;
        sources[1].clip = bikeSpinning;
        // Application.targetFrameRate = 20;
        Open();
        //Connect to bluetooth
       /* BtConnector.moduleName("HC-06");
        BtConnector.connect();
        */
        //Initialize Cardboard head
        //head = Camera.main.GetComponent<StereoController>().Head;

        //Initialize Audio source
        //source = player.GetComponent<AudioSource>();
        //source.clip = bikePedaling;
        
    }


    public void WriteToArduino(string message)
    {
        // Send the request
        stream.WriteLine(message);
        stream.BaseStream.Flush();
    }

    public void Open()
    {
        // Opens the serial port
        stream = new SerialPort("COM3", baudrate);
        stream.ReadTimeout = 50;
        stream.Open();
        //this.stream.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
        Debug.Log(ReadFromArduino());
    }


    public string ReadFromArduino()
    {
        stream.ReadTimeout = 50;
        try
        {
            return stream.ReadLine();
        }
        catch (TimeoutException)
        {
            return null;
        }
    }


    public IEnumerator AsynchronousReadFromArduino(Action<string> callback, Action fail = null, float timeout = float.PositiveInfinity)
    {
        DateTime initialTime = DateTime.Now;
        DateTime nowTime;
        TimeSpan diff = default(TimeSpan);

        string dataString = null;

        do
        {
            // A single read attempt
            try
            {
                dataString = stream.ReadLine();
            }
            catch (TimeoutException)
            {
                dataString = null;
            }

            if (dataString != null)
            {
                callback(dataString);
                yield return null;
            }
            else
                yield return new WaitForSeconds(0.05f);

            nowTime = DateTime.Now;
            diff = nowTime - initialTime;

        } while (diff.Milliseconds < timeout);

        if (fail != null)
            fail();
        yield return null;
    }

    public void Close()
    {
        stream.Close();
    }



    // Update is called once per frame
    void Update()
    {
        prevVelocity = velocity;
        velocity = Math.Abs(Math.Round(player.GetComponent<Rigidbody>().velocity.magnitude, 2));

        if (prevVelocity >= velocity)
        {
            notPedalingCounter++;
        }
        else
        {
            notPedalingCounter = 0;
        }



        //Update audio source
        if (!sources[1].isPlaying && notPedalingCounter > 150 && velocity < 10)
        {
            sources[1].Play();
        }
        else if (sources[1].isPlaying && (notPedalingCounter <= 150 || velocity >= 10))
        {
            sources[1].Pause();
        }
        else if (sources[1].isPlaying)
        {
            sources[1].volume = player.GetComponent<Rigidbody>().velocity.magnitude / 200;
        }




        /*fpsTotal += Time.deltaTime;
        fpsCounter++;
        if(fpsCounter == 5)
        {
            fpsText.text = "FPS: " + (1 / (fpsTotal / 5)).ToString("F2");
            fpsTotal = 0;
            fpsCounter = 0;
        }
        */

        
        Vector3 forward = head.transform.forward;
        forward.y = 0;
        player.transform.position += forward * Time.deltaTime * velocityObject.GetComponent<Rigidbody>().velocity.z;
        




        //If using google cardboard + phone
        //speedText.text = "Velocity: " + Math.Abs(Math.Round(velocityObject.GetComponent<Rigidbody>().velocity.z, 2));

        //If testing on unity editor
        speedText.text = "Velocity: " + Math.Abs(Math.Round(player.GetComponent<Rigidbody>().velocity.magnitude, 2));

        if (player.GetComponent<Rigidbody>().velocity.magnitude < 2)
        {
            player.GetComponent<Rigidbody>().drag = 1.5f;
        }
        else if (player.GetComponent<Rigidbody>().velocity.magnitude < 5)
        {
            player.GetComponent<Rigidbody>().drag = .5f;
        }
        else if (player.GetComponent<Rigidbody>().velocity.magnitude >= 5)
        {
            player.GetComponent<Rigidbody>().drag = .2f;
        }

        //Update audio source
        if (!sources[0].isPlaying && player.GetComponent<Rigidbody>().velocity.magnitude > .5)
        {
            sources[0].Play();
        }
        else if (sources[0].isPlaying && player.GetComponent<Rigidbody>().velocity.magnitude <= .5)
        {
            sources[0].Pause();
        }
        else if (sources[0].isPlaying)
        {
            sources[0].volume = player.GetComponent<Rigidbody>().velocity.magnitude / 30;
        }










        //controlData = BtConnector.readControlData();
      //  if (BtConnector.isConnected())
      //  {
            //   if (test == 5)
            //{


            //message = BtConnector.readLine();//hold the data in messageFromMC
            //cause BtConnector.read () will delete the buffer
            string thing = ReadFromArduino();
            if(thing != null) {

            message = thing;
            Debug.Log(message);
                if (message.Length > 0)
                {
                    if (message.Substring(0, 1) == "t")
                    {
                        // tempText.text = message.Substring(1, message.Length-1);
                    }
                    else if (message.Substring(0, 1) == "a")
                    {

                        /* accelX = Double.Parse(message.Substring(1, message.IndexOf(",")));
                         string messageTemp = message.Substring(message.IndexOf(",") + 1);
                         accelY = Double.Parse(messageTemp.Substring(0, messageTemp.IndexOf(",")));
                         messageTemp = messageTemp.Substring(messageTemp.IndexOf(",") + 1);
                         accelZ = Double.Parse(messageTemp);

                         accelX = accelX / 32607 * 360;
                         accelY = accelY / 32607 * 360;
                         accelZ = accelZ / 32607 * 360;
                         */
                    }
                    else if (message.Substring(0, 1) == "g")
                    {
                        prevX = gyroX;
                        prevY = gyroY;
                        prevZ = gyroZ;


                        gyroX = Double.Parse(message.Substring(1, message.IndexOf(",")));
                        string messageTemp = message.Substring(message.IndexOf(",") + 1);
                        gyroY = Double.Parse(messageTemp.Substring(0, messageTemp.IndexOf(",")));
                        messageTemp = messageTemp.Substring(messageTemp.IndexOf(",") + 1);
                        gyroZ = Double.Parse(messageTemp);

                        double value = Math.Pow(Math.Pow(Math.Abs(gyroX - prevX), 2) + Math.Pow(Math.Abs(gyroY - prevY), 2) + Math.Pow(Math.Abs(gyroZ - prevZ), 2), .5);

                        velocityObject.GetComponent<Rigidbody>().AddForce(0, 0, (float)value / 800);
                         // player.GetComponent<Rigidbody>().AddForce(player.transform.forward * (float)value / 1000 * -1);
                        // Vector3 direction = new Vector3(head.transform.forward.x, 0, head.transform.forward.z).normalized * (int)value * Time.deltaTime;
                        //Quaternion rotation = Quaternion.Euler(new Vector3(0, -transform.rotation.eulerAngles.y, 0));
                        // player.GetComponent<Rigidbody>().AddForce(head.transform.forward * (float)value / 1000);
                        /* if ((prevX < -5000 && gyroX > 5000) || (prevX > 5000 && gyroX < -5000) || (prevY < -5000 && gyroY > 5000) || (prevY > 5000 && gyroY < -5000) || (prevZ < -5000 && gyroZ > 5000) || (prevZ > 5000 && gyroZ < -5000))
                         {
                             steps++;
                             player.GetComponent<Rigidbody>().AddForce(0, 0, 100);
                         }
                         */
                        // transform.rotation = Quaternion.Euler((float)gyroX, (float)gyroY, (float)gyroZ);



                    }
           //     }

                //tempText.text = message;
            }
            /*   } else {
                   test++;
               }
               */

        }

    }
}