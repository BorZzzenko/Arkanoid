using UnityEngine;

//Крепкий блок
public class StrongBrick : Brick {
    protected int healthsPoints;    //Количество жизней блока
    private Sprite hitSprite;       //Спрайт после первого попадания

	// Use this for initialization
	void Start () {
        healthsPoints = 2;
        points = 40;
        hitSprite = Resources.Load<Sprite>("element_grey_rectangle_strong");

        sound = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //Уничтожение блока
    //При первом попадании меняется спрайт
    //При втором блок уничтожается
    public override void DestroyBrick() {
        gameManager.UpdateScore(points);
        healthsPoints--;

        GetComponent<SpriteRenderer>().sprite = hitSprite;

        sound.clip = Resources.Load<AudioClip>("Wowpulse");

        if (healthsPoints == 0) {
            gameManager.UpdateCountOfBricks();
            Instantiate(Resources.Load("ExplosionGreen"), transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    //Взаимодействие с взрывающимся блоком
    public virtual void  Explosion() {
        DestroyBrick();
        DestroyBrick();
    }
}
