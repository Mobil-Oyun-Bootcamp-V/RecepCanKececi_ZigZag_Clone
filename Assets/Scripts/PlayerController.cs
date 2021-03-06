using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // This region would much better with headers. TO DO...
    Rigidbody _rb;
    [SerializeField] Transform _playerRoot;
    [SerializeField] Transform _light;
    [SerializeField] float _radius;
    [SerializeField] float _lightOffset;
    [SerializeField] float _speed;
    [SerializeField] SpawnManager spawnManager;
    private int _direction = 0;
    private int _score = -1;
    private int _highScore;
    private int _collisionCount;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        spawnManager = FindObjectOfType<SpawnManager>();
        _highScore = PlayerPrefs.GetInt("highScore", 0);
    }
    void Update()
    {
        switch (GameManager.instance.CurrentGameState)
            {
                case GameManager.GameState.Prepare:
                GameStart();
                    break;
                case GameManager.GameState.MainGame:
                PlayerMove();        
                CollisionCheck();               
                    break;
                case GameManager.GameState.FinishGame:   
                FinishMove();                     
                    break;
            }
    }
    // With every click we increase the direction value and mod it to change the direction with if statements.
    void PlayerMove()
    {
        LightMove();
        if(Input.GetMouseButtonDown(0))
        {
            _direction++;
        }
        if(_direction % 2 == 0)
        {
            _playerRoot.Translate((Vector3.right + Vector3.forward) * Time.deltaTime * _speed);
        }
        if(_direction % 2 == 1)
        {
            _playerRoot.Translate((Vector3.left + Vector3.forward) * Time.deltaTime * _speed);
        }
    }
    void GameStart()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GameManager.instance.ToMain();
            UIManager.instance.GamePanel();
        }
    }
    // Checking the collisions. If there is no collision(from tiles) game is over.
    void CollisionCheck()
    {
        if(_collisionCount == 0)
        {
            GameManager.instance.ToGameOver();
            _rb.isKinematic = false;
            UIManager.instance.RetryPanel();
        }
    }
    // Collision value is increase with every trigger enter, and decrease with every exit for collision check.
    private void OnTriggerEnter(Collider other) 
    {
        _collisionCount++;
        if(other.gameObject.CompareTag("collect"))
        {
            _collisionCount--;
        }
        _score++;
        if(_score > _highScore)
        {
            _highScore = _score;
            PlayerPrefs.SetInt("highScore", _highScore);
        }
        UIManager.instance.ScoreUpdate(_score, _highScore);
        spawnManager.SpawnTiles();
    }
    private void OnTriggerExit(Collider other) 
    {
        _collisionCount--;
    }
    void FinishMove()
    {
        if(_direction % 2 == 0)
        {
            _playerRoot.Translate((Vector3.right + Vector3.forward) * Time.deltaTime * _speed);
        }
        if(_direction % 2 == 1)
        {
            _playerRoot.Translate((Vector3.left + Vector3.forward) * Time.deltaTime * _speed);
        }
    }
    // The light in the scene translates through this method. Position on x of last tile in overlapsphere
    // is the position of the lamp and transform of light is lerping to it.
    void LightMove()
    {
        Vector3 center = _playerRoot.position;
        Collider[] hitColliders = Physics.OverlapSphere(center, _radius);
        float posX = hitColliders[hitColliders.Length - 1].transform.position.x;
        float posZ = _playerRoot.position.z + _lightOffset;
        _light.position = Vector3.Lerp(_light.position, new Vector3(posX, _light.position.y, posZ), Time.deltaTime * 2);
    }
}
