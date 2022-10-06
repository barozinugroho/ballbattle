using UnityEngine;

public class Ball : MonoBehaviour
{
    public Transform player;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            /*Vector3 target = player.localPosition - transform.localPosition;
            target.y = 0f;
            transform.Translate(target * speed * Time.deltaTime);*/

            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    public void SetParent()
    {
        transform.parent = player;
        transform.SetAsFirstSibling();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                Debug.Log(collision.gameObject.name);
                if (collision.gameObject.GetComponent<Soldier>().param.isAttacker)
                {
                    if (!GameManager.instance.isBallOccupied)
                    {
                        collision.gameObject.GetComponent<Soldier>().highlight.SetActive(true);

                        transform.SetParent(collision.transform);
                        transform.SetAsFirstSibling();

                        GameManager.instance.isBallOccupied = true;
                        GameManager.instance.listSoldier.freeAttackers.Remove(collision.transform);

                        player = null;
                    }
                }
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
                Debug.Log(other.name);
                break;
        }
    }
}
