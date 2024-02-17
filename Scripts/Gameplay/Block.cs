using DG.Tweening;
using TMPro;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private GameObject _coin;

    [SerializeField] private float _movementSpeed = 5f;

    [SerializeField] private TextMeshProUGUI _countOfTouchesText;

    private BlockSpawner _spawner;
    private Rigidbody2D _rigidBody;
    private Collider2D _collider;

    private Vector2 _direction;
    private bool _isMoving;

    private bool _isActivated;
    private bool _canActivated;
    public bool IsActivated => _isActivated;
    //public int CountOfTouches = 3;

    public void Initialize(float movementSpeed, bool hasCoin, BlockSpawner spawner)
    { 
        _movementSpeed = movementSpeed;
        _spawner = spawner;

        if (hasCoin)
            _coin.gameObject.SetActive(true);
        else
            _coin.gameObject.SetActive(false);
    }
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

        _isMoving = true;
        _collider.isTrigger = true;
        _direction = GetRandomDirection();
    }
    private void FixedUpdate()
    {

        if (_isActivated)
        {
            if (_canActivated)
                Activate();
            else
            {
                if (Ball.Instance.transform.position.y > transform.position.y)
                    _canActivated = true;
            }
        }

        if (!_isMoving)
            return;

        _rigidBody.velocity = _direction * _movementSpeed;

        if (_direction == Vector2.right)
        {
            if (transform.position.x >= BlockSpawner.Instance.GetRightLimit())
                ChangeDirection();
        }
        else
        {
            if (transform.position.x <= BlockSpawner.Instance.GetLeftLimit())
                ChangeDirection();
        }
    }
    private Vector2 GetRandomDirection()
    {
        float random = Random.Range(0f, 100f);
        if (random >= 50)
            return Vector2.right;
        else
            return -Vector2.right;
    }
    private void ChangeDirection()
    {
        if (_direction == Vector2.right)
            _direction = -Vector2.right;
        else
            _direction = Vector2.right;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            ChangeDirection();
        }
    }
    private void OnMouseDown()
    {
        //OnBlockActivated();
    }
    public void OnBlockActivated()
    {
        if (_isActivated)
            return;

        _isActivated = true;
        _isMoving = false;
        _rigidBody.velocity = Vector3.zero;
        _rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    private void Activate()
    {
        _canActivated = false;
        _collider.isTrigger = false;

        GetComponent<SpriteRenderer>().DOFade(1, 0.3f).SetLink(gameObject);

        //BlockSpawner.Instance.ActivateBlock();
    }
    /*public void UpdateTouches()
    {
        CountOfTouches--;
        _countOfTouchesText.DOFade(1, 0.3f).SetLink(_countOfTouchesText.gameObject);
        _countOfTouchesText.text = CountOfTouches.ToString();
        if (CountOfTouches <= 0)
            GameState.Instance.FinishGame();
    }*/
}
