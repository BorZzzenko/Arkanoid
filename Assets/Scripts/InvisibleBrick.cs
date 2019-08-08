using UnityEngine;

//Невидимый блок
public class InvisibleBrick : StrongBrick {
    private SpriteRenderer sprite;  //Спрайт объекта

    // Use this for initialization
    void Start () {
        healthsPoints = 2;
        points = 15;

        sprite = GetComponent<SpriteRenderer>();

        Color color = new Color(0.1589534f, 0.7169812f, 0.7169812f, 0);     //при старте спрайт прозрачный
        sprite.color = color;

        sound = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //Уничтожение блока
    //При первом попадании спрайт становится видимым
    //При втором попадании блок уничтожается
    public override void DestroyBrick() {
        gameManager.UpdateScore(points);
        healthsPoints--;

        Color newColor = new Color(0.1589534f, 0.7169812f, 0.7169812f, 255);
        sprite.color = newColor;

        sound.clip = Resources.Load<AudioClip>("Wowpulse");

        if (healthsPoints == 0) {
            gameManager.UpdateCountOfBricks();
            Instantiate(Resources.Load("ExplosionBlue"), transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    //Взаимодействие с врзрывающимся блоком
    public override void Explosion() {
        DestroyBrick();
        DestroyBrick();
    }
}
