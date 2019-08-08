using UnityEngine;

//Бонус замедления шара
public class SlowBallPowerUp : PowerUp {
    protected Ball ball;    //ссылка на шар

    new void Start() {
        base.Start();
        try {
            ball = GameObject.FindGameObjectWithTag("ball").GetComponent<Ball>();
        }
        catch {
            ball = GameObject.FindGameObjectWithTag("bonusBall").GetComponent<Ball>();
        }           
    }

    //-100 скорости
    public override void GetBonus() {
        ball.UpdateSpeed(-100);
    }
}
