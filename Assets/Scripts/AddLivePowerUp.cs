using UnityEngine;

//Бонус +1 жизнь
public class AddLivePowerUp : PowerUp {

    public override void GetBonus() {
        gameManager.UpdateLives(+1);
    }
}
