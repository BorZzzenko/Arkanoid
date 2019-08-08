//Бонус смерть
using UnityEngine;

public class DeathPowerUp : SlowBallPowerUp {

    //Отнимается жизнь, сбрасываются бонусы, шар возвращается на платформу
    public override void GetBonus() {
        if(ball.InGame == true) {
            ball.DeathSound();
            ball.Restart();
        }
    }
}