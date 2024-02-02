using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    public GameObject panelGameOver, logoBenar, logoSalah;
    public List<QuestionAndAnswer> QnA;
    public GameObject[] options;
    public int currentQuestion;

    public float sisaWaktu;
    public bool timerNyala = true;

    public Image imageQuestion;
    public Text scoreText, scoreTextGameOver;
    public Text timer;

    int score = 0;
    float waitTime = 1f;

    private void Start()
    {
        Time.timeScale = 1f;
        panelGameOver.SetActive(false);
        timerNyala = true;
        GenerateQuestion();
    }

    private void Update()
    {
        if(timerNyala)
        {
            if(sisaWaktu > 0)
            {
                sisaWaktu -= Time.deltaTime;
                updateTimer(sisaWaktu);
            }
            else
            {
                sisaWaktu = 0;
                timerNyala = false;
                GameOver();
            }
        }
    }

    void updateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        timer.text = string.Format("{0:00} : {1:00}", minutes, seconds);

    }

    public void Correct()
    {
        score += 1;
        StartCoroutine(ShowLogo(logoBenar));
        scoreText.text = score.ToString();
        QnA.RemoveAt(currentQuestion);
        GenerateQuestion();
    }
    public void GameOver()
    {
        Time.timeScale = 0f;
        panelGameOver.SetActive(true);
        scoreTextGameOver.text = "Score: " + scoreText.text;
    }

    public void Wrong()
    {
        StartCoroutine(ShowLogo(logoSalah));
        QnA.RemoveAt(currentQuestion);
        GenerateQuestion();
    }

    void SetAnswer()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<Text>().text = QnA[currentQuestion].answer[i];

            if (QnA[currentQuestion].correctAnswer == i + 1)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }
    }

    void GenerateQuestion()
    {
        if(QnA.Count>0)
        {
            currentQuestion = Random.Range(0, QnA.Count);

            imageQuestion.sprite = QnA[currentQuestion].question;
            SetAnswer();
        }
        else
        {
            Debug.Log("Out Of Question");
            GameOver();
        }
        

        
    }
    IEnumerator ShowLogo(GameObject logo)
    {
        logo.SetActive(true);
        yield return new WaitForSeconds(waitTime);
        logo.SetActive(false);

    }
}
