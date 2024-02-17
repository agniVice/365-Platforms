using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Ball : MonoBehaviour, ISubscriber
{
    public static Ball Instance;

    [SerializeField] private float _jumpForce = 5f;

    private Rigidbody2D _rigidBody;
    private Collider2D _collider;

    private bool _isJumping = true;

    private float _currentTimer;
    private float _timer = 1f;

    private Vector2 _startPosition;
    private List<GameObject> _blocksUsed = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }
    private void Start()
    {
        _startPosition = transform.position;
    }
    private void Update()
    {
        if (GameState.Instance.CurrentState == GameState.State.InGame)
        {
            if (_rigidBody.velocity.magnitude <= 0.05f)
            {
                if (_currentTimer <= 0)
                    Jump();
                else
                    _currentTimer -= Time.fixedDeltaTime;
            }
        }
        if (BlockSpawner.Instance.GetActivatedBlocksCount() == -1)
        {
            if (_isJumping)
            {
                if (transform.position.y <= _startPosition.y)
                {
                    _isJumping = false;
                    _rigidBody.velocity = Vector3.zero;
                }
                if (!_isJumping)
                {
                    _isJumping = true;
                    Jump();
                }
                if (_rigidBody.velocity == Vector2.zero)
                {
                    Jump();
                }
            }
        }
    }
    private void ResetBall()
    {
        _rigidBody.velocity = Vector3.zero;
        _isJumping = true;
        transform.position = _startPosition;
        _blocksUsed.Clear();
    }
    private void Jump()
    {
        AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.Jump, Random.Range(0.9f, 1.1f));
        _rigidBody.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
        _currentTimer = _timer;
    }
    public void SubscribeAll()
    {
        GameState.Instance.LevelAdded += ResetBall;
    }
    public void UnsubscribeAll()
    {
        GameState.Instance.LevelAdded -= ResetBall;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameState.Instance.CurrentState != GameState.State.InGame)
            return;

        if (collision.CompareTag("Finish"))
        {
            AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.LevelUp, 1f);
            PlayerScore.Instance.AddLevel();
            GameState.Instance.AddLevel();
        }
        if (collision.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.Coin, 1f);
            PlayerScore.Instance.AddCoin();
        }
        if (collision.CompareTag("End"))
        {
            GameState.Instance.FinishGame();
            _collider.enabled = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameState.Instance.CurrentState != GameState.State.InGame)
            return;

        if (collision.gameObject.CompareTag("Block"))
        {
            if (collision.gameObject.GetComponent<Block>().IsActivated)
            {
                //if(collision.relativeVelocity.y < 0
                if (BlockSpawner.Instance.GetActivatedBlocksCount() > 0)
                {
                    if (collision.gameObject.GetComponent<Block>() != BlockSpawner.Instance._blocks[BlockSpawner.Instance.GetActivatedBlocksCount()])
                    {
                        GameState.Instance.FinishGame();
                        _collider.enabled = false;
                        return;
                    }
                }
                PlayerScore.Instance.AddScore(1);

                //collision.gameObject.GetComponent<Block>().UpdateTouches();
                Jump();
            }
        }
    }
}
