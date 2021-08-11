using UnityEngine;
using TMPro;
using SAY2048.Core;
using SAY2048.Inputs;

namespace SAY2048.Contollers
{

    public class GameController : MonoBehaviour
    {
        int boxSize = 100;
        int emptySpaceBtwBoxes = 20;
        public TextMeshProUGUI textMeshProUGUI;
        private float clickCooldown = 0.6f;
        private float clickTimer = 0;
        public bool isMoving = false;
        private GameObject tempGameObject;


        void FixedUpdate()
        {
            InputUpdate();
        }

        void InputUpdate()
        {
            if (GameInputs.singleton.inputX == 1 && clickTimer < Time.time)
            {
                //This is for wait  1,5 seconds until next press
                clickTimer = Time.time + clickCooldown;
                if (GameManager.singleton.isInstantiating == false)
                {
                    GoRight();
                }

                if (isMoving == false)
                {
                    GameManager.singleton.Invoke("BoxGeneratorInFreeSpace",0.1f);
                }
            }
            if (GameInputs.singleton.inputX == -1 && clickTimer < Time.time)
            {
                //This is for wait  1,5 seconds until next press
                clickTimer = Time.time + clickCooldown;
                //Shift Left
                if (GameManager.singleton.isInstantiating == false)
                {
                    GoLeft();
                }

                if (isMoving == false)
                {
                    GameManager.singleton.Invoke("BoxGeneratorInFreeSpace", 0.1f);
                }
            }
            if (GameInputs.singleton.inputY == 1 && clickTimer < Time.time)
            {
                //This is for wait  1,5 seconds until next press
                clickTimer = Time.time + clickCooldown;
                //Shift Up
                Debug.Log("Y=1");
                if (GameManager.singleton.isInstantiating == false)
                {
                    GoUp();
                }

                if (isMoving == false)
                {
                    GameManager.singleton.Invoke("BoxGeneratorInFreeSpace", 0.1f);
                }
            }
            if (GameInputs.singleton.inputY == -1 && clickTimer < Time.time)
            {
                //This is for wait  1,5 seconds until next press
                clickTimer = Time.time + clickCooldown;
                //Shift Down
                Debug.Log("Y=-1");
                if (GameManager.singleton.isInstantiating == false)
                {
                    GoDown();
                }

                if (isMoving == false)
                {
                    GameManager.singleton.Invoke("BoxGeneratorInFreeSpace", 0.4f);
                }
            }
        }

        void GoRight()
        {
            isMoving = true;

            //Shift 
            Shift(0);
            Shift(1);
            Shift(2);
            Shift(3);

            isMoving = false;
        }
        void GoLeft()
        {
            isMoving = true;
            ShiftLeft(0, 3);
            ShiftLeft(1, 3);
            ShiftLeft(2, 3);
            ShiftLeft(3, 3);
            isMoving = false;
        }
        void GoUp()
        {
            isMoving = true;

            //Shift 
            ShiftUp(0);
            ShiftUp(1);
            ShiftUp(2);
            ShiftUp(3);

            isMoving = false;

        }
        void GoDown()
        {
            isMoving = true;

            //Shift 
            ShiftDown(0, 3);
            ShiftDown(1, 3);
            ShiftDown(2, 3);
            ShiftDown(3, 3);

            isMoving = false;

        }


        void LookAndShift(int shiftFromX, int shiftFromY, int shiftToX, int shiftToY, int step, int dirX, int dirY)
        {
            string tempText = GameManager.singleton.boxes[shiftFromX, shiftFromY].GetComponentInChildren<TextMeshProUGUI>().text;
            string tempName = GameManager.singleton.boxes[shiftFromX, shiftFromY].name;
            DestroyImmediate(GameManager.singleton.boxes[shiftFromX, shiftFromY]);
            
            GameManager.singleton.boxes[shiftToX, shiftToY] = Instantiate(GameManager.singleton.boxesPrefab, GameManager.singleton.boxPanel);
            GameManager.singleton.boxes[shiftToX, shiftToY].name = shiftToX.ToString() + "," + shiftToY.ToString();
            GameManager.singleton.boxes[shiftToX, shiftToY].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = tempText;

            int panelPosition_x = shiftToX * (boxSize) + (shiftToX + 1) * (emptySpaceBtwBoxes);
            int panelPosition_y = shiftToY * (boxSize) + (shiftToY + 1) * (emptySpaceBtwBoxes);
            GameManager.singleton.boxes[shiftToX, shiftToY].transform.localPosition = new Vector3(panelPosition_x, panelPosition_y, 0);

        }
        void LookAndCombine(int destroyedCoordinateX, int destroyedCoordinateY, int combinedCoordinateX, int combinedCoordinateY)
        {
            int boxNumber = int.Parse(GameManager.singleton.boxes[combinedCoordinateX, combinedCoordinateY].GetComponentInChildren<TextMeshProUGUI>().text);
            boxNumber *= 2;
            GameManager.singleton.boxes[combinedCoordinateX, combinedCoordinateY].GetComponentInChildren<TextMeshProUGUI>().text = boxNumber.ToString();


            DestroyImmediate(GameManager.singleton.boxes[destroyedCoordinateX, destroyedCoordinateY]);
        }

