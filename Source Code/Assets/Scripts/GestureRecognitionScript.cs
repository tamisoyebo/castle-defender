using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using PDollarGestureRecognizer;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class GestureRecognitionScript : MonoBehaviour
{
    public float inputThreshold = 0.1f;
    public float newPosThreshold = 0.03f;
    public float offsetDebug = 1f;
    private bool moving = false;
    public XRNode inputSource;
    public InputHelpers.Button inputButton;
    public Transform movementSource;
    public GameObject debugTrail;
    public GameObject rightHandController;
    //PDollar Method Information
    public Result result;
    private List<Vector3> positionsList = new List<Vector3>();
    private List<Gesture> trainingSet = new List<Gesture>();
    public ScoreManager manager;
    //Angle Position Information
    private Vector2 startPos;
    private Vector2 endPos;
    public float angle;
    //Spell Assets
    public GameObject fireballAsset;
    public GameObject lightningAsset;
    public GameObject iceAsset;
    public GameObject windAsset;
    public float fireSpeed = 100f;
    //enemy score values
    public int iceScore = 100;
    public int fireScore = 100;
    public int lightningScore = 100;
    //Sound info
    public AudioSource soundSource;
    public AudioClip lightningSound;
    public AudioClip fireSound;
    public AudioClip iceSound;
    public AudioClip wand;
    //Debug Info
    public LineRenderer lr;
    public string spellCheck = "No Spell";
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(inputSource), inputButton, out bool pressed, inputThreshold);
        if (!moving && pressed)
        {
            StartMovement();
        }
        else if(moving && pressed)
        {
            inMovement();
        }
        else if(moving && !pressed)
        {
            EndMovement();
        }
        
    }
    //Movement states and gesture recognition based on angle between start and endpoints
    void StartMovement()
    {
        Debug.Log("Start Movement");
        moving = true;
        
        /*positionsList.Clear();
        positionsList.Add(movementSource.position);*/
        
        Destroy(Instantiate(debugTrail, movementSource.position, Quaternion.identity), 2);

        startPos = new Vector2(movementSource.position.x, movementSource.position.y);
    }
    void inMovement()
    {
        Debug.Log("Moving");
        /*Vector3 lastPosition = positionsList[positionsList.Count - 1];
        if(Vector3.Distance(movementSource.position, lastPosition) > newPosThreshold)
        {
            positionsList.Add(movementSource.position);
        }*/
        Destroy(Instantiate(debugTrail, (movementSource.position + (Vector3.forward * offsetDebug)), movementSource.rotation), 2);
 
        
    }
    void EndMovement()
    {
        Debug.Log("End Movement");
        moving = false;
        /*Point[] points = new Point[positionsList.Count];
        for(int i = 0; i < positionsList.Count; i++)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(positionsList[i]);
            points[i] = (new Point(screenPoint.x, screenPoint.y, 0));
        }

        Gesture newGesture = new Gesture(points);
        result = PointCloudRecognizer.Classify(newGesture, trainingSet.ToArray());
        Debug.Log(result.GestureClass + result.Score);
        if(result.GestureClass == "line~01")
        {
            Fireball();

        }
        else if(result.GestureClass == "five_point_star~01")
        {
            LightningSpell();          
        }*/
        endPos = new Vector2(movementSource.position.x, movementSource.position.y);
        angle = Vector2.Angle(startPos, endPos);
        if((angle > 0f) && (angle <= 0.1f))
        {
            spellCheck = "Fireball";
            Fireball();
        }
        else if((angle >= 0.10f) && (startPos.x > endPos.x))
        {
            spellCheck = "Lightning";
            LightningSpell();
        }
        else if((angle >= 0.10f) && (startPos.x < endPos.x))
        {
            spellCheck = "Ice";
            Ice();
        }
        /*else if(angle >= 0.27f)
        {
            spellCheck = "Wind";
            Wind();
        }*/
    }

    //Spell References
    void LightningSpell()
    {
        Debug.Log("Lightning Cast");
        lr.positionCount = 2;
        float spellTimer = 5f; 
        int frameCounter = 0;
        
        soundSource.PlayOneShot(lightningSound, 1f);

        while(spellTimer > 0f)
        {
            Vector3 targetPos =  (movementSource.position + (movementSource.forward * offsetDebug));
            lr.SetPosition(0, movementSource.position);
            lr.SetPosition(1,targetPos);
            Vector3 gestureLoc = movementSource.position;
            Quaternion gestureAngle = movementSource.rotation;
            
            frameCounter++;
            if((frameCounter % 2) == 0)
            {
                GameObject lightningBall = Instantiate(lightningAsset, gestureLoc, gestureAngle);
                lightningBall.GetComponent<Rigidbody>().AddRelativeForce(movementSource.forward * fireSpeed);
            }
            spellTimer -= Time.deltaTime;
        }
        lr.positionCount = 0;
    }

    void Fireball()
    {
        GameObject fireball = Instantiate(fireballAsset, movementSource.position, movementSource.rotation);
        fireball.GetComponent<Rigidbody>().AddRelativeForce(movementSource.forward * fireSpeed);

        soundSource.PlayOneShot(fireSound, 1f);
        
        Debug.Log("Fireball Cast");
    }
    
    void Ice()
    {
        Debug.Log("Ice Cast");

        lr.positionCount = 2;
        float spellTimer = 5f;
        int frameCounter = 0;
        
        soundSource.PlayOneShot(iceSound, 1f);

        while(spellTimer > 0f)
        {
            Vector3 targetPos =  (movementSource.position + (movementSource.forward * offsetDebug));
            lr.SetPosition(0, movementSource.position);
            lr.SetPosition(1,targetPos);
            Vector3 gestureLoc = movementSource.position;
            Quaternion gestureAngle = movementSource.rotation;
            
            frameCounter++;
            if((frameCounter % 2) == 0)
            {
                GameObject iceBall = Instantiate(iceAsset, gestureLoc, gestureAngle);
                iceBall.GetComponent<Rigidbody>().AddRelativeForce(movementSource.forward * fireSpeed);
            }
            spellTimer -= Time.deltaTime;
        }
        
        lr.positionCount = 0;
    }

    void Wind()
    {
        Debug.Log("Wind Cast");

        lr.positionCount = 2;
        float spellTimer = 5f;
        int frameCounter = 0;
        
        while(spellTimer > 0f)
        {
            Vector3 targetPos =  (movementSource.position + (movementSource.forward * offsetDebug));
            lr.SetPosition(0, movementSource.position);
            lr.SetPosition(1,targetPos);
            Vector3 gestureLoc = movementSource.position;
            Quaternion gestureAngle = movementSource.rotation;
            
            frameCounter++;
            if((frameCounter % 2) == 0)
            {
                GameObject windBall = Instantiate(iceAsset, gestureLoc, gestureAngle);
                windBall.GetComponent<Rigidbody>().AddRelativeForce(movementSource.forward * fireSpeed);
            }
            spellTimer -= Time.deltaTime;
        }
        lr.positionCount = 0;
    }
}