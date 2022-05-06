using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BeltPlacement : MonoBehaviour
{   
    public Tilemap beltTM;
    public Tilemap blueprintTM;
    public Tile bluePrint;
    public RuleTile beltUp;
    public RuleTile beltDown;
    public RuleTile beltLeft;
    public RuleTile beltRight;

    public RuleTile beltTopLeftS;
    public RuleTile beltTopRightS;
    public RuleTile beltBottomRightS;
    public RuleTile beltBottomLeftS;

    public RuleTile beltTopLeftCC;
    public RuleTile beltTopRightCC;
    public RuleTile beltBottomRightCC;
    public RuleTile beltBottomLeftCC;

    private Vector3Int pos1;


    //debugging variables below

    Vector3Int loc;
    Vector3Int loc2;
    RuleTile Ttile;
    bool leftSTile;

    void Update()
    {
        if(Input.GetKeyDown("q")) {

            Vector3 pos = Input.mousePosition;
            Vector3Int testPos = new Vector3Int(Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(pos).x - 0.5f), Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(pos).y - 0.5f), 0);

            // if(blueprintTM.GetTile(new Vector3Int(testPos.x+1, testPos.y))) {
            //     // Debug.Log("#2");;
            // }

            // if(blueprintTM.GetTile(new Vector3Int(testPos.x, testPos.y+1))) {
            //     // Debug.Log("#4");;
            // }

            // GetCornerDirections(testPos.x, testPos.y);
        }

        if(Input.GetMouseButton(0)) {
            blueprintTM.ClearAllTiles();

            Vector3 pos = Input.mousePosition;
            Vector3Int currentMousePos = new Vector3Int(Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(pos).x - 0.5f), Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(pos).y - 0.5f), 0);

            int px1 = pos1.x; //try to remove assigning values
            int px2 = currentMousePos.x;
            
            int py1 = pos1.y;
            int py2 = currentMousePos.y;

            if(px1 > px2) { //possibly change to XOR swap
                px1 = currentMousePos.x;
                px2 = pos1.x;
            }

            if(py1 > py2) { //possibly change to XOR swap
                py1 = currentMousePos.y;
                py2 = pos1.y;
            }

            if(px1 == px2) {
                for (int y = py1; y < py2 + 1; y++)
                {
                    blueprintTM.SetTile(new Vector3Int(px1, y, 0), bluePrint);
                }
            }

            else if(py1 == py2) {
                for (int x = px1; x < px2 + 1; x++)
                {
                    blueprintTM.SetTile(new Vector3Int(x, py1, 0), bluePrint);
                }
            }
            
            // for (int x = px1; x < px2; x++) //if can be placed across 2 dimentions
            // {
            //     for (int y = py1; y < py2; y++)
            //     {
            //         blueprintTM.SetTile(new Vector3Int(x, y, 0), bluePrint);
            //     }
            // }
        }

        if(Input.GetMouseButtonDown(0)) {

            Vector3 pos = Input.mousePosition;
            Vector3Int mousePos = new Vector3Int(Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(pos).x - 0.5f), Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(pos).y - 0.5f), 0);

            pos1 = mousePos;
        }

        else if(Input.GetMouseButtonUp(0)) {

            Vector3 pos = Input.mousePosition;
            Vector3Int pos2 = new Vector3Int(Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(pos).x - 0.5f), Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(pos).y - 0.5f), 0);

            RuleTile tileType = GetTileDirection(pos1, pos2);

            if(pos1.x == pos2.x && pos1.y != pos2.y) {

                int y1 = pos1.y;
                int y2 = pos2.y;

                if(pos1.y > pos2.y) { //possibly change to XOR swap
                    y1 = pos2.y;
                    y2 = pos1.y;
                }

                for (int i = y1; i < y2 + 1; i++) //double check the +1
                {
                    if(GetCornerDirections(pos1.x, i) != null) {
                        beltTM.SetTile(new Vector3Int(pos1.x, i, 0), GetCornerDirections(pos1.x, i));
                    }
                    else {
                        beltTM.SetTile(new Vector3Int(pos1.x, i, 0), tileType);
                    }
                }
            }
            else if(pos1.y == pos2.y && pos1.x != pos2.x) {
                int x1 = pos1.x;
                int x2 = pos2.x;

                if(pos1.x > pos2.x) { //possibly change to XOR swap
                    x1 = pos2.x;
                    x2 = pos1.x;
                }
                else {
                    x1 = pos1.x;
                    x2 = pos2.x;
                }

                for (int i = x1; i < x2 + 1; i++) //!!!!!!!!!!!!!! here is the Issue for the misplaced end tile when placing belts !!!!!!!!!!!!! //maybe not??
                {
                    if(GetCornerDirections(i, pos1.y) != null) {
                        beltTM.SetTile(new Vector3Int(i, pos1.y, 0), GetCornerDirections(i, pos1.y));
                    }
                    else {
                        beltTM.SetTile(new Vector3Int(i, pos1.y, 0), tileType);
                    }
                }
            }
            else if (pos1.x == pos2.x && pos1.y == pos2.y) {

                if(GetCornerDirections(pos1.x, pos1.y) != null) {
                    beltTM.SetTile(new Vector3Int(pos1.x, pos1.y), GetCornerDirections(pos1.x, pos1.y));
                }
                else if(!beltTM.GetTile(new Vector3Int(pos1.x, pos1.y))) {
                    beltTM.SetTile(new Vector3Int(pos1.x, pos1.y, 0), tileType);
                }
            }

            blueprintTM.ClearAllTiles();
        }

        else if(Input.GetMouseButton(1)) {
            Vector3 pos = Input.mousePosition;
            Vector3Int worldPos = new Vector3Int(Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(pos).x - 0.5f), Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(pos).y - 0.5f), 0);

            beltTM.SetTile(worldPos, null);
        }
    }

    private RuleTile GetCornerDirections(int x, int y) {

        // Debug.Log(beltTM.GetTile(new Vector3Int(x, y, 0)));
        // Debug.Log("Test: " + beltUp);

        // Vector3Int tilePos = new Vector3Int(x, y, 0);

        bool leftS;
        bool rightS;
        bool topS;
        bool bottomS;

        bool leftB;
        bool rightB;
        bool topB;
        bool bottomB;

        bool top_topLeftC;
        bool right_topRightC;
        bool left_bottomRightC;
        bool bottom_bottomLeftC;

        bool top_topLeftCC;
        bool right_topRightCC;
        bool left_bottomRightCC;
        bool bottom_bottomLeftCC;


        //extras

        bool bottom_bottomRightC;
        bottom_bottomRightC = CornerSubChecks(x, y, 0, -1, 7);

        bool left_topLeftC;
        left_topLeftC = CornerSubChecks(x, y, -1, 0, 5);

        bool left_bottomLeftC;
        left_bottomLeftC = CornerSubChecks(x, y, -1, 0, 8);

        bool top_topRightC;
        top_topRightC = CornerSubChecks(x, y, 0, 1, 6);

        bool right_bottomRightC;
        right_bottomRightC = CornerSubChecks(x, y, 1, 0, 7);

        //CC border extras

        bool top_topRightCC;
        top_topRightCC = CornerSubChecks(x, y, 0, 1, 10);

        bool left_bottomLeftCC;
        left_bottomLeftCC = CornerSubChecks(x, y, -1, 0, 12);

        bool right_bottomRightCC;
        right_bottomRightCC = CornerSubChecks(x, y, 1, 0, 11);

        bool bottom_bottomRightCC;
        bottom_bottomRightCC = CornerSubChecks(x, y, 0, -1, 11);

        bool left_topLeftCC;
        left_topLeftCC = CornerSubChecks(x, y, -1, 0, 9);
    

        topS = CornerSubChecks(x, y, 0, 1, 1);
        topB = CornerSubChecks(x, y, 0, 1, 3);
        rightS = CornerSubChecks(x, y, 1, 0, 2);
        rightB = CornerSubChecks(x, y, 1, 0, 4);

        bottomS = CornerSubChecks(x, y, 0, -1, 1);
        bottomB = CornerSubChecks(x, y, 0, -1, 3);
        leftS = CornerSubChecks(x, y, -1, 0, 2);
        leftB = CornerSubChecks(x, y, -1, 0, 4);

        //___________________________________________________________
        //Divider Between Corner Detection and Standard
        //Change the if statements below to be in the final conditions; i.e get rid of them and check them directly (they are using CheckTileOrientation only in one direction that isnt universal in the final checks)

        top_topLeftC = CornerSubChecks(x, y, 0, 1, 5);
        bottom_bottomLeftC = CornerSubChecks(x, y, 0, -1, 8);
        right_topRightC = CornerSubChecks(x, y, 1, 0, 6);
        // left_bottomRightC = CornerSubChecks(x, y, -1, 0, 7);

        top_topLeftCC = CornerSubChecks(x, y, 0, 1, 9);
        bottom_bottomLeftCC = CornerSubChecks(x, y, 0, -1, 12);
        right_topRightCC = CornerSubChecks(x, y, 1, 0, 10);
        // left_bottomRightCC = CornerSubChecks(x, y, -1, 0, 11);

        //test block below //delete in production
        bool testLeft;
        loc = new Vector3Int(x, y, 0);
        loc2 = new Vector3Int(x-1,y,0);
        Ttile = beltTM.GetTile<RuleTile>(loc);
        leftSTile = false;
        if(Ttile == beltLeft) leftSTile = true;
        if(beltTM.GetTile<RuleTile>(loc2) == beltLeft) testLeft = true;
        else testLeft = false;
        // Debug.Log("TestLeft: " + testLeft + "| isLeftSTile " + leftSTile + "| TopS: " + topS + "| topB: " + topB + "| rightS: " + rightS + "| rightB: " + rightB + "| bottomS: " + bottomS + "| bottomB: " + bottomB + "| leftS: " + leftS + "| leftB: " + leftB + " Right-TopRightCC: " + right_topRightCC + " Top-TopLeftC: " + top_topLeftC + " Left-BottomRightCC: " + left_bottomRightC); // + "| 1: " + top_topLeftC + "| 2: " + top_topRightC + "| 3: " + bottom_bottomLeftC + "| 4: " + bottom_bottomRightC
        // Debug.Log(top_topRightCC + " | " + left_bottomRightC);


        //nested operators not working
        if((rightS || right_topRightC) && (bottomS || bottom_bottomLeftC) && !leftS && !topS) return beltTopLeftS; //double check !not statements AND nested operators // || top_topRightC
        // else if((leftS || topLeftC) && (bottomS || bottomRightC) && !rightS && !topB) return beltTopRightS; //error here //bottomB not checked
        else if((bottomB || bottom_bottomRightC) && (leftS || left_topLeftC) && !topB && !rightS) return beltTopRightS;
        else if((leftB || left_bottomLeftC) && (topB || top_topRightC) && !rightB && !bottomS) return beltBottomRightS;
        else if((rightB || right_bottomRightC) && (topS || top_topLeftC) && !leftS && !bottomS) return beltBottomLeftS; // || bottom_bottomRightC (first section)

        else if((rightB || right_topRightCC) && (bottomB || bottom_bottomLeftCC) && !topB && !leftB) return beltTopLeftCC;
        else if ((bottomS || bottom_bottomRightCC) && (leftB || left_topLeftCC) && !rightB && !topS) return beltTopRightCC;
        else if ((topB || top_topLeftCC) && (rightS || right_bottomRightCC) && !bottomB && !leftS) return beltBottomLeftCC;
        else if ((topS || top_topRightCC) && (leftS || left_bottomLeftCC) && !bottomS && !rightB) return beltBottomRightCC;

        else return null;
    }

    private RuleTile GetTileDirection(Vector3Int pos1, Vector3Int pos2) {

        // Debug.Log("Pos1x: " + pos1.x + " Pos2x: " + pos2.x);

        if(pos2.y > pos1.y) return beltUp; //removed pos1 == pos2 || 

        else if(pos2.y < pos1.y) return beltDown;

        else if(pos2.x > pos1.x) return beltRight;

        else if(pos2.x < pos1.x) return beltLeft;

        else return beltUp; //remove if possible
    }

    private bool CornerSubChecks(int x, int y, int xMod, int yMod, int orientation) {
        if(beltTM.GetTile(new Vector3Int(x + xMod, y + yMod)) 
        && CheckTileOrientation(new Vector3Int(x + xMod, y + yMod), orientation) 
        || blueprintTM.GetTile(new Vector3Int(x + xMod, y + yMod))) 
        return true;
        else return false;
    }

    private bool CheckTileOrientation(Vector3Int location, int check) {

        bool checkUp = false;
        bool checkRight = false;
        bool checkDown = false;
        bool checkLeft = false;
        
        bool checkTopLeft = false;
        bool checkTopRight = false;
        bool checkBottomRight = false;
        bool checkBottomLeft = false;

        bool checkTopLeftCC = false;
        bool checkTopRightCC = false;
        bool checkBottomRightCC = false;
        bool checkBottomLeftCC = false;

        RuleTile thisTile = beltTM.GetTile<RuleTile>(location);

        if(thisTile == beltUp) checkUp = true;
        if(thisTile == beltRight) checkRight = true;
        if(thisTile == beltDown) checkDown = true;
        if(thisTile == beltLeft) checkLeft = true;

        if(thisTile == beltTopLeftS) checkTopLeft = true;
        if(thisTile == beltTopRightS) checkTopRight = true;
        if(thisTile == beltBottomRightS) checkBottomRight = true;
        if(thisTile == beltBottomLeftS) checkBottomLeft = true;

        if(thisTile == beltTopLeftCC) checkTopLeftCC = true;
        if(thisTile == beltTopRightCC) checkTopRightCC = true;
        if(thisTile == beltBottomRightCC) checkBottomRightCC = true;
        if(thisTile == beltBottomLeftCC) checkBottomLeftCC = true;

        // Debug.Log("A:" + checkTopLeft + " B:" + checkTopRight + " C:" + checkBottomRight + " D:" + checkBottomLeft);
        
        if(check == 1) {
            if(checkUp) return true;
        } else if(check == 2) {
            if(checkRight) return true;
        } else if(check == 3) {
            if(checkDown) return true;
        } else if(check == 4) {
            if(checkLeft) return true;
        } 

        else if(check == 5) {
            if(checkTopLeft) return true;
        } else if(check == 6) {
            if(checkTopRight) return true;
        } else if(check == 7) {
            if(checkBottomRight) return true;
        } else if(check == 8) {
            if(checkBottomLeft) return true;
        }

        else if(check == 9) {
            if(checkTopLeftCC) return true;
        } else if(check == 10) {
            if(checkTopRightCC) return true;
        } else if(check == 11) {
            if(checkBottomRightCC) return true;
        } else if(check == 12) {
            if(checkBottomLeftCC) return true;
        }

        return false;
    }
}
