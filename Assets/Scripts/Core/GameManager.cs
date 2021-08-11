using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace SAY2048.Core
{
    public class GameManager : MonoBehaviour
    {
        public Transform[] panelObjectsTransform = new Transform[16];
        public GameObject boxesPrefab;
        public GameObject[,] boxes = new GameObject[4, 4];
        //public GameObject[,] panelPosition = new GameObject[4, 4];
        public PanelObject[,] panelObject = new PanelObject[4, 4];
        public Transform boxPanel;

        public bool[,] isSpaceFilled = new bool[4, 4] { { false, false, false, false }, { false, false, false, false }, { false, false, false, false }, { false, false, false, false } };
        RectTransform rectTransform;
        int boxSize = 100;
        int emptySpaceBtwBoxes = 20;
        public static GameManager singleton;
        bool gameIsContinuing = true;
        [HideInInspector]public bool isInstantiating = false;
        public GameObject gameOverSprite;
        public AudioSource gameOverSound;
        public AudioSource bGSound;
        public AudioSource winSound;
        public ParticleSystem winParticles;


        private void Awake()
        {
            singleton = this;
        }
        private void Start()
        {
            BoxGeneratorInFreeSpace();
        }

        public void BoxGeneratorInFreeSpace()
        {
            isInstantiating = true;
            //Generate a "Number 2" in free space  

            Vector2Int sellectedRandomCoordinates = SelectRandomCoordinate(0, 4);
            //look if it is not empty find empty space
            while (boxes[sellectedRandomCoordinates.x, sellectedRandomCoordinates.y] && gameIsContinuing)
            {

                sellectedRandomCoordinates = SelectRandomCoordinate(0, 4);
                gameIsContinuing = false;
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (boxes[i, j] == null)
                        {
                            gameIsContinuing = true;
                        }
                        else if (boxes[i, j].GetComponentInChildren<TextMeshProUGUI>().text=="2048")
                        {
                            Win();
                        }
                    }
                }

                if (gameIsContinuing == false)
                {
                    StartCoroutine(GameOver());
                }
            }

            //if it is empty instantiate
            if (boxes[sellectedRandomCoordinates.x, sellectedRandomCoordinates.y] == null)
            {
                isInstantiating = true;
                InstantiateNumber(sellectedRandomCoordinates);
            }
            isInstantiating = false;
        }



        private void InstantiateNumber(Vector2Int instantiateCoordinate)
        {
           
            boxes[instantiateCoordinate.x, instantiateCoordinate.y] = Instantiate(boxesPrefab, boxPanel);  
            boxes[instantiateCoordinate.x, instantiateCoordinate.y].name = instantiateCoordinate.x.ToString() + "," + instantiateCoordinate.y.ToString();

            int panelPosition_x = instantiateCoordinate.x * (boxSize) + (instantiateCoordinate.x + 1) * (emptySpaceBtwBoxes);
            int panelPosition_y = instantiateCoordinate.y * (boxSize) + (instantiateCoordinate.y + 1) * (emptySpaceBtwBoxes);
            boxes[instantiateCoordinate.x, instantiateCoordinate.y].transform.localPosition = new Vector3(panelPosition_x, panelPosition_y, 0);
            
           // panelObject[instantiateCoordinate.x, instantiateCoordinate.y].boxObjcet = boxes[instantiateCoordinate.x, instantiateCoordinate.y];

            //if all spaces full then do not instantiate again after this, and game over
        }

        Vector2Int SelectRandomCoordinate(int coordinateX, int coordinateY)
        {
            int randomCoordinateX = UnityEngine.Random.Range(coordinateX, coordinateY);
            int randomCoordinateY = UnityEngine.Random.Range(coordinateX, coordinateY);
            return new Vector2Int(randomCoordinateX, randomCoordinateY);
        }

        IEnumerator GameOver()
        {

            Debug.Log("GameOver");
            gameOverSprite.SetActive(true);
            bGSound.Stop();
            gameOverSound.Play();
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        void Win()
        {
            bGSound.Stop();
            winSound.Play();
            winParticles.Play();
        }
    }
}

