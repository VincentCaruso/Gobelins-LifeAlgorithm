using UnityEngine;

public class Cell : MonoBehaviour {

    public Material mat;
    [ColorUsageAttribute(true, true)]
    public Color color;
    public int isAlive = 0;

    // Start is called before the first frame update
    void Awake() {
        mat = GetComponent<MeshRenderer>().material;
    }

    public void MakeBlack() {
        mat.SetColor("_EmissionColor", Color.black);
        isAlive = 0;
    }

    public void MakeWhite() {
        mat.SetColor("_EmissionColor", color);
        isAlive = 1;
    }
}
