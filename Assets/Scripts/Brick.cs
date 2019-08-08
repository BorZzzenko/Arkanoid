using UnityEngine;

//Блок
public class Brick : MonoBehaviour {

    protected int points;           //Поле очки за уничтожение
    protected static GameManager gameManager;  //Ссылка на GameManager
    public AudioSource sound;       //звук при уничтожении

    // Use this for initialization
    void Start () {
        gameManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();
        points = 50;

        sound = GetComponent<AudioSource>();
	}

    //Уничтожение блока
    virtual public void DestroyBrick() {
        sound.Play();
        gameManager.UpdateScore(points);
        gameManager.UpdateCountOfBricks();
        Instantiate(Resources.Load("Explosion"), transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}
