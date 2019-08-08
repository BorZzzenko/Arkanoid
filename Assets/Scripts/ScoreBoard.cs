// Структура для хранения таблицы рекодов

public struct ScoreBoard {
    int highScore;
    string name;

    public int HighScore {
        get {
            return highScore;
        }
    }

    public string Name {
        get {
            return name;
        }
    }

    public ScoreBoard(int score, string name) {
        highScore = score;
        this.name = name;
    }
}
