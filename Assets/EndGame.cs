using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TreeSharpPlus;

public class EndGame : MonoBehaviour
{
    [SerializeField]
    public GameObject fairy;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnGUI()
    {

        Val<Vector3> playerpos = Val.V(() => player.transform.position);
        Val<Vector3> cluepos = Val.V(() => fairy.transform.position);
        Val<Vector3> diffpos = Val.V(() => playerpos.Value - cluepos.Value);
        Val<float> distaway = Val.V(() => diffpos.Value.sqrMagnitude);

        if (Input.GetKeyDown(KeyCode.E) && distaway.Value < 2.0)
        {

            Application.OpenURL("about:blank");

        }
    }
}
