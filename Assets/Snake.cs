using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class SnakeMovement : MonoBehaviour
{
    public float speed = 0.1f;
    public float updateRate = 0.1f;
    private float updateTime = 0.1f;
    private Vector2 direction = Vector2.zero;
    private bool gameStarted = false; // Track whether the game has started
    private List<Transform> body; 
    public int Score = 0;
    public Text ScoreBoard;
    public Transform BodyPrefab;

    // public int initialSize = 4;
    private void Start(){
        body = new List<Transform>();
        body.Add(this.transform);
        if (ScoreBoard == null)
        {
            Debug.LogError("Score Text is not assigned!");
        }
        else
        {
            // Update the score display initially
            UpdateScoreBoard();
        }
    }

    private void UpdateScoreBoard(){
        if (ScoreBoard != null)
        {
            ScoreBoard.text = "Score: " + Score.ToString();
        }
    }

    private void CheckStartGame()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            direction = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            direction = Vector2.right;
        }
        if (direction != Vector2.zero)
        {
            gameStarted = true;
        }
    }

    void Update()
    {
        if(!gameStarted){
            CheckStartGame();
            return;
        }
        else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && direction != Vector2.down)
            direction = Vector2.up;
        else if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && direction != Vector2.up)
            direction = Vector2.down;
        else if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && direction != Vector2.right)
            direction = Vector2.left;
        else if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && direction != Vector2.left)
            direction = Vector2.right;
    }
    private void FixedUpdate(){
        if(!gameStarted){
            return;
        }
        for(int i = body.Count - 1; i > 0; i--){
            body[i].position = body[i-1].position;
        }
        this.transform.position = new Vector3(Mathf.Round(this.transform.position.x+ direction.x), Mathf.Round(this.transform.position.y+ direction.y), 0.0f);
    }

    private void Grow()
    {
        Transform segment = Instantiate(this.BodyPrefab);
        segment.position = body[body.Count - 1].position;
        body.Add(segment);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Food")
        {
            Score++;
            UpdateScoreBoard();
            Grow();
        }
        else if(other.tag == "Wall" || other.tag == "Player"){
            ResetGame();
        }
    }
    private void ResetGame(){
        gameStarted = false;
        direction = Vector2Int.zero;
        transform.position = Vector3.zero;
        Score = 0;
        for (int i = 1; i < body.Count; i++) {
            Destroy(body[i].gameObject);
        }
        body.Clear();
        body.Add(transform);
        UpdateScoreBoard();
    }
}
