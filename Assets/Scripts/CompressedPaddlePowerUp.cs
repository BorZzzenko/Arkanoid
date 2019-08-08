using UnityEngine;
using System;

//Бонус Сжатие платформы
public class CompressedPaddlePowerUp : ExpandPaddlePowerUp {
    
    //Текущая платформа удаляется и загружается новая, в зависимости от имени старой, в том же месте
    public override void GetBonus() {
        String name = paddle.name;
        Vector3 position = paddle.transform.position;
        Quaternion rotation = paddle.transform.rotation;

        Destroy(paddle.gameObject);
        gameManager.CompressedPaddle(name, position, rotation);
    }
}
