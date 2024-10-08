using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class PlatformManager : MonoBehaviour
{
    public GameObject platformPrefab;
    public float platformSize=9f;
    public float disappearTime=5f;
    public float newSpawnTime=2.5f;
    public Text scoreText;

    private GameObject currPlatform;
    private GameObject nextPlatform;
    private Vector3 lastDirection;
    private int score=0;

    void Start(){
        score=0;
        UpdateScoreUI();
        SpawnInitialPlatform();
    }

    void SpawnInitialPlatform(){
        currPlatform=Instantiate(platformPrefab, Vector3.zero,Quaternion.identity);
        lastDirection = Vector3.zero;
        Invoke("SpawnNewPlatform",newSpawnTime);
    }

    void SpawnNewPlatform(){
        Vector3 newPosition=GetRandomDirection();
        nextPlatform=Instantiate(platformPrefab,newPosition,Quaternion.identity);
        lastDirection = newPosition - currPlatform.transform.position;
        Invoke("DestroyCurrPlatform",disappearTime);
    }

    void DestroyCurrPlatform(){
        float fallThreshold = currPlatform.transform.position.y - 1.0f;

        if (GameObject.FindWithTag("Player").transform.position.y < fallThreshold)
        {
            GameOver();
        }
        else
        {
            Destroy(currPlatform);
            currPlatform = nextPlatform;
            IncreaseScore();
            Invoke("SpawnNewPlatform", newSpawnTime);
        }
    }

    Vector3 GetRandomDirection(){
        Vector3[] directions={
            Vector3.right*platformSize,
            Vector3.left*platformSize,
            Vector3.forward*platformSize,
            Vector3.back*platformSize
        };
        Vector3[] validDirections = System.Array.FindAll(directions, dir => dir != lastDirection && dir != -lastDirection);
        Vector3 randomDirection = validDirections[Random.Range(0, validDirections.Length)];
        Vector3 newPosition = currPlatform.transform.position + randomDirection;    

        return newPosition;
    }
    
    void IncreaseScore()
    {
        score++;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score.ToString();
    }
    
    void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
