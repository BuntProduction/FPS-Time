using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerSolo : MonoBehaviour
{
    [SerializeField] Image healthbarImage;

    [SerializeField] Image weaponHUD;
    [SerializeField] Image hudHUD;

    [SerializeField] GameObject competences;

    [SerializeField] GameObject ui;
	[SerializeField] GameObject cameraHolder;
	[SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;

    [SerializeField] Item[] items;

    [SerializeField] GameObject liquid;

    /*[SerializeField] GameObject bullet;
    Collider bullet;*/


    int itemIndex;
    int previousItemIndex = -1;


	float verticalLookRotation;
	bool grounded;
	Vector3 smoothMoveVelocity;
	Vector3 moveAmount;

    Rigidbody rb;

    const float maxHealth = 100f;
    float currentHealth = maxHealth;

    PlayerManager playerManager;

    public TimeManager timeManager;

    public Animator animator;
    private bool isScoped = false;
    public GameObject scopeOverlay;
    public GameObject weaponCamera;
    public Camera mainCamera;
    public float scopedFOV = 25f;
    private float normalFOV;

    void Awake()
    {
    	rb = GetComponent<Rigidbody>();

    }

    void Start()
    {

    	
    	
    		EquipItem(0);
            Destroy(liquid.gameObject);
    	
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        
    	Look();
    	Move();
    	Jump();

    	for(int i = 0; i < items.Length; i++)
        {
            if(Input.GetKeyDown((i + 1).ToString()))
            {
                EquipItem(i);
                break;
            }
        }
        if(Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            if(itemIndex >= items.Length - 1)
            {
                EquipItem(0);
            }
            else
            {
                EquipItem(itemIndex + 1);

            }
        }
    	else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            if(itemIndex <= 0)
            {
                EquipItem(items.Length - 1);
            }
            else
            {
                EquipItem(itemIndex - 1);
            }

        }

        if(Input.GetMouseButtonDown(0))
        {
            items[itemIndex].Use();
        }
        if(transform.position.y < -10f)
        {
            Die();
        }

        if(Input.GetKeyDown(KeyCode.V))
        {
            //rb.interpolation = RigidbodyInterpolation.Interpolate;
            Slowmotion();
        }
        //rb.interpolation = RigidbodyInterpolation.None;

        if(Input.GetButtonDown("Fire2"))
        {
            isScoped = !isScoped;
            animator.SetBool("Scopped", isScoped);

            scopeOverlay.SetActive(isScoped);

            
            if(isScoped) 
                StartCoroutine(OnScoped());
            else
                OnUnscoped();
            //StartCoroutine(OnScoped());//delete this line and uncomment to set the scope on a click
        }

        if(Input.GetMouseButtonDown(0))
        {
            isScoped = false;
            animator.SetBool("Scopped", isScoped);
            OnUnscoped();
        }
        /*
        else if (Input.GetButtonUp("Fire2"))
        {
            isScoped = !isScoped;
            animator.SetBool("Scopped", isScoped);
            StartCoroutine(OnUnscoped());
        }*/




    }


    

    void Look()
    {
    	transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

    	verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
    	verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

    	cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;

    }
    void Move()
    {
    	Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

    	moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
    }
    void Jump()
    {
    	if(Input.GetKeyDown(KeyCode.Space) && grounded)
    	{
    		rb.AddForce(transform.up * jumpForce);
    	}
    }

    void EquipItem(int _index)
    {
        if(_index == previousItemIndex)
            return;
        itemIndex = _index;

        items[itemIndex].itemGameObject.SetActive(true);

        if(previousItemIndex != -1)
        {
            items[previousItemIndex].itemGameObject.SetActive(false);
        }
        previousItemIndex = itemIndex;

    }


    public void SetGroundedState(bool _grounded)
    {
    	grounded = _grounded;
    }

    void FixedUpdate()
    {
        
		rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }
    /*public void TakeDamage(float damage)
    {
            PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }*/
/*
    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if(!PV.IsMine)
        return;

        currentHealth -= damage;

        healthbarImage.fillAmount = currentHealth / maxHealth;

        if(currentHealth <= 0)
        {
            Die();
        }

        Debug.Log("took damage: " + damage);
    }*/

    void Die()
    {
        playerManager.Die();
    }

    void Slowmotion()
    {
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        
        timeManager.DoSlowmotion();
    }

    void OnUnscoped()
        {
            //yield return new WaitForSeconds(.1f);
            scopeOverlay.SetActive(false);
            weaponCamera.SetActive(true);
            mainCamera.fieldOfView = normalFOV;

        }

    IEnumerator OnScoped()
        {
            yield return new WaitForSeconds(.1f);
            weaponCamera.SetActive(false);
            scopeOverlay.SetActive(true);
            

            normalFOV = mainCamera.fieldOfView;
            mainCamera.fieldOfView = scopedFOV;
        }
}
