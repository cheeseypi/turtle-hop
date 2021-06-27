using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float MovementSpeed = 5;
    public float JumpForce = 5;

    public int numPlatforms = 2;

    public GameObject platform;

    public bool CanLeaveLevel { get => _collectedMagic; }

    private Rigidbody2D _rigidBody;
    private bool _collectedMagic = false;

    private bool _isCollided = false;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        var movement = Input.GetAxis("Horizontal");
        //transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * MovementSpeed;
        _rigidBody.velocity = new Vector2(MovementSpeed * movement, _rigidBody.velocity.y);

        //if (!Mathf.Approximately(0, movement))
        //transform.rotation = movement > 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;

        if (!_collectedMagic && Input.GetButtonDown("Jump") && !_isCollided && numPlatforms > 0)
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
            }
        }
        else if (Input.GetButtonDown("Jump") && _isCollided)
        {
            impartJumpForce();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("Level1");
        Destroy(this.gameObject);
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
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _isCollided = true;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        _isCollided = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (_collectedMagic && collision.collider.name == platform.name + "(Clone)")
        {
            Destroy(collision.collider.gameObject);
        }
        _isCollided = false;
    }
}
