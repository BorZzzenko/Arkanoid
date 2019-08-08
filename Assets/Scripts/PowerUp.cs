using UnityEngine;

//Абстрактный класс бонус
public abstract class PowerUp : MonoBehaviour {

    protected static GameManager gameManager;
    public AudioSource sound;
    protected float speed = 5;          //Скорость

    // Use this for initialization
    protected void Start() {
        gameManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();

        sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        transform.Translate(new Vector2(0f, -1f) * Time.deltaTime * speed);     //Движется вертикально вниз

        if(transform.position.y < 8.36) {       //Если падает за пределы экраны, удаляется
            Destroy(gameObject);
        }
    }

    //Функция применения бонуса
    public abstract void GetBonus();
}