using UnityEngine;

public class Land : MonoBehaviour
{
    public GameObject ground;
    public GameObject fence;
    public GameObject gate;
    public GameObject tiang1, tiang2;
    public WidgetPlayer uiPlayer;

    public Material attackerMat;
    public Material defenderMat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeMat()
    {
        ground.tag = gameObject.tag;
        switch (gameObject.tag)
        {
            case "Attacker":
                fence.GetComponent<MeshRenderer>().material = attackerMat;
                gate.GetComponent<MeshRenderer>().material = attackerMat;
                tiang1.GetComponent<MeshRenderer>().material = attackerMat;
                tiang2.GetComponent<MeshRenderer>().material = attackerMat;
                break;
            case "Defender":
                fence.GetComponent<MeshRenderer>().material = defenderMat;
                gate.GetComponent<MeshRenderer>().material = defenderMat;
                tiang1.GetComponent<MeshRenderer>().material = defenderMat;
                tiang2.GetComponent<MeshRenderer>().material = defenderMat;
                break;
        }
    }
}
