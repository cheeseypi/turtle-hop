using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;

    public float MovementSpeed = 5;
    public float JumpForce = 5;

    public int numPlatforms = 2;

    public GameObject platform;

    public AudioSource Jump;
    public AudioSource Land;
    public AudioSource Walk;
    public AudioSource PlacePlat;
    public AudioSource CollectibleGet;

    public bool CanLeaveLevel { get => _collectedMagic; }

    private Rigidbody2D _rigidBody;
    private BoxCollider2D _boxCollider;
    private bool _collectedMagic = false;

    private bool _isGrounded
    {
        get
        {
            float paddingHeight = 0.13f;
            var centerVector = _boxCollider.bounds.center;
            var leftVector = new Vector2(_boxCollider.bounds.center.x - _boxCollider.bounds.extents.x + 0.01f, _boxCollider.bounds.center.y);
            var rightVector = new Vector2(_boxCollider.bounds.center.x + _boxCollider.bounds.extents.x - 0.01f, _boxCollider.bounds.center.y);
            var length = _boxCollider.bounds.extents.y + paddingHeight;
            RaycastHit2D centerHit = Physics2D.Raycast(centerVector, Vector2.down, length, layerMask);
            RaycastHit2D leftHit = Physics2D.Raycast(leftVector, Vector2.down, length, layerMask);
            RaycastHit2D rightHit = Physics2D.Raycast(rightVector, Vector2.down, length, layerMask);
            Debug.DrawRay(centerVector, Vector2.down * length, centerHit.collider == null ? Color.red : Color.green);
            Debug.DrawRay(leftVector, Vector2.down * length, leftHit.collider == null ? Color.red : Color.green);
            Debug.DrawRay(rightVector, Vector2.down * length, rightHit.collider == null ? Color.red : Color.green);
            if (new List<RaycastHit2D> { centerHit, leftHit, rightHit }.Any(x => x.collider != null))
            {
                return true;
            }
            return false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var movement = Input.GetAxis("Horizontal");
        //transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * MovementSpeed;
        _rigidBody.velocity = new Vector2(MovementSpeed * movement, _rigidBody.velocity.y);

        //if (!Mathf.Approximately(0, movement))
        //transform.rotation = movement > 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;

        if (!_collectedMagic && Input.GetButtonDown("Jump") && !_isGrounded && numPlatforms > 0)
        {
            //_rigidBody.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
            var createdPlatform = Instantiate(platform, new Vector3(transform.position.x, transform.position.y - .5f, 0), Quaternion.Euler(0, 0, 0));
            var platformHitbox = createdPlatform.GetComponent<BoxCollider2D>();
            var anInternIsGoingToBeMurderedList = new List<Collider2D>();
            if (platformHitbox.OverlapCollider(new ContactFilter2D().NoFilter(), anInternIsGoingToBeMurderedList) > 0)
            {
                Destroy(createdPlatform);
            }
            else
            {
                numPlatforms -= 1;
                impartJumpForce();
                PlacePlat.Play();
            }
        }
        else if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            impartJumpForce();
            Jump.Play();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void impartJumpForce()
    {
        _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, JumpForce);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "MagicSphere")
        {
            Destroy(collision.gameObject);
            _collectedMagic = true;
            CollectibleGet.Play();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isGrounded)
        {
            Land.Play();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (_collectedMagic && collision.collider.name == platform.name + "(Clone)")
        {
            Destroy(collision.collider.gameObject);
        }
    }
}
