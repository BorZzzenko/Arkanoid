using UnityEngine;

//Анимация уничтожения блока
public class ExplosionAnimation : MonoBehaviour {

    //После 1 цикла анимации она уничтожается
    public void destroyObject() {
        Destroy(gameObject);
    }
}
