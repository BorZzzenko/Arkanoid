using UnityEngine;

//Бонус дополнительный шар
public class AddBallPowerUp : PowerUp {
    protected Ball ball;

    new void Start() {
        base.Start();

        //Получение ссылки на основной шар
        try {
            ball = GameObject.FindGameObjectWithTag("ball").GetComponent<Ball>();
        }
        catch {
            ball = GameObject.FindGameObjectWithTag("bonusBall").GetComponent<Ball>();
        }
    }

    public override void GetBonus() {
        //Создание бонусного шара в месте, где находится основной шар
        if (gameManager.CountOfBalls < 3 && ball.InGame) {
            Instantiate(Resources.Load("BonusBall"), ball.transform.position, ball.transform.rotation);
            gameManager.CountOfBalls++;
        }
    }
}
