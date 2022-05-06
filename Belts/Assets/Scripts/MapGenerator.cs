using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{

    // public Tilemap testTM;
    public int mapWidth;
    public int mapHeight;
    
    [Range(0.00f, 1.00f)]
    public float randomThreshold;
    public int smoothLevels;
    public int biomeSmoothLevels;
    public int chunkSize;
    public int mergeOverlapRange;

    [Range(0.00f, 1.00f)]
    public float randomVarientChance;
    [Range(0.00f, 0.50f)]
    public float biomeTwoThreshold;    

    [Range(0.00f, 0.50f)]
    public float biomeThreeThreshold;

    [Range(0.00f, 1.00f)]
    public float baseBiome;

    [Range(0.00f, 1.00f)]
    public float coreBiome;

    [Range(0.00f, 0.10f)]
    public float biomeInfluence;

    public Tilemap tm;
    private Dictionary<Vector3, int> dict;
    private Dictionary<Vector3, WorldTile> biomeDict; 
    public RuleTile tile1;
    public RuleTile tile2;
    public RuleTile tile3;
    // public Tile tile1Varient;
    public Tile tile1Down;

    public int chunkX;
    public int chunkY;
    

    void Start()
    {

        chunkX++;

        dict = new Dictionary<Vector3, int>();

        if(chunkSize == 0) {
            chunkSize = 10;
        }

        int fullOffSetX = chunkX * chunkSize;
        int fullOffSetY = chunkY * chunkSize;

        firstIteration(fullOffSetX, fullOffSetY);
        
    }

    void Update() {

        // if(Input.GetKeyDown("v")) {
        //     int fullOffSetX = chunkX * chunkSize;
        //     int fullOffSetY = chunkY * chunkSize;
        //     TilePlacementIteration(fullOffSetX, fullOffSetY);
        //     Debug.Log("==");
        // }

        if(Input.GetKeyDown("space")) {
            
            chunkX++;
            int fullOffSetX = chunkX * chunkSize;
            int fullOffSetY = chunkY * chunkSize;
            // Debug.Log("FullOgset: " + fullOffSet);
            firstIteration(fullOffSetX, fullOffSetY);
        }

        if(Input.GetKeyDown("r")) {

            dict = new Dictionary<Vector3, int>();
            tm.ClearAllTiles();
            chunkX = 0;
            chunkY = 0;

            chunkX++;

            int fullOffSetX = chunkX * chunkSize;
            int fullOffSetY = chunkY * chunkSize;

            firstIteration(fullOffSetX, fullOffSetY);
        }

        if(Input.GetKeyDown("b")) {
            chunkY--;
            chunkX = 0;
        }
    }

    private void firstIteration(int offsetX, int offsetY) {

        for (int x = Mathf.RoundToInt(-mapWidth/2) + offsetX - mergeOverlapRange; x < Mathf.RoundToInt(mapWidth/2) + offsetX + mergeOverlapRange; x++)
        {
            for (int y = Mathf.RoundToInt(-mapHeight/2) + offsetY - mergeOverlapRange; y < Mathf.RoundToInt(mapHeight/2) + offsetY + mergeOverlapRange; y++)
            {

                Vector3Int worldLocation = new Vector3Int(x, y, 0);

                float randNum = Random.Range(0, 101);
                randNum /= 100;

                if(randNum <= randomThreshold) { 
                    if(!dict.ContainsKey(worldLocation)) {
                        dict.Add(worldLocation, 1);
                    }
                }
                else if(!dict.ContainsKey(worldLocation)) {
                    dict.Add(worldLocation, 0);
                }
            }
        }

        for (int i = 0; i < smoothLevels; i++)
        {
            smoothingIteration(offsetX, offsetY, i);
        }
    }

    private void smoothingIteration(int offsetX, int offsetY, int iterationNumber) {
        for (int x = Mathf.RoundToInt(-mapWidth/2) + offsetX - mergeOverlapRange; x < Mathf.RoundToInt(mapWidth/2) + offsetX + mergeOverlapRange; x++) {
            for (int y = Mathf.RoundToInt(-mapHeight/2) + offsetY - mergeOverlapRange; y < Mathf.RoundToInt(mapHeight/2) + offsetY + mergeOverlapRange; y++)
            {
                // Debug.Log("1.05");

                int surroundingTiles = getSurroundingTiles(x, y);

                Vector3Int worldLocation = new Vector3Int(x, y, 0);

                if(surroundingTiles >= 5) {
                    dict[worldLocation] = 1;

                    tm.SetTile(worldLocation, tile1);
                }
            }
        }

        // if(iterationNumber == smoothLevels -1) { //when using biomes
        //     generateBiomes(offsetX, offsetY);
        //     Debug.Log("==");
        // }
    }

    private void generateBiomes(int offsetX, int offsetY) { //currently turned off

        //create random number and create probability chunk will be loaded into biome.

        biomeDict = new Dictionary<Vector3, WorldTile>();

        for (int x = Mathf.RoundToInt(-mapWidth/2) + offsetX - mergeOverlapRange; x < Mathf.RoundToInt(mapWidth/2) + offsetX + mergeOverlapRange; x++) {
            for (int y = Mathf.RoundToInt(-mapHeight/2) + offsetY - mergeOverlapRange; y < Mathf.RoundToInt(mapHeight/2) + offsetY + mergeOverlapRange; y++) {
                if(tm.GetTile<RuleTile>(new Vector3Int(x, y, 0))) {
                    var tile = new WorldTile {
                        // location = new Vector3Int(x, y, 0),
                        tileTypeID = 1,
                    };

                    biomeDict.Add(new Vector3(x, y, 0), tile);
                }
                else {
                    var tile = new WorldTile {
                        // location = new Vector3Int(x, y, 0),
                        tileTypeID = 0
                    };
                
                    biomeDict.Add(new Vector3(x, y, 0), tile);
                }
            }
        }

        for (int x = Mathf.RoundToInt(-mapWidth/2) + offsetX - mergeOverlapRange; x < Mathf.RoundToInt(mapWidth/2) + offsetX + mergeOverlapRange; x++) {
            for (int y = Mathf.RoundToInt(-mapHeight/2) + offsetY - mergeOverlapRange; y < Mathf.RoundToInt(mapHeight/2) + offsetY + mergeOverlapRange; y++) {
                if(biomeDict.ContainsKey(new Vector3Int(x, y, 0)) && biomeDict[new Vector3Int(x, y, 0)].tileTypeID == 1) {
                    // Debug.Log("WORKIGN");
                    float randBiome = Random.Range(0, 100);
                    randBiome /= 100;

                    Vector3Int tileLocation = new Vector3Int(x, y, 0);

                    if(randBiome <= biomeTwoThreshold) {
                        
                        var tile = new WorldTile {
                            tileTypeID = 1,
                            biomeType = 2,
                            // snowStrength = baseBiome,
                            // grassStrength = coreBiome,
                            // candyStrength = baseBiome
                        };
                        
                        tm.SetTile(tileLocation, tile1);
                        biomeDict[tileLocation] = tile;
                    }

                    else if(randBiome <= (biomeTwoThreshold + biomeThreeThreshold)) {

                        var tile = new WorldTile {
                            tileTypeID = 1,
                            biomeType = 3,
                            // snowStrength = baseBiome,
                            // grassStrength = baseBiome,
                            // candyStrength = coreBiome
                        };

                        tm.SetTile(tileLocation, tile2);
                        biomeDict[tileLocation] = tile;
                    }

                    else {

                        var tile = new WorldTile {
                            tileTypeID = 1,
                            biomeType = 1,                            
                            // snowStrength = coreBiome,
                            // grassStrength = baseBiome,
                            // candyStrength = baseBiome
                        };

                        tm.SetTile(tileLocation, tile3);
                        biomeDict[tileLocation] = tile;
                    }
                }
            }
        }

        for (int i = 0; i < biomeSmoothLevels; i++)
        {
            smoothBiome(offsetX, offsetY); 
        }
    }

    private void smoothBiome(int offsetX, int offsetY) { //currently disabled
        for (int x = Mathf.RoundToInt(-mapWidth/2) + offsetX - mergeOverlapRange; x < Mathf.RoundToInt(mapWidth/2) + offsetX + mergeOverlapRange; x++) {
            for (int y = Mathf.RoundToInt(-mapHeight/2) + offsetY - mergeOverlapRange; y < Mathf.RoundToInt(mapHeight/2) + offsetY + mergeOverlapRange; y++) {

                if(biomeDict[new Vector3(x, y, 0)].biomeType == 1) {
                    getSurroundingBiome(x, y);
                }     
            }
        }
    }
    private int getSurroundingTiles(int x, int y) { //active


        int surroundingTiles = 0;

        if(dict.ContainsKey(new Vector3(x+1, y, 0)) && dict[new Vector3(x+1, y, 0)] == 1) {
            surroundingTiles++;
        }
        if(dict.ContainsKey(new Vector3(x-1, y, 0)) && dict[new Vector3(x-1, y, 0)] == 1) {
            surroundingTiles++;
        }
        if(dict.ContainsKey(new Vector3(x, y+1, 0)) && dict[new Vector3(x, y+1, 0)] == 1) {
            surroundingTiles++;
        }
        if(dict.ContainsKey(new Vector3(x, y-1, 0)) && dict[new Vector3(x, y-1, 0)] == 1) {
            surroundingTiles++;
        }

        if(dict.ContainsKey(new Vector3(x+1, y+1, 0)) && dict[new Vector3(x+1, y+1, 0)] == 1) {
            surroundingTiles++;
        }
        if(dict.ContainsKey(new Vector3(x-1, y+1, 0)) && dict[new Vector3(x-1, y+1, 0)] == 1) {
            surroundingTiles++;
        }
        if(dict.ContainsKey(new Vector3(x+1, y-1, 0)) && dict[new Vector3(x+1, y-1, 0)] == 1) {
            surroundingTiles++;
        }
        if(dict.ContainsKey(new Vector3(x-1, y-1, 0)) && dict[new Vector3(x-1, y-1, 0)] == 1) {
            surroundingTiles++;
        }

        return surroundingTiles;
    }

    private void getSurroundingBiome(int x, int y) { //currently disabled


        int nearSnow = 0;
        int nearGrass = 0;
        int nearCandy = 0;

        if(biomeDict.ContainsKey(new Vector3(x+1, y, 0))) {
            GetTileBiome(x+1, y);
        }
        if(biomeDict.ContainsKey(new Vector3(x-1, y, 0)) && biomeDict[new Vector3(x-1, y, 0)].biomeType == 1) {
            GetTileBiome(x-1, y);
        }
        if(biomeDict.ContainsKey(new Vector3(x, y+1, 0)) && biomeDict[new Vector3(x, y+1, 0)].biomeType == 1) {
            GetTileBiome(x, y+1);
        }
        if(biomeDict.ContainsKey(new Vector3(x, y-1, 0)) && biomeDict[new Vector3(x, y-1, 0)].biomeType == 1) {
            GetTileBiome(x, y-1);
        }

        if(biomeDict.ContainsKey(new Vector3(x+1, y+1, 0)) && biomeDict[new Vector3(x+1, y+1, 0)].biomeType == 1) {
            GetTileBiome(x+1, y+1);
        }
        if(biomeDict.ContainsKey(new Vector3(x-1, y+1, 0)) && biomeDict[new Vector3(x-1, y+1, 0)].biomeType == 1) {
            GetTileBiome(x-1, y+1);
        }
        if(biomeDict.ContainsKey(new Vector3(x+1, y-1, 0)) && biomeDict[new Vector3(x+1, y-1, 0)].biomeType == 1) {
            GetTileBiome(x+1, y-1);
        }
        if(biomeDict.ContainsKey(new Vector3(x-1, y-1, 0)) && biomeDict[new Vector3(x-1, y-1, 0)].biomeType == 1) {
            GetTileBiome(x-1, y-1);
        }

        //roughly working -- impliment all biomes next and maybe tweak variables
        if(nearSnow > nearGrass && nearSnow > nearCandy) { 
            var tile = new WorldTile {
                tileTypeID = 1,
                biomeType = 1
            };
            biomeDict[new Vector3(x, y, 0)] = tile;
            tm.SetTile(new Vector3Int(x, y, 0), tile1);
        }

        if(nearGrass > nearSnow && nearGrass > nearCandy) { 
            var tile = new WorldTile {
                tileTypeID = 1,
                biomeType = 2
            };
            biomeDict[new Vector3(x, y, 0)] = tile;
            tm.SetTile(new Vector3Int(x, y, 0), tile2);
        }

        if(nearCandy > nearSnow && nearCandy > nearGrass) { 
            var tile = new WorldTile {
                tileTypeID = 1,
                biomeType = 3
            };
            biomeDict[new Vector3(x, y, 0)] = tile;
            tm.SetTile(new Vector3Int(x, y, 0), tile3);
        }

        void GetTileBiome(int x, int y) {
            int biomeType = biomeDict[new Vector3(x, y, 0)].biomeType;

            if(biomeType == 1) nearSnow++;
            else if(biomeType == 2) nearGrass++;
            else if(biomeType == 3) nearCandy++;
        }

        // return [nearSnow, nearGrass, nearCandy];
    }
}
