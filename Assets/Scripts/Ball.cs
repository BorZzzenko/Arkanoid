using UnityEngine;

//Объект Шар
public class Ball : MonoBehaviour {

    private static int speed = 400;     //Скорость шара и ее границы
    private int maxSpeed = 700;
    private int minSpeed = 300;

    private static bool inGame = false;     //В игре ли шары

    private Rigidbody2D rBody;
    private static Transform paddle;        //Ссылка на платформу
    private GameManager gameManager;
    private Brick brick;                    //Указатель на Кирпич

    public AudioSource sound;               //Звуки взаимодействия шара с платформой, границами
    private AudioClip paddleSound;
    private AudioClip edgeSound;
    private AudioClip deathSound;           //Звук смерти

    public int Speed {
        set {
            speed = value;
        }
        get {
            return speed;
        }
    }

    public bool InGame {
        set {
            inGame = value;
        }
        get {
            return inGame;
        }
    }
    

	// Use this for initialization
	void Start () {
        paddle = GameObject.FindGameObjectWithTag("ballPosition").transform;
        gameManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();

        rBody = GetComponent<Rigidbody2D>();

        sound = GetComponent<AudioSource>();
        paddleSound = Resources.Load<AudioClip>("Boing");
        edgeSound = Resources.Load<AudioClip>("Bassdrum");
        deathSound = Resources.Load<AudioClip>("Padexplo");

        if (gameManager.CountOfBalls > 1) {
            rBody.AddForce((Vector2.up + Vector2.right).normalized * speed);
        }
	}
	
	// Update is called once per frame
	void Update () {
        //Если платформа уничтожена,обновляется ссылка на нее
        if(paddle == null) {
            UpdatePaddle();
        }

        //Удаляются бонусные шары при рестарте
        if (inGame == false) {
            if (tag == "mainBall") {
                tag = "ball";
            }
            if (tag != "ball") {
                Destroy(gameObject);
            }
        }

        if(gameManager.IsGameOver == true) {
            rBody.velocity = Vector2.zero;
            return;
        }

        //Шар на платформе, пока он не в игре
        if (inGame == false && gameManager.CountOfBalls == 0) {
            transform.position = paddle.position;
        }

        //По нажатию на ЛКМ шар запускается в игру
        if (Input.GetMouseButtonDown(0) && inGame == false) {
            gameManager.CountOfBalls = 1;
            rBody.velocity = Vector2.zero;
            speed = 400;
            inGame = true;
            rBody.AddForce((Vector2.up + Vector2.right).normalized * speed);
        }
    }

    //Функция взаимодествия шара с обьектами TriggerCollider
    void OnTriggerEnter2D(Collider2D collider) {
       
        //Нижняя граница
        if (collider.CompareTag("bottomEdge")) {
            rBody.velocity = Vector2.zero;
            gameManager.CountOfBalls--;
           
            if (gameManager.CountOfBalls == 0) {
                Debug.Log("Restart");
                sound.PlayOneShot(deathSound, 0.7f);
                Restart();
            }
            else {
                if (tag == "bonusBall") {
                    Destroy(this.gameObject);
                }
                else {
                    if (tag == "ball") {
                        
                        gameObject.tag = "mainBall";
                    }
                }
            }
        }

        //Платформа
        if (collider.CompareTag("MidSidePaddle")) {
            rBody.velocity = Vector2.zero;
            float x = HitFactor(transform.position, collider.transform.position, collider.bounds.size.x);
            Vector2 vector = new Vector2(x, 1).normalized;
            UpdateSpeed(+2);
            rBody.AddForce(vector * speed);

            sound.PlayOneShot(paddleSound, 0.7f);
        }
    }

    //Функция взаимодествия шара с обьектами Collider
    void OnCollisionEnter2D(Collision2D collision) {
        
        //Блоки
        if(collision.transform.CompareTag("brick") || collision.transform.CompareTag("strongBrick")) {
            UpdateSpeed(0);
            brick = collision.gameObject.GetComponent<Brick>();
            gameManager.GetPowerUps(collision.transform.position, collision.transform.rotation);
            sound.PlayOneShot(brick.sound.clip, 1);
            brick.DestroyBrick();
        }

        //Границы экрана
        if (collision.transform.CompareTag("screenEdge")) {
            UpdateSpeed(+1);
            sound.PlayOneShot(edgeSound, 0.7f);
        }

        //С другими шарами
        if (collision.transform.CompareTag("ball") || collision.transform.CompareTag("bonusBall")) {
            UpdateSpeed(0);
            sound.PlayOneShot(paddleSound, 0.7f);
        }
    }
    
    //Вычисление координаты Х отскока шара от платформы
    float HitFactor(Vector2 ballPosition, Vector2 paddlePosition, float paddleWidth) {
        return (ballPosition.x - paddlePosition.x) / paddleWidth;
    }

    //Обновление скорости
    public void UpdateSpeed(int changeInSpeed) {
        if ((changeInSpeed > 0 && speed < maxSpeed) || (changeInSpeed < 0 && speed > minSpeed)) {
            speed += changeInSpeed;

            if(speed < minSpeed) {
                speed = minSpeed;
            }
            else {
                if (speed > maxSpeed) {
                    speed = maxSpeed;
                }
            }

            Vector2 vec = rBody.velocity;
            rBody.velocity = Vector2.zero;
            rBody.AddForce(vec.normalized * speed);
        }
    }

    //Сброс всех бонусов, возвращение шара на платформу
    public void Restart() {
        inGame = false;
        gameManager.CountOfBalls = 0;
        gameManager.UpdateLives(-1);

        Paddle pad = GameObject.FindGameObjectWithTag("paddle").GetComponent<Paddle>();
        Instantiate(Resources.Load("Paddle"), pad.transform.position, pad.transform.rotation);
        Destroy(pad.gameObject);

        Debug.Log("Count of Balls: " + gameManager.CountOfBalls);
    }

    //Обновление ссылки на Платформу
    public void UpdatePaddle() {
        paddle = GameObject.FindGameObjectWithTag("ballPosition").transform;
    }

    public void DeathSound() {
        sound.PlayOneShot(deathSound, 0.7f);
    }
}
