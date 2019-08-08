using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using System.Text.RegularExpressions;

//Объект для управления всеми объектами на сцене
public class GameManager : MonoBehaviour {

    private int lives = 2;      //Количество жизней
    private int score = 0;      //Количество очков

    private bool isGameOver = false;    //Конец игры
    private bool isEnterName = false;   //Введено ли имя

    private Text livesText;     //Ссылка на поле для вывод количества жизней
    private Text scoreText;     //Ссылка на поле для вывод количества очков

    private Text gameOverMessage;   //Сообщение в конце игры

    private GameObject gameOverPanel;   //Панель конца игры

    private int countOfBricks;          //Текущее количество блоков на уровне
    private int countOfBalls = 0;       //Количество шаров в игре

    private ScoreBoard[] scoreBoard = new ScoreBoard[6];    //Таблица рекордов

    private string playerName = "";         //Имя игрока
    private GameObject enterNamePanel;      //Панель для ввода имени
    private InputField inputNameField;      //Текстовое поле для ввода имени

    private GameObject scoreBoardPanel;     //Панель таблицы рекордов
    private Text[] scoreBoardLines;         //Массив текстовых полей в панели таблицы рекодов

    private Ball ball;                      //Ссылка на шар

    private AudioClip expandPaddleSound;        //Звук расширени платформы
    private AudioClip compressedPaddleSound;    //Звук сжатия платформы

    private int levelsCount = 5;                //Количество уровней
    private int currentLevelIndex = 0;          //Номер текущего уровня

    private bool isPause = false;
    private GameObject pausePanel;

    private GameObject loadPanel;

    public int CountOfBalls {
        get {
            return countOfBalls;
        }
        set {
            countOfBalls = value;
        }
    }

    public bool IsGameOver {
        get {
            return isGameOver;
        }
    }

