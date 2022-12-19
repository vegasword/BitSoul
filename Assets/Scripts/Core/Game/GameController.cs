using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    private        bool playerWon;
    private static bool firstSpawn;

    // Event mangament related properties.
    [HideInInspector] public UnityEvent playerIsDead;
    [HideInInspector] public UnityEvent bossIsDead;

    public        Vector3 playerSpawn;
    public static Vector3 playerRespawn;

    public GameObject Player;
    public GameObject Camera;

    void Start()
    {
        // Application setup.
        Application.targetFrameRate = 60;

        // Player and camera spawn initialization.
        if (!firstSpawn)
        {
            playerRespawn = playerSpawn;
            Player.transform.position = playerSpawn;
            Camera.transform.position = new Vector3(playerSpawn.x, playerSpawn.y, Camera.transform.position.z);
            firstSpawn = true;
        }
        else
        {
            Player.transform.position = playerRespawn;
            Camera.GetComponent<MyCameraController>().InitPosition(new Vector3(playerRespawn.x, playerRespawn.y, Camera.transform.position.z));
        }

        // Events subscribing.
        if (playerIsDead == null) playerIsDead = new UnityEvent();
        if (playerIsDead == null) bossIsDead   = new UnityEvent();
        
        playerIsDead.AddListener(ReloadScene);
        bossIsDead.AddListener(LevelEnd);
    }

    void Update() 
    {
        if (playerWon) Player.GetComponent<PlayerHealth>().playerHealed.Invoke();
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

    public void SetRespawnPoint(Vector3 pos) { playerRespawn = pos; }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LevelEnd()
    {
        firstSpawn = false;
        playerWon  = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
