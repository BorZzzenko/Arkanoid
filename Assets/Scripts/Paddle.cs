using UnityEngine;

//Объект платформа
public class Paddle : MonoBehaviour {

    private float speed = 3;        //Скорость

    public float rightScreenEdge;   //Границы экрана
    public float leftScreenEdge;

    private GameManager gameManager;
    private PowerUp powerUp;
    private AudioSource sound;

    // Use this for initialization
    void Start () {
        gameManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();

        sound = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        if (gameManager.IsGameOver == true) {
            return;
        }

        float horizontal = Input.GetAxis("Mouse X");

        transform.Translate(Vector2.right * horizontal * Time.deltaTime * speed);

       if(transform.position.x < leftScreenEdge) {
            transform.position = new Vector2(leftScreenEdge, transform.position.y);
        }

        if (transform.position.x > rightScreenEdge) {
            transform.position = new Vector2(rightScreenEdge, transform.position.y);
        }
    }

    //Взаимодейтсвие с бонусами
    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("powerUp")) {
            powerUp = collider.gameObject.GetComponent<PowerUp>();
            powerUp.GetBonus();
            Destroy(collider.gameObject);
            sound.PlayOneShot(powerUp.sound.clip, 0.7f);
        }
    }
}
