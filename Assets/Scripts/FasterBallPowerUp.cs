//Бонус ускорение шара
public class FasterBallPowerUp : SlowBallPowerUp {
    //+100 к скорости
    public override void GetBonus() {
        
        ball.UpdateSpeed(+100);
    }
}
