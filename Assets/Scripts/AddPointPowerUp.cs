
//Бонус +100 очков
public class AddPointPowerUp : PowerUp {

    public override void GetBonus() {
        gameManager.UpdateScore(+100);
    }
}
