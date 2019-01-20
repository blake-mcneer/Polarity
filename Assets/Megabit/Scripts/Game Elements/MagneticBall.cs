using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticBall : MonoBehaviour {

    public Material[] MagneticMaterials;
    public GameObject[] Explosions;
    public AudioSource AudioClipCollide;
    public AudioSource AudioClipExplode;
    public GameObject trailPrefab;
    public GameObject shieldComponent;
    public float magneticConstant = 1.0f;
    public float placeholderMagneticField = 0.0f;
    public MagneticType type;
    public int totalCount = 1;
    public TextMesh tMesh;
    int collisionCount = 0;
    float scale = 1.0f;
    float targetScale = 1.0f;
    float scaleSpeed = 4.0f;
    bool shrinkingAway = false;
    bool hasExploded = false;
    int collisionLimit = 10;
    ScreenLimits limits;
    [HideInInspector] public int index;
    [HideInInspector] public bool hasBeenScored = false;
    [HideInInspector] public bool hasBeenAbsorbed = false;
    [HideInInspector] public bool shieldsActivated = false;
    Vector3 previousVelocity;
    Rigidbody rb;
    GameObject myTrail;
    GameManager manager;
    private void Start()
    {
        ConfigureScreenLimits();
        ConfigureBall();
        myTrail = Instantiate(trailPrefab, transform.parent);
    }
    void ConfigureScreenLimits()
    {
        Vector3 lowerLeftCorner = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 upperRightCorner = Camera.main.ViewportToWorldPoint(Vector3.one);
        Vector3 upperRightScreenPoint = Camera.main.ViewportToScreenPoint(Vector3.one);
        upperRightScreenPoint.y = Screen.height * (1.0f - GameSingleton.Instance.bannerPercentage);
        upperRightCorner = Camera.main.ScreenToWorldPoint(upperRightScreenPoint);

        limits.leftLimit = lowerLeftCorner.x + transform.localScale.x /2.0f;
        limits.rightLimit = upperRightCorner.x - transform.localScale.x /2.0f;
        limits.bottomLimit = lowerLeftCorner.y + transform.localScale.x/2.0f;
        limits.topLimit = upperRightCorner.y - transform.localScale.x/2.0f;
    }

    public void ConfigureBall()
    {
        rb = GetComponent<Rigidbody>();
        manager = FindObjectOfType<GameManager>();
        SetMaterial();
    }
    public void DeactivateShields()
    {
        shieldsActivated = false;
        shieldComponent.SetActive(false);
    }

    public void ActivateShields()
    {
        shieldsActivated = true;
        shieldComponent.SetActive(true);
    }

    public void SetMaterial()
    {
        Material mat = GetComponent<Renderer>().sharedMaterial;
        switch (type)
        {
            case MagneticType.M1:
                mat = MagneticMaterials[0];
                //mat = M1_Material;
                break;
            case MagneticType.M2:
                mat = MagneticMaterials[1];
                //mat = M2_Material;
                break;
            case MagneticType.M3:
                mat = MagneticMaterials[2];
                //mat = M3_Material;
                break;
        }
        Renderer rend = GetComponent<Renderer>();
        rend.sharedMaterial = mat;
        //renderer.sharedMaterials = mats;
    }

    void UpdateScale()
    {
        float direction = shrinkingAway ? -1.0f : 1.0f;
        scale += Time.deltaTime * scaleSpeed * direction;
        if (shrinkingAway){
            scale = Mathf.Max(0.0f, scale);
        }else{
            scale = Mathf.Min(scale, targetScale);
        }
        transform.localScale = Vector3.one * scale;
        ConfigureScreenLimits();
    }
    Vector3 LimitPosition(Vector3 rawPosition)
    {
        float x = Mathf.Max(rawPosition.x, limits.leftLimit);
        x = Mathf.Min(x, limits.rightLimit);
        float y = Mathf.Max(rawPosition.y, limits.bottomLimit);
        y = Mathf.Min(y, limits.topLimit);

        Vector3 limitedPosition = new Vector3(x, y, rawPosition.z);
        return limitedPosition;
    }
    private void FixedUpdate()
    {
        if (manager == null) ConfigureBall();

        if (manager == null) return;

        if (Mathf.Abs((targetScale - scale)) > 0.05f)
        {
            UpdateScale();
        }else if (shrinkingAway){
            Destroy(myTrail);
            Destroy(gameObject);    
        }

        if (shrinkingAway) return;

        rb.velocity = Vector3.zero;
        Vector2 pulseAffect = manager.AffectOnPosition(transform.position, index);
        Vector2 otherBallEffect = manager.AffectFromNearbyObjects(this);
        bool shieldsShouldBeActivated = (Mathf.Abs(otherBallEffect.x) > 0.1f && Mathf.Abs(otherBallEffect.y) > 0.1f);
        //if (shieldsShouldBeActivated && !shieldsActivated) ActivateShields();
        //if (!shieldsShouldBeActivated && shieldsActivated) DeactivateShields();
        float xTarget = transform.position.x + (pulseAffect.x + otherBallEffect.x) * magneticConstant * Time.deltaTime;
        float yTarget = transform.position.y - (pulseAffect.y + otherBallEffect.y) * magneticConstant * Time.deltaTime;
        Vector3 targetPos = LimitPosition(new Vector3(xTarget, yTarget, 0.0f));
        targetPos = Vector3.Lerp(transform.position, targetPos, 0.75f);
        rb.MovePosition(targetPos);
        myTrail.transform.position = transform.position;
    }
    void TallyCollision()
    {
        if (hasExploded) return;
        collisionCount++;
        if (collisionCount >= collisionLimit)
        {
            SelfDestruct();
        }
    }
    void HandleCollisionWithBall(MagneticBall ball)
    {
        AudioManager.Instance.PlayClip(AudioClipCollide);
        TallyCollision();
        int[] indecesAffected = { index, ball.index };
        Vector3 pos = (ball.transform.position + transform.position)/2.0f;
        //Vector3 mult = (ball.transform.position - transform.position).normalized;
        //Vector3 addVec = new Vector3(pos.x * mult.x, pos.y * mult.y, pos.z * mult.z);
        //pos = transform.position - addVec;
        if (index > ball.index)
        {
            manager.AddRepulsion(pos, indecesAffected, -5.0f);
        }
        //manager.RemoveFromCurrentAttractionPulses(index);
    }
    void HandleCollisionWithBarrier(Transform collisionTransform)
    {
        AudioManager.Instance.PlayClip(AudioClipCollide);
        //TallyCollision();
        int[] indecesAffected = { index };
        Vector3 pos = (collisionTransform.position + transform.position)/2.0f;
        //pos = transform.position + pos;
        //manager.RemoveFromCurrentAttractionPulses(index);
        manager.AddRepulsion(pos, indecesAffected, -2.5f);
    }
    void HandleCollisionWithGoal(Goal g)
    {
        if (g.type == type)
        {
            g.HitByMagneticObject(gameObject.GetComponent<MagneticBall>());
            ShrinkAway();
        }
        else
        {
            //TallyCollision();
            AudioManager.Instance.PlayClip(AudioClipCollide);
            int[] indecesAffected = { index };
            Vector3 pos = (g.transform.position + transform.position)/2.0f;
            //pos = transform.position + pos;
            manager.AddRepulsion(pos, indecesAffected, -2.5f);
            //manager.RemoveFromCurrentAttractionPulses(index);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "MagneticBall")
        {
            MagneticBall mag = collision.gameObject.GetComponent<MagneticBall>();
            if (mag.type == type)
            {
                if (index > mag.index)
                {
                    AbsorbObject(mag);
                }
            }
            else
            {
                HandleCollisionWithBall(mag);
            }
        }else if (collision.gameObject.tag == "Barrier" || collision.gameObject.tag == "BorderPiece")
        {
            HandleCollisionWithBarrier(collision.transform);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
        {
            Goal g = other.transform.parent.GetComponent<Goal>();
            HandleCollisionWithGoal(g);
        }
    }
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.tag == "Finish")
    //    {
    //        Goal g = other.transform.parent.GetComponent<Goal>();
    //        HandleCollisionWithGoal(g);
    //    }
    //}
    public void AbsorbObject(MagneticBall obj)
    {
        if (obj.hasBeenAbsorbed) return;
        obj.hasBeenAbsorbed = true;
        totalCount+= obj.totalCount;
        collisionCount = 0;
        tMesh.text = totalCount.ToString();
        targetScale = (float)(totalCount -1) * 0.25f + 1.0f;
        obj.ShrinkAway();
        
    }
    public void SelfDestruct()
    {
        AudioManager.Instance.PlayClip(AudioClipExplode);
        hasExploded = true;
        GameObject explosionPrefab = Explosions[0];
        switch (type)
        {
            case MagneticType.M1:
                explosionPrefab = Explosions[0];
                break;
            case MagneticType.M2:
                explosionPrefab = Explosions[1];
                break;
            case MagneticType.M3:
                explosionPrefab = Explosions[2];
                break;
        }
        Instantiate(explosionPrefab, transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f));
        ShrinkAway();
        //pulse.prefab = Instantiate(prefab, pulse.pulseLocation, Quaternion.Euler(0.0f, 0.0f, 0.0f), transform);

    }
    public void ShrinkAway()
    {
        targetScale = 0.0f;
        shrinkingAway = true;
    }
    private void OnDrawGizmosSelected()
    {
        SetMaterial();
    }
}
