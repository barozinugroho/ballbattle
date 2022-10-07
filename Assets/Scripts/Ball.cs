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
}
