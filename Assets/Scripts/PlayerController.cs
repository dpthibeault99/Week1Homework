using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(CharacterController))] // will add the dependencey to the object
public class PlayerController : MonoBehaviour
{
    // test for git
    // another git test
    [Header("Movement")]
    [Tooltip("Set Speed of player movement")]
    public float moveSpeed = 5f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.8f;
    public float rotaionSmoothTime = 0.1f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance;
    public LayerMask groundMask;

    [Header("Refrences")]
    public Transform cameraTransform;


    // private

    CharacterController _characterController;
    Vector3 _velocity;
    Vector2 _moveInput;
    bool _isGrounded;
    bool _isJumping;
    float _jumpCoolDown = 0f;
    bool _jumpPressed;
    float _rotationVelocity;

    bool _onPlatform;



    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        // auto find camera transform if not assigned
        if(cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    // inputs via messageing
    /// <summary>
    /// this handles the movent via input
    /// </summary>
    /// <param name="value"> input value passed from input system</param>

    public void OnMove(InputValue value) // needed to be OnMove , not onMove
    {
        //Debug.Log(value.Get<Vector2>());
        _moveInput = value.Get<Vector2>();

    }
    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            _jumpPressed = true;
        }
    }
    public void handleMovement()
    {
        if(_moveInput.sqrMagnitude < 0.01f)
        {
            return;
        }

        float speed = moveSpeed;

        // camera related directions
        float targetAngle = Mathf.Atan2(_moveInput.x,_moveInput.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;

        //smooth rotaion tward move direction
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _rotationVelocity, rotaionSmoothTime);

        transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

        //Vector3 moveDir = new Vector3 (_moveInput.x, 0f , _moveInput.y);
        
        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        if (!_onPlatform)
        _characterController.Move(moveDir.normalized * speed * Time.deltaTime);
    }

    void handleGroundCheck()
    {
        if (_jumpCoolDown > 0f)
        {
            _jumpCoolDown -= Time.deltaTime;
            _isGrounded = false;
            return;
        }
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //var movingPlatform = Physics.OverlapSphere(groundCheck.position, groundDistance);
        //foreach (var platform in movingPlatform) 
        //{
        //    if (platform.CompareTag("MovingPlatform"))
        //    {
        //        Debug.Log("Hit Platform");
        //        transform.parent = platform.gameObject.transform;
        //    }        
        //}


        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
            _isJumping = false;
           // transform.parent = null;
        }

    }



    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Hola");
        handleMovement();
        handleJump();
        ApplyGravity();
        handleGroundCheck();
        if (_isGrounded)
        {
           // Debug.Log("on ground");
        }
       // Debug.Log(_velocity.y);
    }

    /// <summary>
    /// this function handles gravity for the charater
    /// </summary>
    void ApplyGravity()
    {
        _velocity.y += gravity * Time.deltaTime;
        _characterController.Move(new Vector3(0, _velocity.y, 0) * Time.deltaTime);
    }

    void handleJump()
    {
        if (_jumpPressed && _isGrounded && !_isJumping)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            _isJumping = true;
            _jumpCoolDown = 0.5f;
        }

        _jumpPressed = false;   
    }

    private void OnDrawGizmos()
    {
        if (groundCheck == null) return;
        Gizmos.color = _isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position , groundDistance);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "MovingPlatform")
        {
            transform.parent = hit.transform;
            _onPlatform = true;
        }
        else
        {
            transform.parent = null;
        }
    }
}
