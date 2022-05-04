using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using PDollarGestureRecognizer;

public class PlayerMovementController : MonoBehaviour
{
    // Start is called before the first frame update
   [Header("Movement Settings")]
    public float walkSpeed;
    public float sprintSpeed;
    public float jumpForce;
    public SprintType sprintType;
    public float gravityScale = 1;
    public bool isInAir = false;
    //other Variables
    CharacterController charControl;
    [Tooltip("The transform which the movement is relative to, for typical fps movement select main camera. But it can be anything")]
    public Transform mainCamera;
    Vector2 pInput;

    public PlayerInputController playerInputController;
    float movementSpeed;
    float gravity;
    bool isSprinting = false;
    public bool moving = false;
    public float inputThreshold = 0.1f;
    public float newPosThreshold = 0.03f;
    public Transform movementSource;
    public GameObject debugTrail;
    private List<Vector3> positionsList = new List<Vector3>();
    private List<Gesture> trainingSet = new List<Gesture>();
    //Fireball Spell Assets
    public GameObject fireballAsset;
    public float fireSpeed = 10f;
    
    
   
    void Start()
    {
        charControl = GetComponent<CharacterController>();
        
        //playerInputController = FindObjectOfType<PlayerInputController>();
    }




    // Update is called once per frame
    private void Update()
    {

        #region Jump
        if (isInAir == false)
        {
            if (playerInputController.inputActions.Player.Jump.triggered)
            { 
            gravity = jumpForce * Time.deltaTime;
            isInAir = true;
            }
        }
        #endregion
        
        #region Gesture
        if(playerInputController.inputActions.Player.Fire.triggered && !moving)
        {
            StartMovement();
        }
        else if(moving && playerInputController.inputActions.Player.Fire.triggered)
        {
            inMovement();
        }
        else if(moving && !playerInputController.inputActions.Player.Fire.triggered)
        {
            EndMovement();
        }
        #endregion
        #region sprinting
        if (isSprinting)
        {
            movementSpeed = sprintSpeed;
        }
        else
        {
            movementSpeed = walkSpeed;
        }
        switch(sprintType)
        {
            case SprintType.clickToSprint:
                if(playerInputController.inputActions.Player.Sprint.triggered)
                isSprinting = !isSprinting;    
                break;
            case SprintType.holdToSprint:
                playerInputController.inputActions.Player.Sprint.performed += sprint => isSprinting = true;
                playerInputController.inputActions.Player.Sprint.canceled += sprint => isSprinting = false;
               
                break;

        }
       
        #endregion
        
        //set y rotation = camera y rotation
        transform.rotation = Quaternion.Euler(new Vector3(0, mainCamera.eulerAngles.y, 0));
       
        pInput = playerInputController.inputActions.Player.Move.ReadValue<Vector2>();
        
        charControl.Move(transform.right * pInput.x * movementSpeed * Time.deltaTime + transform.forward * pInput.y * movementSpeed * Time.deltaTime + gravity * transform.up * Time.deltaTime);
    }
    private void FixedUpdate()
    {
       
        if (charControl.isGrounded)
        {
            isInAir = false;
        }
        else
        {
            isInAir = true;
            gravity += gravityScale*Physics.gravity.y*Time.deltaTime;
        }
      
    }
    public enum SprintType
    {
        clickToSprint,
        holdToSprint
    }
    void StartMovement()
    {
        Debug.Log("Start Movement");
        moving = true;
        positionsList.Clear();
        positionsList.Add(movementSource.position);
        if(debugTrail)
        {
            Destroy(Instantiate(debugTrail, movementSource.position, Quaternion.identity), 5);
        }
        
    }
    void inMovement()
    {
        Debug.Log("Moving");
        Vector3 lastPosition = positionsList[positionsList.Count - 1];
        if(Vector3.Distance(movementSource.position, lastPosition) > newPosThreshold)
        {
            positionsList.Add(movementSource.position);
        }
        if(debugTrail)
        {
            Destroy(Instantiate(debugTrail, new Vector3(movementSource.position.x, movementSource.position.y, movementSource.position.z + 1f), Quaternion.identity), 5);
        }
        
    }
    void EndMovement()
    {
        Debug.Log("End Movement");
        moving = false;
        Point[] points = new Point[positionsList.Count];
        for(int i = 0; i < positionsList.Count; i++)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(positionsList[i]);
            points[i] = (new Point(screenPoint.x, screenPoint.y, 0));
        }

        Gesture newGesture = new Gesture(points);
        Result result = PointCloudRecognizer.Classify(newGesture, trainingSet.ToArray());
        Debug.Log(result.GestureClass + result.Score);
        if(result.GestureClass == "line~01")
        {
            Fireball();

        }
        /*else if(result.GestureClass == "five_point_star~01")
        {
            LightningSpell();          
        }*/
    }

    //Spell References
    /*void LightningSpell()
    {
        float spellTimer = 5f;
        Debug.Log("Lightning Cast");
        RaycastHit hit;
        while(spellTimer > 0f)
        {
            if(Physics.Raycast(rightHandController.transform.position, new Vector3(rightHandController.transform.position.x, rightHandController.transform.position.y, (rightHandController.transform.position.z + 10f)), out hit, Mathf.Infinity))
            {
                if(hit.transform.gameObject.tag == "lightningEnemy")
                {
                    Destroy(hit.transform.gameObject);
                }
            }
            spellTimer -= Time.deltaTime;
        }
    }*/

    void Fireball()
    {
        GameObject fireball = Instantiate(fireballAsset, transform.position, Quaternion.identity);
        fireball.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * fireSpeed);
        Debug.Log("Fireball Cast");
    }
    
    /*void Ice()
    {
        float spellTimer = 5f;
        Debug.Log("Ice Cast");
        RaycastHit hit;
        while(spellTimer > 0f)
        {
            if(Physics.Raycast(rightHandController.transform.position, new Vector3(rightHandController.transform.position.x, rightHandController.transform.position.y, (rightHandController.transform.position.z + 10f)), out hit, Mathf.Infinity))
            {
                if(hit.transform.gameObject.tag == "iceEnemy")
                {
                    Destroy(hit.transform.gameObject);
                }
            }
            spellTimer -= Time.deltaTime;
        }
    }

    void Wind()
    {
        float spellTimer = 5f;
        Debug.Log("Wind Cast");
        RaycastHit hit;
        while(spellTimer > 0f)
        {
            if(Physics.Raycast(rightHandController.transform.position, new Vector3(rightHandController.transform.position.x, rightHandController.transform.position.y, (rightHandController.transform.position.z + 10f)), out hit, Mathf.Infinity))
            {
                hit.transform.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 0f, 1f) * windForce);
            }
            spellTimer -= Time.deltaTime;
        }
    }*/
}
  

