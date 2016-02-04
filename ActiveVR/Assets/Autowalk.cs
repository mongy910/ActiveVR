using UnityEngine;
using System.Collections;

public class Autowalk : MonoBehaviour
{
    private const int RIGHT_ANGLE = 90;

    // This variable determinates if the player will move or not 
    private bool isWalking = false;

    //CardboardHead head = null;

    //This is the variable for the player speed
    [Tooltip("With this speed the player will move.")]
    public float speed;

    [Tooltip("Activate this checkbox if the player shall move when the Cardboard trigger is pulled.")]
    public bool walkWhenTriggered;

    [Tooltip("Activate this checkbox if the player shall move when he looks below the threshold.")]
    public bool walkWhenLookDown;

    [Tooltip("This has to be an angle from 0° to 90°")]
    public double thresholdAngle;

    [Tooltip("Activate this Checkbox if you want to freeze the y-coordiante for the player. " +
             "For example in the case of you have no collider attached to your CardboardMain-GameObject" +
             "and you want to stay in a fixed level.")]
    public bool freezeYPosition;

    [Tooltip("This is the fixed y-coordinate.")]
    public float yOffset;

    void Start()
    {
        //head = Camera.main.GetComponent<StereoController>().Head;
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            //Debug.Log("Pressing w");
            GetComponent<Rigidbody>().AddForce(0, 0, -4);
        }

       

      

        if (freezeYPosition)
        {
            transform.position = new Vector3(transform.position.x, yOffset, transform.position.z);
        }
    }
}