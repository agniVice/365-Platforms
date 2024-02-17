using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour, ISubscriber
{
    public static BlockSpawner Instance;

    [SerializeField] private GameObject[] _blockPrefabs;
    [SerializeField] private Transform[] _blockPositions;

    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;

    [SerializeField] private float _minBlockSpeed;
    [SerializeField] private float _maxBlockSpeed;

    public List<Block> _blocks = new List<Block>();

    private int _blocksActivated = -1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    private void Start()
    {
        SpawnBlocks();
    }
    public void SubscribeAll()
    {
        GameState.Instance.LevelAdded += SpawnBlocks;
        PlayerInput.Instance.PlayerMouseDown += ActivateBlock;
    }

    public void UnsubscribeAll()
    {
        GameState.Instance.LevelAdded -= SpawnBlocks;
        PlayerInput.Instance.PlayerMouseDown -= ActivateBlock;
    }
    public void SpawnBlocks()
    {
        ClearBlocks();

        int blockCoinId = -1;
        if (LevelHasCoin())
            blockCoinId = GetBlockIdWithCoin();

        for (int i = 0; i < _blockPositions.Length; i++)
        {
            bool hasCoin = false;
            if (blockCoinId == i)
                hasCoin = true;

            var block = Instantiate(_blockPrefabs[Random.Range(0, _blockPrefabs.Length)], new Vector3(Random.Range(_minX, _maxX), _blockPositions[i].position.y, 0), Quaternion.identity).GetComponent<Block>();
            block.Initialize(GetRandomBlockMovementSpeed(), hasCoin, this);
            _blocks.Add(block.GetComponent<Block>());
        }
    }
    public void OnBlockStopped(Block block)
    {
        if (block == _blocks[_blocks.Count - 1])
        {
            GameState.Instance.LevelRebuilded?.Invoke();
        }
    }
    private void ClearBlocks()
    {
        foreach (var item in _blocks)
            Destroy(item.gameObject);

        _blocks.Clear();
        _blocksActivated = -1;
    }
    private float GetRandomBlockMovementSpeed()
    {
        return Random.Range(_minBlockSpeed, _maxBlockSpeed) + PlayerScore.Instance.Level;
    }
    private bool LevelHasCoin()
    {
        float random = Random.Range(0f, 100f);
        if (random >= 40)
            return true;
        else
            return false;
    }
    private int GetBlockIdWithCoin()
    {
        return Random.Range(0, _blocks.Count);
    }
    public void ActivateBlock()
    {
        if (_blocksActivated < _blocks.Count-1)
        {
            _blocksActivated++;
            _blocks[_blocksActivated].OnBlockActivated();
        }
    }
    public Vector2 GetCurrentBlockPosition()
    {
        return _blockPositions[_blocksActivated].transform.position;    
    }
    public int GetActivatedBlocksCount()
    {
        return _blocksActivated;
    }
    public float GetLeftLimit()
    {
        return _minX;
    }
    public float GetRightLimit()
    {
        return _maxX;
    }
}