        void Shift(int coordinateY)
        {
            //if there is object in 2 horizontal
            if (GameManager.singleton.boxes[2, coordinateY] != null) //shift from here
            {
                if (GameManager.singleton.boxes[3, coordinateY] == null) //shift to here
                {
                    LookAndShift(2, coordinateY, 3, coordinateY, 1, 1, 0);
                }
                else if (GameManager.singleton.boxes[3, coordinateY] != null)
                // IF THERE IS OBJECT
                //If the object is the same combine 
                {
                    //If number are equal then multiply by 2 and combine them
                    if (GameManager.singleton.boxes[2, coordinateY].GetComponentInChildren<TextMeshProUGUI>().text == GameManager.singleton.boxes[3, coordinateY].GetComponentInChildren<TextMeshProUGUI>().text)
                    {
                        LookAndCombine(2, coordinateY, 3, coordinateY);
                    }
                    //Else If numbers are not equal then not move 
                }
            }
            //if there is object in 1 horizontal
            if (GameManager.singleton.boxes[1, coordinateY] != null)
            {
                if (GameManager.singleton.boxes[2, coordinateY] == null)
                {
                    if (GameManager.singleton.boxes[3, coordinateY] == null)
                    {
                        LookAndShift(1, coordinateY, 3, coordinateY, 2, 1, 0);
                    }
                    else if (GameManager.singleton.boxes[3, coordinateY] != null)
                    {
                        //If number are equal then multiply by 2 and combine them
                        if (GameManager.singleton.boxes[1, coordinateY].GetComponentInChildren<TextMeshProUGUI>().text == GameManager.singleton.boxes[3, coordinateY].GetComponentInChildren<TextMeshProUGUI>().text)
                        {
                            LookAndCombine(1, coordinateY, 3, coordinateY);
                        }
                        //Else If numbers are not equal then not move
                        else
                        {
                            LookAndShift(1, coordinateY, 2, coordinateY, 2, 1, 0);
                        }
                    }
                }
                else
                {
                    //If number are equal then multiply by 2 and combine them
                    if (GameManager.singleton.boxes[1, coordinateY].GetComponentInChildren<TextMeshProUGUI>().text == GameManager.singleton.boxes[2, coordinateY].GetComponentInChildren<TextMeshProUGUI>().text)
                    {
                        LookAndCombine(1, coordinateY, 2, coordinateY);
                    }
                    //Else If numbers are not equal then not move
                }
            }
            //if there is object in 0 horizontal
            if (GameManager.singleton.boxes[0, coordinateY] != null)
            {
                if (GameManager.singleton.boxes[1, coordinateY] == null)
                {
                    if (GameManager.singleton.boxes[2, coordinateY] == null)
                    {
                        if (GameManager.singleton.boxes[3, coordinateY] == null)
                        {
                            LookAndShift(0, coordinateY, 3, coordinateY, 3, 1, 0);
                        }
                        else if (GameManager.singleton.boxes[3, coordinateY] != null)
                        {
                            if (GameManager.singleton.boxes[0, coordinateY].GetComponentInChildren<TextMeshProUGUI>().text == GameManager.singleton.boxes[3, coordinateY].GetComponentInChildren<TextMeshProUGUI>().text)
                            {
                                LookAndCombine(0, coordinateY, 3, coordinateY);
                            }
                            else
                            {
                                LookAndShift(0, coordinateY, 2, coordinateY, 2, 1, 0);
                            }
                        }
                    }
                    else if (GameManager.singleton.boxes[2, coordinateY] != null)
                    {
                        if (GameManager.singleton.boxes[0, coordinateY].GetComponentInChildren<TextMeshProUGUI>().text == GameManager.singleton.boxes[2, coordinateY].GetComponentInChildren<TextMeshProUGUI>().text)
                        {
                            LookAndCombine(0, coordinateY, 2, coordinateY);
                        }
                        else
                        {
                            LookAndShift(0, coordinateY, 1, coordinateY, 1, 1, 0);
                        }
                    }
                }
                else if (GameManager.singleton.boxes[1, coordinateY] != null)
                {
                    if (GameManager.singleton.boxes[0, coordinateY].GetComponentInChildren<TextMeshProUGUI>().text == GameManager.singleton.boxes[1, coordinateY].GetComponentInChildren<TextMeshProUGUI>().text)
                    {
                        LookAndCombine(0, coordinateY, 1, coordinateY);
                    }
                    //Else If numbers are not equal then not move
                }
            }
        }
        void ShiftLeft(int coordinateY, int maxCoordinateNumber)
        {
            if (GameManager.singleton.boxes[maxCoordinateNumber - 2, coordinateY] != null)
            {
                if (GameManager.singleton.boxes[maxCoordinateNumber - 3, coordinateY] == null)
                {
                    LookAndShift(maxCoordinateNumber - 2, coordinateY, maxCoordinateNumber - 3, coordinateY, 1, -1, 0);
                }
                else if (GameManager.singleton.boxes[maxCoordinateNumber - 3, coordinateY] != null)
                {
                    if (GameManager.singleton.boxes[maxCoordinateNumber - 2, coordinateY].GetComponentInChildren<TextMeshProUGUI>().text == GameManager.singleton.boxes[maxCoordinateNumber - 3, coordinateY].GetComponentInChildren<TextMeshProUGUI>().text)
                    {
                        LookAndCombine(maxCoordinateNumber - 2, coordinateY, maxCoordinateNumber - 3, coordinateY);
                    }
                }
            }
            if (GameManager.singleton.boxes[maxCoordinateNumber - 1, coordinateY] != null)
            {
                if (GameManager.singleton.boxes[maxCoordinateNumber - 2, coordinateY] == null)
                {
                    if (GameManager.singleton.boxes[maxCoordinateNumber - 3, coordinateY] == null)
                    {
                        LookAndShift(maxCoordinateNumber - 1, coordinateY, maxCoordinateNumber - 3, coordinateY, 2, -1, 0);
                    }
                    else if (GameManager.singleton.boxes[maxCoordinateNumber - 3, coordinateY] != null)
                    {
                        //If number are equal then multiply by 2 and combine them
                        if (GameManager.singleton.boxes[maxCoordinateNumber - 1, coordinateY].GetComponentInChildren<TextMeshProUGUI>().text == GameManager.singleton.boxes[maxCoordinateNumber - 3, coordinateY].GetComponentInChildren<TextMeshProUGUI>().text)
                        {
                            LookAndCombine(maxCoordinateNumber - 1, coordinateY, maxCoordinateNumber - 3, coordinateY);
                        }
                        //Else If numbers are not equal then not move
                        else
                        {
                            LookAndShift(maxCoordinateNumber - 1, coordinateY, maxCoordinateNumber - 2, coordinateY, 2, -1, 0);
                        }
                    }
                }
                else
                {
                    //If number are equal then multiply by 2 and combine them
                    if (GameManager.singleton.boxes[maxCoordinateNumber - 1, coordinateY].GetComponentInChildren<TextMeshProUGUI>().text == GameManager.singleton.boxes[maxCoordinateNumber - 2, coordinateY].GetComponentInChildren<TextMeshProUGUI>().text)
                    {
                        LookAndCombine(maxCoordinateNumber - 1, coordinateY, maxCoordinateNumber - 2, coordinateY);
                    }
                    //Else If numbers are not equal then not move
                }
            }
            if (GameManager.singleton.boxes[maxCoordinateNumber - 0, coordinateY] != null)
            {
                if (GameManager.singleton.boxes[maxCoordinateNumber - 1, coordinateY] == null)
                {
                    if (GameManager.singleton.boxes[maxCoordinateNumber - 2, coordinateY] == null)
                    {
                        if (GameManager.singleton.boxes[maxCoordinateNumber - 3, coordinateY] == null)
                        {
                            LookAndShift(maxCoordinateNumber - 0, coordinateY, maxCoordinateNumber - 3, coordinateY, 3, -1, 0);
                        }
                        else if (GameManager.singleton.boxes[maxCoordinateNumber - 3, coordinateY] != null)
                        {
                            if (GameManager.singleton.boxes[maxCoordinateNumber - 0, coordinateY].GetComponentInChildren<TextMeshProUGUI>().text == GameManager.singleton.boxes[maxCoordinateNumber - 3, coordinateY].GetComponentInChildren<TextMeshProUGUI>().text)
                            {
                                LookAndCombine(maxCoordinateNumber - 0, coordinateY, maxCoordinateNumber - 3, coordinateY);
                            }
                            else
                            {
                                LookAndShift(maxCoordinateNumber - 0, coordinateY, maxCoordinateNumber - 2, coordinateY, 2, -1, 0);
                            }
                        }
                    }
                    else if (GameManager.singleton.boxes[maxCoordinateNumber - 2, coordinateY] != null)
                    {
                        if (GameManager.singleton.boxes[maxCoordinateNumber - 0, coordinateY].GetComponentInChildren<TextMeshProUGUI>().text == GameManager.singleton.boxes[maxCoordinateNumber - 2, coordinateY].GetComponentInChildren<TextMeshProUGUI>().text)
                        {
                            LookAndCombine(maxCoordinateNumber - 0, coordinateY, maxCoordinateNumber - 2, coordinateY);
                        }
                        else
                        {
                            LookAndShift(maxCoordinateNumber - 0, coordinateY, maxCoordinateNumber - 1, coordinateY, 1, -1, 0);
                        }
                    }
                }
                else if (GameManager.singleton.boxes[maxCoordinateNumber - 1, coordinateY] != null)
                {
                    if (GameManager.singleton.boxes[maxCoordinateNumber - 0, coordinateY].GetComponentInChildren<TextMeshProUGUI>().text == GameManager.singleton.boxes[maxCoordinateNumber - 1, coordinateY].GetComponentInChildren<TextMeshProUGUI>().text)
                    {
                        LookAndCombine(maxCoordinateNumber - 0, coordinateY, maxCoordinateNumber - 1, coordinateY);
                    }
                    //Else If numbers are not equal then not move
                }
            }
        }
        void ShiftUp(int coordinateY)
        {
            //if there is object in 2 horizontal
            if (GameManager.singleton.boxes[coordinateY, 2] != null) //shift from here
            {
                if (GameManager.singleton.boxes[coordinateY, 3] == null) //shift to here
                {
                    LookAndShift(coordinateY, 2, coordinateY, 3, 1, 0, 1);
                }
                else if (GameManager.singleton.boxes[coordinateY, 3] != null)
                // IF THERE IS OBJECT
                //If the object is the same combine 
                {
                    //If number are equal then multiply by 2 and combine them
                    if (GameManager.singleton.boxes[coordinateY, 2].GetComponentInChildren<TextMeshProUGUI>().text == GameManager.singleton.boxes[coordinateY, 3].GetComponentInChildren<TextMeshProUGUI>().text)
                    {
                        LookAndCombine(coordinateY, 2, coordinateY, 3);
                    }
                    //Else If numbers are not equal then not move 
                }
            }
            //if there is object in 1 horizontal
            if (GameManager.singleton.boxes[coordinateY, 1] != null)
            {
                if (GameManager.singleton.boxes[coordinateY, 2] == null)
                {
                    if (GameManager.singleton.boxes[coordinateY, 3] == null)
                    {
                        LookAndShift(coordinateY, 1, coordinateY, 3, 2, 0, 1);
                    }
                    else if (GameManager.singleton.boxes[3, coordinateY] != null)
                    {
                        //If number are equal then multiply by 2 and combine them
                        if (GameManager.singleton.boxes[coordinateY, 1].GetComponentInChildren<TextMeshProUGUI>().text == GameManager.singleton.boxes[coordinateY, 3].GetComponentInChildren<TextMeshProUGUI>().text)
                        {
                            LookAndCombine(coordinateY, 1, coordinateY, 3);
                        }
                        //Else If numbers are not equal then not move
                        else
                        {
                            LookAndShift(coordinateY, 1, coordinateY, 2, 2, 0, 1);
                        }
                    }
                }
                else
                {
                    //If number are equal then multiply by 2 and combine them
                    if (GameManager.singleton.boxes[coordinateY, 1].GetComponentInChildren<TextMeshProUGUI>().text == GameManager.singleton.boxes[coordinateY, 2].GetComponentInChildren<TextMeshProUGUI>().text)
                    {
                        LookAndCombine(coordinateY, 1, coordinateY, 2);
                    }
                    //Else If numbers are not equal then not move
                }
            }
            //if there is object in 0 horizontal
            if (GameManager.singleton.boxes[coordinateY, 0] != null)
            {
                if (GameManager.singleton.boxes[coordinateY, 1] == null)
                {
                    if (GameManager.singleton.boxes[coordinateY, 2] == null)
                    {
                        if (GameManager.singleton.boxes[coordinateY, 3] == null)
                        {
                            LookAndShift(coordinateY, 0, coordinateY, 3, 3, 0, 1);
                        }
                        else if (GameManager.singleton.boxes[coordinateY, 3] != null)
                        {
                            if (GameManager.singleton.boxes[coordinateY, 0].GetComponentInChildren<TextMeshProUGUI>().text == GameManager.singleton.boxes[coordinateY, 3].GetComponentInChildren<TextMeshProUGUI>().text)
                            {
                                LookAndCombine(coordinateY, 0, coordinateY, 3);
                            }
                            else
                            {
                                LookAndShift(coordinateY, 0, coordinateY, 2, 2, 0, 1);
                            }
                        }
                    }
                    else if (GameManager.singleton.boxes[coordinateY, 2] != null)
                    {
                        if (GameManager.singleton.boxes[coordinateY, 0].GetComponentInChildren<TextMeshProUGUI>().text == GameManager.singleton.boxes[coordinateY, 2].GetComponentInChildren<TextMeshProUGUI>().text)
                        {
                            LookAndCombine(coordinateY, 0, coordinateY, 2);
                        }
                        else
                        {
                            LookAndShift(coordinateY, 0, coordinateY, 1, 1, 0, 1);
                        }
                    }
                }
                else if (GameManager.singleton.boxes[coordinateY, 1] != null)
                {
                    if (GameManager.singleton.boxes[coordinateY, 0].GetComponentInChildren<TextMeshProUGUI>().text == GameManager.singleton.boxes[coordinateY, 1].GetComponentInChildren<TextMeshProUGUI>().text)
                    {
                        LookAndCombine(coordinateY, 0, coordinateY, 1);
                    }
                    //Else If numbers are not equal then not move
                }
            }
        }
        void ShiftDown(int coordinateY, int maxCoordinateNumber)
        {
            //if there is object in 2 horizontal
            if (GameManager.singleton.boxes[coordinateY, maxCoordinateNumber - 2] != null) //shift from here
            {
                if (GameManager.singleton.boxes[coordinateY, maxCoordinateNumber - 3] == null) //shift to here
                {
                    LookAndShift(coordinateY, maxCoordinateNumber - 2, coordinateY, maxCoordinateNumber - 3, 1, 0, -1);
                } //x
                else if (GameManager.singleton.boxes[coordinateY, maxCoordinateNumber - 3] != null)
                // IF THERE IS OBJECT
                //If the object is the same combine 
                {
                    //If number are equal then multiply by 2 and combine them
                    if (GameManager.singleton.boxes[coordinateY, maxCoordinateNumber - 2].GetComponentInChildren<TextMeshProUGUI>().text == GameManager.singleton.boxes[coordinateY, maxCoordinateNumber - 3].GetComponentInChildren<TextMeshProUGUI>().text)
                    {
                        LookAndCombine(coordinateY, maxCoordinateNumber - 2, coordinateY, maxCoordinateNumber - 3);
                    }
                    //Else If numbers are not equal then not move 
                }
            }
            //if there is object in 1 horizontal
            if (GameManager.singleton.boxes[coordinateY, maxCoordinateNumber - 1] != null)
            {
                if (GameManager.singleton.boxes[coordinateY, maxCoordinateNumber - 2] == null)
                {
                    if (GameManager.singleton.boxes[coordinateY, maxCoordinateNumber - 3] == null)
                    {
                        LookAndShift(coordinateY, maxCoordinateNumber - 1, coordinateY, maxCoordinateNumber - 3, 2, 0, -1);
                    }
                    else if (GameManager.singleton.boxes[maxCoordinateNumber - 3, coordinateY] != null)
                    {
                        //If number are equal then multiply by 2 and combine them
                        if (GameManager.singleton.boxes[coordinateY, maxCoordinateNumber - 1].GetComponentInChildren<TextMeshProUGUI>().text == GameManager.singleton.boxes[coordinateY, maxCoordinateNumber - 3].GetComponentInChildren<TextMeshProUGUI>().text)
                        {
                            LookAndCombine(coordinateY, maxCoordinateNumber - 1, coordinateY, maxCoordinateNumber - 3);
                        }
                        //Else If numbers are not equal then not move
                        else
                        {
                            LookAndShift(coordinateY, maxCoordinateNumber - 1, coordinateY, maxCoordinateNumber - 2, 2, 0, -1);
                        }
                    }
                } 
                else
                {
                    //If number are equal then multiply by 2 and combine them
                    if (GameManager.singleton.boxes[coordinateY, maxCoordinateNumber - 1].GetComponentInChildren<TextMeshProUGUI>().text == GameManager.singleton.boxes[coordinateY, maxCoordinateNumber - 2].GetComponentInChildren<TextMeshProUGUI>().text)
                    {
                        LookAndCombine(coordinateY, maxCoordinateNumber - 1, coordinateY, maxCoordinateNumber - 2);
                    }
                    //Else If numbers are not equal then not move
                }
            }
            //if there is object in 0 horizontal
            if (GameManager.singleton.boxes[coordinateY, maxCoordinateNumber - 0] != null)
            {
                if (GameManager.singleton.boxes[coordinateY, maxCoordinateNumber - 1] == null)
                {
                    if (GameManager.singleton.boxes[coordinateY, maxCoordinateNumber - 2] == null)
                    {
                        if (GameManager.singleton.boxes[coordinateY, maxCoordinateNumber - 3] == null)
                        {
                            LookAndShift(coordinateY, maxCoordinateNumber - 0, coordinateY, maxCoordinateNumber - 3, 3, 0, -1);
                        }
                        else if (GameManager.singleton.boxes[coordinateY, maxCoordinateNumber - 3] != null)
                        {
                            if (GameManager.singleton.boxes[coordinateY, maxCoordinateNumber - 0].GetComponentInChildren<TextMeshProUGUI>().text == GameManager.singleton.boxes[coordinateY, maxCoordinateNumber - 3].GetComponentInChildren<TextMeshProUGUI>().text)
                            {
                                LookAndCombine(coordinateY, maxCoordinateNumber - 0, coordinateY, maxCoordinateNumber - 3);
                            }
                            else
                            {
                                LookAndShift(coordinateY, maxCoordinateNumber - 0, coordinateY, maxCoordinateNumber - 2, 2, 0, -1);
                            }
                        }
                    }
                    else if (GameManager.singleton.boxes[coordinateY, maxCoordinateNumber - 2] != null) //x
                    {
                        if (GameManager.singleton.boxes[coordinateY, maxCoordinateNumber - 0].GetComponentInChildren<TextMeshProUGUI>().text == GameManager.singleton.boxes[coordinateY, maxCoordinateNumber - 2].GetComponentInChildren<TextMeshProUGUI>().text)
                        {
                            LookAndCombine(coordinateY, maxCoordinateNumber - 0, coordinateY, maxCoordinateNumber - 2);
                        }
                        else
                        {
                            LookAndShift(coordinateY, maxCoordinateNumber - 0, coordinateY, maxCoordinateNumber - 1, 1, 0, -1);
                        }
                    }
                }
                else if (GameManager.singleton.boxes[coordinateY, maxCoordinateNumber - 1] != null)
                {
                    if (GameManager.singleton.boxes[coordinateY, maxCoordinateNumber - 0].GetComponentInChildren<TextMeshProUGUI>().text == GameManager.singleton.boxes[coordinateY, maxCoordinateNumber - 1].GetComponentInChildren<TextMeshProUGUI>().text)
                    {
                        LookAndCombine(coordinateY, maxCoordinateNumber - 0, coordinateY, maxCoordinateNumber - 1);
                    }
                    //Else If numbers are not equal then not move
                }
            }
        }
    }
}


