using Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public float health;
    [Header("Movement")]
    public float moveSpeed = 15;
    
    public Animator animator;
    public Transform orientation;

    [Header("Aiming")]
    public CinemachineFreeLook cam;
    [SerializeField] GameObject crosshair;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private AudioSource gunSounds;
    
    bool isAiming;
    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Vector2 screenCentre = new Vector2(Screen.width/2,Screen.height/2);
    
    Rigidbody rb;
    private Camera mainCam;
    private float damageDealt = 20f;

    void Start(){
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        mainCam = GetComponentInChildren<Camera>();

        crosshair.SetActive(false);
    }

    void Update(){
        InputHandler();
        Aiming();
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            Shooting();
        }
    }

    void FixedUpdate(){
        MovePlayer();
    }

    private void InputHandler(){
        //grab player input
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer(){
        //move direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //change animation speed based on user input, reset to 0 otherwise
        if(isAiming == true){
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)){
                animator.SetFloat("Speed", 1);
            }
            else{
                animator.SetFloat("Speed", 0);
            }
            animator.SetBool("IsAiming", true);
        }
        else if(isAiming == false){
            animator.SetBool("IsAiming", false);
            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && Input.GetKey(KeyCode.LeftShift)){
                animator.SetFloat("Speed", 5);
            }
            else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.LeftShift)){
                animator.SetFloat("Speed", 2.5f);
            }
            else{
                animator.SetFloat("Speed", 0);
            }
        }


        //change sprint speed on keypress 
        if(Input.GetKey(KeyCode.LeftShift) && isAiming == false){
            moveSpeed = 30;
        }
        else if (isAiming == true){
            moveSpeed = 7.5f;
        }
        else{
            moveSpeed = 15;
        }

        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    private void Aiming(){
        //pause check
        if(Time.timeScale == 1){
            if(Input.GetKey(KeyCode.Mouse1)){
                cam.m_Lens.FieldOfView = 40;
                isAiming = true;
                crosshair.SetActive(true);
            }
            if(Input.GetKeyUp(KeyCode.Mouse1)){
                cam.m_Lens.FieldOfView = 60;
                isAiming = false;
                crosshair.SetActive(false);
            }
        }
    }

    private void Shooting(){
        //pause check
        if(Time.timeScale == 1){
            //fire ray and gather information, damage if hit enemy
            Ray ray = mainCam.ScreenPointToRay(screenCentre);
            muzzleFlash.Play();
            gunSounds.Play();
            if (Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderLayerMask)){
                debugTransform.position = hit.point;
                Debug.Log(hit.transform.gameObject.tag); //moves green debug ball to hit point
                if(hit.transform.gameObject.tag=="Enemy"){
                    hit.transform.GetComponent<EnemyManager>().takeDamage(damageDealt);
                }
            }
        }
    }
    public void takeDamage(float damage){
        health -= damage;
        if (health <= 0){
            Debug.Log("Dead");
        }
    }
}
