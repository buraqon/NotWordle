using UnityEngine;

public class GridSystem
{
    private Vector2Int _gridSize;
    private Vector2 _gridPos;
    private float _cellSize;
    private Cell[,] _gridArray;

    public Vector2Int GridSize
    {
        get{ return _gridSize; }
    }

    public GridSystem (Vector2Int gridSize, Vector2 gridPos, float cellSize)
    {
        _gridSize = gridSize;
        _gridPos = gridPos;
        _cellSize = cellSize;

        _gridArray = new Cell[gridSize.x, gridSize.y];
    }
    public Vector3 GetWorldPosition(int x, int y){
        return new Vector3(x+0.5f + _gridPos.x, -y+0.5f + _gridPos.y) * _cellSize;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y){
        x = Mathf.FloorToInt((worldPosition.x - _gridPos.x + 0.5f)/ _cellSize) -1;
        y = Mathf.FloorToInt((worldPosition.y - _gridPos.y + 0.5f)/ _cellSize) -1;   
    }

    public void PrintData(Vector3 worldPosition){
        int x,y;
        GetXY(worldPosition, out x, out y);
        Debug.Log(x + " " + y);
    }
    
    private bool IsIndexValid(int x, int y){
        if(x>=0 && y>=0 && x<_gridSize.x && y<_gridSize.y)
            return true;

        return false;
    }
    
    public void SetValue(int x, int y, string value)
    {
        _gridArray[x,y].CellValue = value;
    }

    public void SetCell(int x, int y, Cell cell)
    {
        _gridArray[x,y] = cell;
    }
    public void SetState(int x, int y, State state)
    {
        _gridArray[x,y].SwtichState(state);
    }
}
