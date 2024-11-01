using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerController : MonoBehaviour
{
    public class KeyCodeAndNum
    {
        public KeyCodeAndNum(KeyCode [] keyCodes, Vector2 vec)
        {
            this.keyCodes = keyCodes;
            this.vec = vec;
        }

        public KeyCode [] keyCodes;
        public Vector2 vec;
    }


    public Sprite def;
    public Sprite move;
    public Sprite attack;
    public Sprite damage;

    public GameObject gameManagerObject;
    public gameManager gameManager;

    public GameObject judgeBar;
    public GameObject bulletBar;

    [SerializeField]
	[Tooltip("弾")]
	private GameObject bullet;

    
    [SerializeField]
	[Tooltip("防弾")]
	private GameObject protectSeed;

    private SpriteRenderer spriteRenderer;
    /*
    z:奥
    x:右
    y:上
    */
    KeyCodeAndNum[] contorols = new KeyCodeAndNum[] {
        new KeyCodeAndNum(new KeyCode[] {KeyCode.W, KeyCode.UpArrow},new Vector2(0,1)),
        new KeyCodeAndNum(new KeyCode[] {KeyCode.S, KeyCode.DownArrow},new Vector2(0,-1)),
        new KeyCodeAndNum(new KeyCode[] {KeyCode.D, KeyCode.RightArrow},new Vector2(1,0)),
        new KeyCodeAndNum(new KeyCode[] {KeyCode.A, KeyCode.LeftArrow},new Vector2(-1,0))
    };

    //GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //enum 
        //[x,y] 右が+ 前が+
        //gameManager= GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        gameManager = gameManagerObject.GetComponent<gameManager>();
        bulletBarWidth = judgeBar.GetComponent<SpriteRenderer>().bounds.size.x;
        bulletCount = bulletSlot;
        gameManager.startTime = Time.time;
    }
    int bulletSlot = 200;
    float bulletBarWidth;

    int bulletCount = 100;
    float spendTime = 0f;
    float defSpeed = 25f;
    float speed = 10f;
    float lastPushedTime = -1f;

    //ダッシュ操作（連続キー入力）の最大時間
    float dashTimeLimit = 0.3f;

    //ダッシュ速度を設定(1が歩く速度と同じ)
    float dashSpeedRate = 10f;
    KeyCode lastPushedId = KeyCode.None;
    
    // Update is called once per frame
    //int? offset_x = null;
    //int? offset_y = null;

    int counter = 0;

    void bulletUse(int count) {
        
            bulletCount-=count;
            displayBulletBar();
            if(bulletCount < 1) {
                attackStatus = "reload";
                return;
            }
            //if(attackStatus == "reload")return;
            attackStatus="ok";
    }
    void attackToEnemy(int mode) {
        counter++;
        if(mode == 1 && counter % 10 == 0) {
            bulletUse(2);
            //var rotation = this.transform.rotation;

            Vector2 bulletPosition = this.transform.position;
		    // 上で取得した場所に、"bullet"のPrefabを出現させる。Bulletの向きはMuzzleのローカル値と同じにする（3つ目の引数）
		    GameObject newBullet = Instantiate(bullet, bulletPosition, this.gameObject.transform.rotation);
		    // 出現させた弾のup(Y軸方向)を取得（MuzzleのローカルY軸方向のこと）
		    Vector2 direction = newBullet.transform.up;
            newBullet.transform.Translate(new Vector2(0,1)*10);
		    // 弾の発射方向にnewBallのY方向(ローカル座標)を入れ、弾オブジェクトのrigidbodyに衝撃力を加える
		    newBullet.GetComponent<Rigidbody2D>().AddForce(direction * 100, ForceMode2D.Impulse);
		    // 出現させた弾の名前を"bullet"に変更
		    newBullet.name = bullet.name;
		    // 出現させた弾を0.8秒後に消す
		    Destroy(newBullet, 1.0f);
        } else if(mode == 2 && counter%5==0) {
            
            bulletUse(1);
            
            Vector2 bulletPosition = this.transform.position;
		    // 上で取得した場所に、"bullet"のPrefabを出現させる。Bulletの向きはMuzzleのローカル値と同じにする（3つ目の引数）
		    GameObject newBullet = Instantiate(protectSeed, bulletPosition, this.gameObject.transform.rotation);
		    // 出現させた弾のup(Y軸方向)を取得（MuzzleのローカルY軸方向のこと）
		    Vector2 direction = newBullet.transform.up;
            newBullet.transform.Translate(new Vector2(0,1)*10);
		    // 出現させた弾の名前を"bullet"に変更
		    newBullet.name = bullet.name;
		    // 出現させた弾を0.8秒後に消す
		    Destroy(newBullet, 4.0f);
        }
    }


    float lastTime = 0;
    int frameCounter = 0;
    void playerRotate(int direction) {
        ///counter ++;
        //float d = (Time.fixedDeltaTime - lastTime)*frameCounter;
        //Debug.Log(d);
        //if(d > 0.1) {
        if(frameCounter%5 == 0) {
            this.gameObject.transform.Rotate(new Vector3(0,0,direction));
            lastTime = Time.fixedDeltaTime;
        }
        //}
    }

    string status = "def";

    bool anyKeyIsPressing = false;
    string attackStatus = "ok";

    void displayBulletBar() {
        bulletBar.transform.localScale = new Vector3(1.3f*bulletCount/bulletSlot, 0.13f, 1);
    }
    void Update()
    {
        if(attackStatus == "reload") {
            if(bulletCount > bulletSlot) {
                attackStatus = "ok";
                bulletCount = bulletSlot;
            }
            bulletCount += 1;
            displayBulletBar();
        }
        frameCounter ++;
        if(!anyKeyIsPressing && speed != defSpeed) {
            speed = defSpeed;
        }
        bool temp = false;
        spendTime += Time.deltaTime;
        spriteRenderer.sprite = def;
        for(int i = 0;i < contorols.Length;i++) {
            KeyCodeAndNum ctrl = contorols[i];
            KeyCode [] KeyCodes = ctrl.keyCodes;
            
            Transform transform = this.transform;
            for(int j = 0;j < KeyCodes.Length;j++) {
                KeyCode key = KeyCodes[j];
                if(Input.GetKey(key)) {
                    /*transform.Translate(
                        ctrl.vec.x*Time.deltaTime*speed,
                        ctrl.vec.y*Time.deltaTime*speed,0
                    );*/
                    Rigidbody2D rb = this.gameObject.GetComponent<Rigidbody2D>();
                    rb.AddRelativeFor​​ce(new Vector2(
                        ctrl.vec.x*speed,
                        ctrl.vec.y*speed
                    )); 

                    spriteRenderer.sprite = move;   
                    Awake();     
                    //this.gameObject.transform.localScale = new Vector3(0.25f,0.25f,1f);;
                    status = "move";
                    temp = true;
                }
                
                if (Input.GetKey(KeyCode.L)) {
                    //playerRotate();
                    attackToEnemy(1);
                    spriteRenderer.sprite = attack;
                    status = "attack";
                    changeToDef(0.5f);
                }
                
                if (Input.GetKey(KeyCode.O)) {
                    //playerRotate();
                    attackToEnemy(2);
                    spriteRenderer.sprite = attack;
                    status = "attack";
                    changeToDef(0.5f);
                }
                if (Input.GetKey(KeyCode.P)) {
                    attackStatus = "reload";
                    //bulletCount = 0;
                }

                if (Input.GetKey(KeyCode.Q)||Input.GetKey(KeyCode.A)) {
                    playerRotate(1);
                }
                if (Input.GetKey(KeyCode.E)||Input.GetKey(KeyCode.D)) {
                    playerRotate(-1);
                }

                //ダッシュ操作などのコマンド
                if(spendTime - lastPushedTime > dashTimeLimit) {
                    lastPushedId = KeyCode.None;
                }
                if(Input.GetKeyDown(key)) {
                    if(lastPushedId != KeyCode.None) {
                        if(spendTime - lastPushedTime < dashTimeLimit) {
                            if(lastPushedId == KeyCodes[0]) {
                                speed = dashSpeedRate;
                                lastPushedId = KeyCode.None;
                            }
                        } else {
                            lastPushedId = KeyCode.None;
                        }
                    } else {
                        lastPushedId = KeyCodes[0];
                        lastPushedTime = spendTime;
                    }
                }
            }
            anyKeyIsPressing = temp;
        }
    }    


    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "rotten_sheed") {
            spriteRenderer.sprite = damage;
            if(status == "move") {
                gameManager.damagePlayer(8f);
            } else {
                gameManager.damagePlayer(1f);
            }
            StartCoroutine(changeToDef(1f));
        } else if(other.gameObject.tag == "protect_bullet") {
            CircleCollider2D col = other.gameObject.GetComponent<CircleCollider2D>();
            col.enabled = false;
            reEnable(col);
        }
    }
    //Einumerator changeToDef(float waitTime){
        
    //}
    IEnumerator changeToDef(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        spriteRenderer.sprite = def;
        //this.gameObject.transform.localScale = new Vector3(1f,1f,1f);
    }

    
    IEnumerator reEnable(CircleCollider2D target)
    {
        yield return new WaitForSeconds(1f);
        target.enabled = true;
        //this.gameObject.transform.localScale = new Vector3(1f,1f,1f);
    }

    private void Awake()
    {
        var spriteRenderer    = GetComponent<SpriteRenderer>();
        var sprite            = spriteRenderer.sprite;
        var polygonCollider2D = GetComponent<PolygonCollider2D>();
        var physicsShapeCount = sprite.GetPhysicsShapeCount();

        polygonCollider2D.pathCount = physicsShapeCount;

        var physicsShape = new List<Vector2>();

        for ( var i = 0; i < physicsShapeCount; i++ )
        {
            physicsShape.Clear();
            sprite.GetPhysicsShape( i, physicsShape );
            var points = physicsShape.ToArray();
            polygonCollider2D.SetPath( i, points );
        }
    }
}
