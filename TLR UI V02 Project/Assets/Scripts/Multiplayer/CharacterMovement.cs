using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
    public GameObject craftBTImage;
    public GameObject craftTablePanel;
    public GameObject craftTableText;

    public GameObject craftingOptionsIndex;
    public List<TextMeshProUGUI> craftingTypeTexts;
    public int index;

    //Boarders
    public GameObject HitBoarder;
    public GameObject MonsterAttackBoarder;

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

            // find boarders
            foreach (var item in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
            {
                if (item.name.Contains("HitBoarder"))
                {
                    HitBoarder = item;
                }
                else if (item.name.Contains("MonsterAttack"))
                {
                    MonsterAttackBoarder = item;
                }

            }
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
            inventory.SetActive(!inventory.activeSelf);
        }
    }

    public void OnTakeAction(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (inRangeCrafting)
            {
                craftBTImage = crafting_obj.transform.GetChild(0).GetChild(0).gameObject;
                //craftTablePanel = crafting_obj.transform.GetChild(2).gameObject;
                foreach (var item in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
                {
                    if (item.name.Contains("CraftTableText"))
                    {
                        craftTableText = item;
                    }
                    else if (item.name.Contains("CraftingPanel"))
                    {
                        craftTablePanel = item;
                    }
                    else if (item.name.Contains("CraftMeOptions"))
                    {
                        craftingOptionsIndex = item;
                    }
                }

                craftBTImage.SetActive(false);
                craftTablePanel.SetActive(true);
                craftTableText.SetActive(true);

                foreach (var item in craftingOptionsIndex.GetComponentsInChildren<TextMeshProUGUI>())
                {
                    craftingTypeTexts.Add(item);
                    item.gameObject.SetActive(false);
                }
                craftingTypeTexts[0].gameObject.SetActive(true);
            }
            else if (inRangeMonster)
            {
                monsterSlider = monster_obj.transform.GetComponentInChildren<Slider>();
                monsterSlider.value -= 1;
            }
            else if (inRangeResource)
            {
                resourceSlider = resource_obj.transform.GetComponentInChildren<Slider>();
                resourceSlider.value -= 1;
            }
            else if (offerItem)
            {
                print("pick up item");
            }
        }
    }

    // 3 Crafting buttons: 2 arrows to see craftables, 1 go to auto fill items and craft (will have delay and then show up in players inventory)
    public void OnCraftingNext(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && inRangeCrafting)
        {
            print("Join arrow");
            craftingTypeTexts[index].gameObject.SetActive(false);
            index = (index + 1) % craftingTypeTexts.Count;
            craftingTypeTexts[index].gameObject.SetActive(true);
        }
    }
    public void OnCraftingPrevious(InputAction.CallbackContext ctx)
    {
        
        if (ctx.performed && inRangeCrafting)
        {
            print("Join arrow");
            craftingTypeTexts[index].gameObject.SetActive(false);
            if (index == 0)
            {
                index = craftingTypeTexts.Count - 1;
            }
            else
            {
                index -= 1;
            }
            craftingTypeTexts[index].gameObject.SetActive(true);
        }
    }

    public void OnCraftItem(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && inRangeCrafting)
        {
            // do check
            // if items in inv and req match then display crafted item and give to player if space if not drop
            // if no items in inventory match rewuired items red and ERROR
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
            if(craftBTImage != null)
                craftBTImage.SetActive(true);
            if (craftTablePanel != null)
                craftTablePanel.SetActive(false);
            if (craftTableText != null)
                craftTableText.SetActive(false);
        }
        else if (collision.GetComponent<Collider>().tag == "MonsterEncounter")
        {
            inRangeMonster = false;
            //boarder
            MonsterAttackBoarder.SetActive(false);
        }
        else if (collision.GetComponent<Collider>().tag == "ResourceEncounter")
        {
            inRangeResource = false;
        }
        else if (collision.GetComponent<Collider>().tag == "ResourceItem")
        {
            offerItem = false;
        }
        else if (collision.GetComponent<Collider>().tag == "BoarderBoundry")
        {
            HitBoarder.SetActive(false);
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
            //for dmg
            monster_obj = collision.gameObject;
            inRangeMonster = true;
            //boarder
            MonsterAttackBoarder.SetActive(true);
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
        else if (collision.GetComponent<Collider>().tag == "BoarderBoundry")
        {
            HitBoarder.SetActive(true);
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
