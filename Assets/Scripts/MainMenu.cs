using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Объект главное меню
public class MainMenu : MonoBehaviour {

    private List<ScoreBoard> scoreBoard = new List<ScoreBoard>();

    private Text[] scoreBoardLines;
    private GameObject scoreBoardPanel;

    private GameObject loadPanel;

    void Start() {
        Cursor.visible = true;

        loadPanel = GameObject.FindGameObjectWithTag("LoadPanel");
        loadPanel.SetActive(false);

        scoreBoardPanel = GameObject.FindGameObjectWithTag("scoreBoardPanel");
        scoreBoardLines = scoreBoardPanel.GetComponentsInChildren<Text>();

        MakeScoreBoard();
    }

    //Таблица рекордов считывается из файла и выводится на экран
    public void MakeScoreBoard() {
        //Считывание таблицы рекордов из файла
        if (!File.Exists("ScoreBoard.dat")) {
            File.Create("ScoreBoard.dat");
        }

        string[] str = File.ReadAllLines("ScoreBoard.dat");

        if (str.Length == 0) {
            Debug.Log("Файл пуст");
            for (int i = 0; i < 5; i++) {
                scoreBoard.Add(new ScoreBoard(0, "None"));
            }
        }
        else {
            for (int i = 0; i < 5; i++) {
                if (i < str.Length) {
                    string[] words = str[i].Split(' ');

                    scoreBoard.Add(new ScoreBoard(Convert.ToInt32(words[1]), words[0]));
                }
                else {
                    scoreBoard.Add(new ScoreBoard(0, "None"));
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
    }

    //Кнопка выход из игры
    public void Quit() {
        Application.Quit();
    }

    //Кнопка начать игру
    public void StartGame() {
        loadPanel.SetActive(true);
        SceneManager.LoadScene("SampleScene");
    }

    //Кнопка таблица рекордов
    public void LookScoreBoard() {
        loadPanel.SetActive(true);
        SceneManager.LoadScene("ScoreBoard");
    }

    //Кнопка возврат в главное меню
    public void LoadMenu() {
        loadPanel.SetActive(true);
        SceneManager.LoadScene("MainMenu");
    }
}