    // Use this for initialization
    void Start() {
        Cursor.visible = false;

        ball = GameObject.FindGameObjectWithTag("ball").GetComponent<Ball>();

        pausePanel = GameObject.FindGameObjectWithTag("pausePanel");

        gameOverMessage = GameObject.FindGameObjectWithTag("gameOverMes").GetComponent<Text>();
        gameOverPanel = GameObject.FindGameObjectWithTag("gameOverPanel");

        enterNamePanel = GameObject.FindGameObjectWithTag("enterNamePanel");
        inputNameField = GameObject.FindGameObjectWithTag("inputNameField").GetComponent<InputField>();

        scoreBoardPanel = GameObject.FindGameObjectWithTag("scoreBoardPanel");
        scoreBoardLines = scoreBoardPanel.GetComponentsInChildren<Text>();

        loadPanel = GameObject.FindGameObjectWithTag("LoadPanel");


        loadPanel.SetActive(false);
        pausePanel.SetActive(false);
        enterNamePanel.SetActive(false);
        scoreBoardPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        livesText = GameObject.FindGameObjectWithTag("livesText").GetComponent<Text>();
        livesText.text = "Lives: " + lives;

        scoreText = GameObject.FindGameObjectWithTag("scoreText").GetComponent<Text>();
        scoreText.text = "Score: " + score;

        countOfBricks = GameObject.FindGameObjectsWithTag("brick").Length + GameObject.FindGameObjectsWithTag("strongBrick").Length;
        Debug.Log("" + countOfBricks);

        expandPaddleSound = Resources.Load<AudioClip>("Effect2");
        compressedPaddleSound = Resources.Load<AudioClip>("Sweepdow");

        GetHighScores();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && isGameOver == false) {
            if (!isPause) {
                Cursor.visible = true;
                Time.timeScale = 0;
                pausePanel.SetActive(true);
                isPause = true;
            }
            else {
                Cursor.visible = false;
                Time.timeScale = 1;
                pausePanel.SetActive(false);
                isPause = false;
            }
        }
    }

    //Обновление жизней
    public void UpdateLives(int changeInLives) {
        lives += changeInLives;

        //если жизни кончились, то конец игры
        if (lives <= 0) {
            lives = 0;
            GameOver();
        }
        else if (lives > 5) {
            lives = 5;
        }

        livesText.text = "Lives: " + lives;
    }

    //обновление очков
    public void UpdateScore(int points) {
        score += points;
        scoreText.text = "Score: " + score;
    }

    //Обновление количество блоков
    public void UpdateCountOfBricks() {
        countOfBricks--;

        //Если блоки кончились, загружается следующий уровень
        if(countOfBricks <= 0) {
            if(currentLevelIndex >= levelsCount - 1) {
                GameOver();
            }
            else {
                
                Invoke("LoadLevel", 1f);
                
            }
        }
    }

    //Загрузка следующего уровня
    public void LoadLevel() {
        Debug.Log("Loading lvl...");

        int n = GameObject.FindGameObjectsWithTag("brick").Length + GameObject.FindGameObjectsWithTag("strongBrick").Length;
         
        if (n == 0) {
            ball.InGame = false;
            countOfBalls = 0;
            Paddle paddle = GameObject.FindGameObjectWithTag("paddle").GetComponent<Paddle>();
            Instantiate(Resources.Load("Paddle"), paddle.transform.position, paddle.transform.rotation);
            Destroy(paddle.gameObject);

            
            Invoke("Loadlvl", 1f);
        }
        else {
            countOfBricks = GameObject.FindGameObjectsWithTag("brick").Length + GameObject.FindGameObjectsWithTag("strongBrick").Length;
        }     
    }

    private void Loadlvl() {
        isGameOver = true;
        currentLevelIndex++;
        GameObject lvl = GameObject.FindGameObjectWithTag("level");
        Destroy(lvl);
        Instantiate(Resources.Load("Level " + currentLevelIndex), Vector2.zero, Quaternion.identity);
        countOfBricks = GameObject.FindGameObjectsWithTag("brick").Length + GameObject.FindGameObjectsWithTag("strongBrick").Length;
        isGameOver = false;
    }

    //Конец игры, появляется панель конца игры
    public void GameOver() {
        Cursor.visible = true;
        isGameOver = true;
        gameOverPanel.SetActive(true);

        gameOverMessage.text = "You lose! Your score: ";

        if (lives <= 0) {
            gameOverMessage.text = "You lose! Your score: " + score.ToString();
        }
        else {
            gameOverMessage.text = "You win! Your score: " + score.ToString();
        }

        //Если установлен рекорд, вводится имя игрока, выводится таблица рекордов
        if (score > scoreBoard[4].HighScore) {
            enterNamePanel.SetActive(true);
            isEnterName = false;
        }
        else {
            scoreBoardPanel.SetActive(true);
            CreateScoreBoard();
            isEnterName = true;
        }
    }

    //Проверка на корректность имени
    public void EnterName() {
        playerName = inputNameField.text;

        bool isLatin = !Regex.IsMatch(playerName, "^[А-Яа-я]+$");
        bool isCorrect = playerName.IndexOf(' ') == -1;

        if (playerName != "" && isLatin && isCorrect) {
            enterNamePanel.SetActive(false);
            scoreBoardPanel.SetActive(true);
            CreateScoreBoard();
            isEnterName = true;
        }
        if(!isCorrect)
            inputNameField.text = "Error! Contains a space";

        if (!isLatin) {
            inputNameField.text = "Error! Invalid character entered";
        }
        
    }

    //Загружается 1й уровень
    public void PlayAgain() {
        if(isEnterName) {
            loadPanel.SetActive(true);
            ball.Restart();
            SceneManager.LoadScene("SampleScene");
        }
    }

    public void LoadM() {
        ball.Restart();
        loadPanel.SetActive(true);
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        isPause = false;
        SceneManager.LoadScene("MainMenu");
    }

    //Загружается главное меню
    public void LoadMenu() {
        if (isEnterName) {
            loadPanel.SetActive(true);
            ball.Restart();
            SceneManager.LoadScene("MainMenu");
        }
    }

    //Приложение закрывается
    public void Quit() {
        Application.Quit();
    }

    //Создание бонуса
   public void GetPowerUps(Vector3 position, Quaternion rotation) {
        int randChance = UnityEngine.Random.Range(1, 101);

        //Доп очки
        if (randChance > 79 && randChance < 90 ) {
            position.z = -2;
            Instantiate(Resources.Load("PowerUp_5"), position, rotation);
        }

        //Доп жизнь
        else if (randChance > 49 && randChance < 53) {
            position.z = -2;
            Instantiate(Resources.Load("PowerUp_6"), position, rotation);
        }

        //Смерть
        else if (randChance > 53 && randChance < 57) {
            position.z = -2;
            Instantiate(Resources.Load("PowerUp_7"), position, rotation);
        }

        //Замедление шара
        else if (randChance > 0 && randChance < 4) {
            position.z = -2;
            Instantiate(Resources.Load("PowerUp_1"), position, rotation);
        }

        //Ускорение шара
        else if (randChance > 5 && randChance < 9) {
            position.z = -2;
            Instantiate(Resources.Load("PowerUp_0"), position, rotation);
        }

        //Расширение платформы
        else if (randChance > 19 && randChance < 23) {
            position.z = -2;
            Instantiate(Resources.Load("PowerUp_3"), position, rotation);
        }

        //Уменьшение платформы
        else if (randChance > 29 && randChance < 33) {
            position.z = -2;
            Instantiate(Resources.Load("PowerUp_4"), position, rotation);
        }

        //Дополнительный шар
        else if (randChance > 13 && randChance < 17) {
            position.z = -2;
            Instantiate(Resources.Load("PowerUp_2"), position, rotation);
        }
    }

    //Загрузка расширенной платформы
    public void ExpandPaddle(String name, Vector3 position, Quaternion rotation) {
        if(name == "SmallPaddle(Clone)") {
            Instantiate(Resources.Load("Paddle"), position, rotation);
            ball.sound.PlayOneShot(expandPaddleSound, 0.7f);
        }
        else {
            Instantiate(Resources.Load("LargePaddle"), position, rotation);
            ball.sound.PlayOneShot(expandPaddleSound, 0.7f);
        }
    }

    //Загрузка уменьшенной платформы
    public void CompressedPaddle(String name, Vector3 position, Quaternion rotation) {
        if (name == "LargePaddle(Clone)") {
            Instantiate(Resources.Load("Paddle"), position, rotation);
            ball.sound.PlayOneShot(compressedPaddleSound, 0.7f);
        }
        else {
            Instantiate(Resources.Load("SmallPaddle"), position, rotation);
            ball.sound.PlayOneShot(compressedPaddleSound, 0.7f);
        }
    }

    //Считывание таблицы рекордов в структуру ScoreBoards
    public void GetHighScores() {
        //Считывание таблицы рекордов из файла
        if (!File.Exists("ScoreBoard.dat")) {
            File.Create("ScoreBoard.dat");
        }

        string[] str = File.ReadAllLines("ScoreBoard.dat");

        if (str.Length == 0) {
            Debug.Log("Файл пуст");
            for (int i = 0; i < 5; i++) {
                scoreBoard[i] = new ScoreBoard(0, "None");
            }
        }
        else {
            for (int i = 0; i < 5; i++) {
                if (i < str.Length) {
                    string[] words = str[i].Split(' ');

                    scoreBoard[i] = new ScoreBoard(Convert.ToInt32(words[1]), words[0]);
                }
                else {
                    scoreBoard[i] = new ScoreBoard(0, "None");
                }
            }
        }
    }

    //Сортировка таблицы с новым результатом и вывод на экран
    public void CreateScoreBoard() {
        scoreBoard[5] = new ScoreBoard(score, playerName);

        //Сортировка таблицы рекордов
        for (int k = 0; k < 6; k++) {
            for (int i = 0; i < 5 - k; i++) {
                if (scoreBoard[i].HighScore < scoreBoard[i + 1].HighScore) {
                    ScoreBoard temp = scoreBoard[i];
                    scoreBoard[i] = scoreBoard[i + 1];
                    scoreBoard[i + 1] = temp;
                }
            }
        }

        //Вывод таблицы в ScoreBoardPanel
        for (int i = 0; i < 5; i++) {
            scoreBoardLines[i * 2].text = scoreBoard[i].Name;
        }
        for (int i = 0; i < 5; i++) {
            scoreBoardLines[i * 2 + 1].text = scoreBoard[i].HighScore.ToString();
        }

        //Сохранение таблицы в файл
        StreamWriter sw = new StreamWriter("ScoreBoard.dat");
        for (int i = 0; i < 5; i++) {
            sw.WriteLine(scoreBoard[i].Name+ " " + scoreBoard[i].HighScore);
        }
        sw.Close();
    }
}
