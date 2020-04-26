using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LifeBoard : MonoBehaviour {

    public GameObject CellPrefab;
    public GameObject Board;

    public int BoardWidth = 100;
    public int BoardHeight = 100;

    public float Interval = 0.3f;

    public Cell[,] listCells;
    public int[,] NextCells;


    private void Awake() {
        listCells = new Cell[BoardWidth, BoardHeight];
        NextCells = new int[BoardWidth, BoardHeight];
    }

    // Start is called before the first frame update
    void Start() {
        MakeBoard();
    }

    private void MakeBoard() {
        GameObject instance;

        for (int y = 0; y < BoardHeight; y++) {
            for (int x = 0; x < BoardWidth; x++) {
                instance = Instantiate(CellPrefab, new Vector3(x, 0, y), Quaternion.identity, Board.transform);

                Cell c = instance.GetComponent<Cell>();

                listCells[x, y] = c;

                if (UnityEngine.Random.value > 0.5) {
                    c.MakeWhite();
                    NextCells[x, y] = 1;
                } else {
                    c.MakeBlack();
                    NextCells[x, y] = 0;
                }
            }
        }

        Board.transform.position = new Vector3(-BoardWidth * 0.5f +0.5f, 0, -BoardHeight * 0.5f + 0.5f);

        StartCoroutine("UpdateBoard");
    }

    IEnumerator UpdateBoard() {

        int neighboursScore;

        while (true) {

            for (int y = 0; y < BoardHeight; y++) {
                for (int x = 0; x < BoardWidth; x++) {
                    neighboursScore = 0;

                    //top
                    neighboursScore += CheckNeighbour(x - 1, y - 1);
                    neighboursScore += CheckNeighbour(x, y - 1);
                    neighboursScore += CheckNeighbour(x + 1, y - 1);
                    //same line
                    neighboursScore += CheckNeighbour(x - 1, y);
                    neighboursScore += CheckNeighbour(x + 1, y);
                    //bottom
                    neighboursScore += CheckNeighbour(x - 1, y + 1);
                    neighboursScore += CheckNeighbour(x, y + 1);
                    neighboursScore += CheckNeighbour(x + 1, y + 1);

                    if (listCells[x, y].isAlive == 1) {
                        if (neighboursScore < 2 || neighboursScore > 3) {
                            NextCells[x, y] = 0;

                            continue;
                        }
                    }

                    if (listCells[x, y].isAlive == 0 && neighboursScore == 3) {
                        NextCells[x, y] = 1;

                        continue;
                    }

                    NextCells[x, y] = listCells[x, y].isAlive;
                }
            }

            for (int y = 0; y < BoardHeight; y++) {
                for (int x = 0; x < BoardWidth; x++) {
                    if (NextCells[x, y] == 0) {
                        listCells[x, y].MakeBlack();
                    } else {
                        listCells[x, y].MakeWhite();
                    }
                }
            }

            yield return new WaitForSeconds(Interval);

        }
    }

    private int CheckNeighbour(int x, int y) {

        if (x >= 0 && y >= 0 && x < BoardWidth && y < BoardHeight) {
            return listCells[x, y].isAlive;
        }

        return 0;
    }

    public void Reset() {
        SceneManager.LoadScene(0);
    }

    public void OnSliderValueChanged(float num) {

        Interval = (0.5f + 0.001f)-num;
    }
}
