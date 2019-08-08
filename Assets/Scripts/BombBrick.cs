using UnityEngine;

//Взрывающийся блок
public class BombBrick : Brick {
    private bool isDestroyed = false;   //Уничтожен ли блок?
    private BoxCollider2D boxCollider;  //Триггер колайдер, для взаимодействия с остальными блоками
    private int time = 0;

    // Use this for initialization
    void Start() {
        points = 100;
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;          //В начале коллайдер отключен

        sound = GetComponent<AudioSource>();
    }

    void Update() {
        if (isDestroyed) {
            time++;
            boxCollider.enabled = true;     //При попадании шарика коллайдер включается и взаимодействует с соседними блоками
            //Через несколько секунд блок уничтожается
            if(time > 10) {
                Instantiate(Resources.Load("Explosion"), transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }

    //Уничтожение блока
    public override void DestroyBrick() {
        isDestroyed = true;
        gameManager.UpdateScore(points);
        gameManager.UpdateCountOfBricks();
    }

    //Взаимодействие с соседними блоками
    public void OnTriggerStay2D(Collider2D collider) {
        if (collider.CompareTag("brick")  && isDestroyed) {
            collider.gameObject.GetComponent<Brick>().DestroyBrick();
            Instantiate(Resources.Load("Explosion"), transform.position, transform.rotation);
            Destroy(gameObject);
        }
        if(collider.CompareTag("strongBrick") && isDestroyed) {
            collider.gameObject.GetComponent<StrongBrick>().Explosion();
            Instantiate(Resources.Load("Explosion"), transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
};
