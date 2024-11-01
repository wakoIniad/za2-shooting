using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{

    
    public GameObject hpBer;
    
    public GameObject gameManagerObject;
    public gameManager gameManager;
	public GameObject bullet;

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = gameManagerObject.GetComponent<gameManager>();
        
    }

    // Update is called once per frame
    int frameCounter = 0;
    void Update()
    {
        Vector2 bulletPosition = this.transform.position;
        Vector2 playerPosition = player.transform.position;
        Vector2 dirOfPlayer = (playerPosition-bulletPosition).normalized*0.1f;
        //Vector2 dirOfPlayer = (playerPosition-bulletPosition)/50;
        this.GetComponent<Rigidbody2D>().AddForce(dirOfPlayer*1, ForceMode2D.Impulse);

        frameCounter ++;
        this.gameObject.transform.Rotate(new Vector3(0,0,1)); 
        if(frameCounter%10==0) {
            //var rotation = this.transform.rotation;

		    // 上で取得した場所に、"bullet"のPrefabを出現させる。Bulletの向きはMuzzleのローカル値と同じにする（3つ目の引数）
		    GameObject newBullet = Instantiate(bullet, bulletPosition, this.gameObject.transform.rotation);
		    // 出現させた弾のup(Y軸方向)を取得（MuzzleのローカルY軸方向のこと）
		    Vector2 direction = newBullet.transform.up;
		    // 弾の発射方向にnewBallのY方向(ローカル座標)を入れ、弾オブジェクトのrigidbodyに衝撃力を加える
		    newBullet.GetComponent<Rigidbody2D>().AddForce(direction * 100, ForceMode2D.Impulse);
		    // 出現させた弾の名前を"bullet"に変更
		    newBullet.name = bullet.name;
		    // 出現させた弾を0.8秒後に消す
		    Destroy(newBullet, 1.0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "purified_sheed") {
            gameManager.damageEnemy(2f);
        }
    }
}
