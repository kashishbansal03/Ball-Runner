using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerScript : MonoBehaviour
{
    public float speed;

    private Vector3 dir;

    public GameObject ps;

    private bool isDead;

    public GameObject resetBtn;

    private int score = 0;

    public Text scoreText;

    public Animator gameOverAnim;

    public Text newHighScore;

    public Image background;

    public Text[] scoreTexts;

// what is ground for player
    public LayerMask whatIsGround;

    // indicates if the player is playing the game

    private bool isPlaying = false;

    // A reference to the contact Point
    public Transform contactPoint;
    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        dir = Vector3.zero;
       //  PlayerPrefs.DeleteAll();

    }

    // Update is called once per frame
    void Update()
    {
        if(!IsGrounded() && isPlaying) {
         // kill player!
                isDead = true;
                GameOver();
                resetBtn.SetActive(true);
                if(transform.childCount > 0) 
                {
                    transform.GetChild(0).transform.parent = null;
                }
        } 
        if(Input.GetMouseButtonDown(0) && !isDead)
         {
            isPlaying = true;
            score++;
           scoreText.text = score.ToString();
            if(dir == Vector3.forward) {
                dir = Vector3.left;
            }
            else 
            {
                dir = Vector3.forward;
            }
        }

        float amountToMove = speed * Time.deltaTime;

        transform.Translate(dir*amountToMove);

    }
    
    void OnTriggerEnter(Collider other) {
        if(other.tag == "Pickup") 
        {
            other.gameObject.SetActive(false);
            Instantiate(ps,transform.position,Quaternion.identity);
            score += 5;
            scoreText.text = score.ToString();
        }
    }

   /* void OnTriggerExit(Collider other)
    {
        if(other.tag == "Tile")
        {
            RaycastHit hit;

            Ray downRay = new Ray(transform.position, -Vector3.up);

            if(!Physics.Raycast(downRay, out hit))
            {
                // kill player!
                isDead = true;
                GameOver();
                resetBtn.SetActive(true);
                if(transform.childCount > 0) 
                {
                    transform.GetChild(0).transform.parent = null;
                }
                
            }
        }
    } */
    
    private void GameOver() 
    {
        gameOverAnim.SetTrigger("GameOver");

        scoreTexts[1].text = score.ToString();

        int bestScore = PlayerPrefs.GetInt("BestScore",0);

        if(score > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", score);
            newHighScore.gameObject.SetActive(true);
            background.color = new Color32(238, 163, 238, 255);
            foreach (Text txt in scoreTexts) 
            {
                txt.color = Color.white;
            }
        }

        scoreTexts[3].text = PlayerPrefs.GetInt("BestScore").ToString();
    }

    private bool IsGrounded()
    {
        Collider[] colliders = Physics.OverlapSphere(contactPoint.position , .5f, whatIsGround);

        for(int i = 0; i < colliders.Length; i++) 
        {
            if(colliders[i].gameObject != gameObject)
            {
                return true;
            }

        }
        return false;
    } 
}
