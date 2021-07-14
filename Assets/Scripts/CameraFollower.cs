using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    Camera cam;
    [SerializeField] Transform _playerRoot;
    [SerializeField] float _offset = 70f; // Distance between player and camera on z axis.
    private void Start() 
    {
        cam = FindObjectOfType<Camera>();    
    }
    private void Update() 
    {
        switch (GameManager.instance.CurrentGameState)
            {
                case GameManager.GameState.Prepare:
                
                    break;
                case GameManager.GameState.MainGame:
                CamFollow();     // Camera follow player on main game state only.          
                    break;
                case GameManager.GameState.FinishGame:   
                                     
                    break;
            }
    }
    void CamFollow()
    {
        var camPosZ = _playerRoot.position.z - _offset;
        cam.transform.position =  new Vector3(0, cam.transform.position.y, camPosZ);
    }
}
