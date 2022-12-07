using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CharacterMovement : MonoBehaviour
{
    public Text displayInventory;
    private PlayerControls playerInput;
    public GameObject body;
    public bool GameStarted;

    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    //private float gravityValue = -9.81f;

    private Rigidbody _rb;
    private CapsuleCollider _col;
    private bool doJump = false;
    //private bool inPotRange = false;

    public float distanceToGround = 0.1f;
    public LayerMask groundLayer;

    public Vector3 lastLook;
    public Vector2 movementInput;

    //Assign Robot info UI
    public GameObject robotInfo;

    //Inventory on/off
    public GameObject inventory;

    //Whats In Range
    public bool inRangeMonster;
    public bool inRangeResource;
    public bool inRangeCrafting;
    public bool offerItem;

    //Items in range
    public GameObject monster_obj;
    public GameObject resource_obj;
    public GameObject crafting_obj;
    public GameObject item_obj;

    //Sliders - health
    public Slider monsterSlider;
    public Slider resourceSlider;

    //Crafting
    public GameObject craftHelp;
    public GameObject craftTable;

    //For pickup
    //public InventoryObject inventory;
    //public Text pickUpText;
    private List<GameObject> itemList = new List<GameObject>();

    private void Awake()
    {
        playerInput = new PlayerControls();
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

        //inventory.Container.Clear();
        //pickUpText.gameObject.SetActive(false);
    }

    public void BeginGame()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            // Assigning Robot Info
            int playerIndex = System.Array.IndexOf(PlayerSpawning.instance.players, gameObject) + 1;
            robotInfo = GameObject.Find("RobotInfo_" + playerIndex);

            // Assign Inventory: For turning off and on inventory
            inventory = robotInfo.transform.GetChild(0).gameObject;
            inventory.SetActive(false);
        }
        
    }

    //player movement
    private void OnEnable()
    {
        playerInput.Enable();
        BeginGame();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    public void OnShowInventory(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            print(inventory);
            inventory.SetActive(true);
        }
    }

    public void OnTakeAction(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (inRangeCrafting)
            {
                craftHelp = crafting_obj.transform.GetChild(0).gameObject;
                //craftTable = crafting_obj.transform.GetChild(1).gameObject;
                foreach (var item in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
                {
                    //print(item.name);
                    if (item.name.Contains("CraftTable"))
                    {
                        //print(item.transform.parent.transform.parent.name);
                        craftTable = item;
                    }
                }

                craftHelp.SetActive(false);
                craftTable.SetActive(true);

            }
            else if (inRangeMonster)
            {
                if (GameObject.FindGameObjectWithTag ("MonsterEncounter"))
                {
                    monsterSlider = (Slider)FindObjectOfType(typeof(Slider));
                }
                
                monsterSlider.value -= monsterSlider.value;
            }
            else if (inRangeResource)
            {
                resourceSlider = (Slider)FindObjectOfType(typeof(Slider));
                resourceSlider.value -= resourceSlider.value;
            }
            else if (offerItem)
            {
                print("pick up item");
            }
        }
    }

    public void OnMove(InputAction.CallbackContext ctx) => movementInput = ctx.ReadValue<Vector2>();
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && IsGrounded())
        {
            doJump = true;
        }
    }


    //check if player on ground
    private bool IsGrounded()
    {
        Vector3 capsuleBottom = new Vector3(_col.bounds.center.x, _col.bounds.min.y, _col.bounds.center.z);
        bool grounded = Physics.CheckCapsule(_col.bounds.center, capsuleBottom, distanceToGround, groundLayer, QueryTriggerInteraction.Ignore);
        return grounded;
    }

    
    public void OnTriggerExit(Collider collision)
    {
        if (collision.GetComponent<Collider>().tag == "TowerCraftingEncounter")
        {
            inRangeCrafting = false;
        }
        else if (collision.GetComponent<Collider>().tag == "MonsterEncounter")
        {
            inRangeMonster = false;
        }
        else if (collision.GetComponent<Collider>().tag == "ResourceEncounter")
        {
            inRangeResource = false;
        }
        else if (collision.GetComponent<Collider>().tag == "ResourceItem")
        {
            offerItem = false;
        }
    }
    
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.GetComponent<Collider>().tag == "TowerCraftingEncounter")
        {
            crafting_obj = collision.gameObject;
            inRangeCrafting = true;
        }
        else if (collision.GetComponent<Collider>().tag == "MonsterEncounter")
        {
            monster_obj = collision.gameObject;
            inRangeMonster = true;
        }
        else if (collision.GetComponent<Collider>().tag == "ResourceEncounter")
        {
            resource_obj = collision.gameObject;
            inRangeResource = true;
        }
        else if (collision.GetComponent<Collider>().tag == "ResourceItem")
        {
            item_obj = collision.gameObject;
            offerItem = true;
            //itemList.Add(collision.gameObject);
        }

    }


    //update
    private void Update()
    {
        transform.Translate(new Vector3(movementInput.x, 0, movementInput.y) * playerSpeed * Time.deltaTime);
        _rb.velocity = new Vector3(0f, _rb.velocity.y, 0f);
        if (movementInput.x != 0f || movementInput.y != 0f)
        {
            lastLook = new Vector3(movementInput.x, 0, movementInput.y);
        }
        body.transform.forward = lastLook;
    }


    private void FixedUpdate()
    {
        if (doJump)
        {
            _rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            doJump = false;
        }

    }
}
