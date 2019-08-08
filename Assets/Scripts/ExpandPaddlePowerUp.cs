using UnityEngine;
using System;

//Бонус расширение платформы
public class ExpandPaddlePowerUp : PowerUp {

    protected Paddle paddle;       //Ссылка на платформу

    new void Start() {
        base.Start();
        paddle = GameObject.FindGameObjectWithTag("paddle").GetComponent<Paddle>();
        Debug.Log("paddle name: " + paddle.name);
    }

    //Текущая платформа удаляется и загружается новая, в зависимости от имени старой, в том же месте
    public override void GetBonus() {
        String name = paddle.name;
        Vector3 position = paddle.transform.position;
        Quaternion rotation = paddle.transform.rotation;

        Destroy(paddle.gameObject);
        gameManager.ExpandPaddle(name, position, rotation);
    }
}
