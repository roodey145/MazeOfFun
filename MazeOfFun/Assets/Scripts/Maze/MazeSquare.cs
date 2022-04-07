using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SquarePosition: byte
{
    TOP_LEFT_CORNOR = 0,
    TOP_RIGHT_CORNOR = 1,
    BOTTOM_RIGHT_CORNOR = 2,
    BOTTOM_LEFT_CORNOR = 3,
    LEFT_BORDER = 4,
    TOP_BORDER = 5,
    RIGHT_BORDER = 6,
    BOTTOM_BORDER = 7,
    CENTER = 8
}


public class MazeSquare : MonoBehaviour
{
    // Initializing defualt info
    //-------------------------------------
    // Square walls
    private bool _isLeftOpen = false;
    private bool _isTopOpen = false;
    private bool _isRightOpen = false;
    private bool _isBottomOpen = false;

    // Open Direction
    private Wall _openDirection = Wall.NONE;

    // Closed Directions
    private Wall[] _closedDirections;

    // Location
    private Vector2Int _location;

    // Square size
    private float _width = 1;
    private float _length = 1;
    //-------------------------------------


    // Constructors
    public MazeSquare(float width, float length)
    {
        _width = width;
        _length = length;
    }

    public MazeSquare(float width, float length, SquarePosition position) : this(width, length)
    {
        switch(position)
        {
            // Cornors case start
            //--------------------------------------------------------
            /* If the square is on the top-left cornor, then the top border
            * and the left border are closing two direction */
            case SquarePosition.TOP_LEFT_CORNOR:
                _closedDirections = new Wall[]
                {
                    Wall.LEFT,
                    Wall.TOP
                };
                break;
            /* If the square is on the top-right cornor, then the top border
            * and the right border are closing two direction */
            case SquarePosition.TOP_RIGHT_CORNOR:
                _closedDirections = new Wall[]
                {
                    Wall.RIGHT,
                    Wall.TOP
                };
                break;
            /* If the square is on the bottom-left cornor, then the bottom border
            * and the left border are closing two direction */
            case SquarePosition.BOTTOM_LEFT_CORNOR:
                _closedDirections = new Wall[]
                {
                    Wall.LEFT,
                    Wall.BOTTOM
                };
                break;
            /* If the square is on the bottom-right cornor, then the bottom border
            * and the right border are closing two direction */
            case SquarePosition.BOTTOM_RIGHT_CORNOR:
                _closedDirections = new Wall[]
                {
                    Wall.RIGHT,
                    Wall.BOTTOM
                };
                break;
            //--------------------------------------------------------
            // Cornors case end

            // Edges(LEFT, TOP, RIGHT, BOTTOM) case start
            //--------------------------------------------------------
            /* The left border is closing the left direction of this square */
            case SquarePosition.LEFT_BORDER:
                _closedDirections = new Wall[]
                {
                    Wall.LEFT
                };
                break;
            /* The top border is closing the top direction of this square */
            case SquarePosition.TOP_BORDER:
                _closedDirections = new Wall[]
                {
                    Wall.TOP
                };
                break;
            /* The right border is closing the right direction of this square */
            case SquarePosition.RIGHT_BORDER:
                _closedDirections = new Wall[]
                {
                    Wall.RIGHT
                };
                break;
            /* The bottom border is closing the bottom direction of this square */
            case SquarePosition.BOTTOM_BORDER:
                _closedDirections = new Wall[]
                {
                    Wall.BOTTOM
                };
                break;
            // Edges(LEFT, TOP, RIGHT, BOTTOM) cases end
            //--------------------------------------------------------
        }
    }

    public MazeSquare(float width, float length, SquarePosition position, Vector2Int location) : this(width, length, position)
    {
        _location = location;
    }

    public MazeSquare(float width, float length, SquarePosition position, int x, int y)
        : this(width, length, position, new Vector2Int(x, y))
    {

    }

    // Methods
    //-------------------------------------


    // *OpenDirection Methods*
    //************************************
    /// <summary>
    /// Checks whether a direction has been assigned to this square. 
    /// </summary>
    /// <returns>Whether a direction is assigned</returns>
    public bool hasOpenDirection()
    {
        return _openDirection != Wall.NONE;
    }


    
    /// <summary>
    /// Assign a direction to a square. Throws an ArgumentException in case a Wall.NONE is 
    /// being assigned as a direction.
    /// </summary>
    /// <param name="direction">The opening direction of this area.</param>
    public void AssignOpenDirection(Wall direction)
    {
        if (direction == Wall.NONE)
            throw new ArgumentException("Can NOT assign Wall.NONE as a direction!");
        _openDirection = direction;
    }


    /// <summary>
    /// Get the open direction of this square. 
    /// </summary>
    /// <returns>The open direction, Can be Wall.NONE if it is not assigned. </returns>
    public Wall GetOpenDirection()
    {
        return _openDirection;
    }
    //************************************
    // Ends Of OpenDirection Methods



    // *Walls Methods*
    //************************************
    /// <summary>
    /// Open of the square walls directions. Throws an ArgumentExceotion in case a Wall.NONE is 
    /// recived as an argument. 
    /// </summary>
    /// <param name="direction">The direction to indicate as open.</param>
    public void OpenAWall(Wall direction)
    {
        // Avoid opening closed walls
        bool argumentException = false;

        // Check if the wall to be opened is closed by a border
        if(_closedDirections != null)
        { // Check if there is walls closed by the borders
            for(int i = 0; i < _closedDirections.Length; i++)
            {
                if(direction == _closedDirections[i])
                { // This wall is closed by a border => Can not be opened
                    argumentException = true;
                    break;
                }
            }
        }

        if (direction == Wall.NONE ||argumentException)
            throw new ArgumentException("Can NOT open Wall.NONE since it is not a direction!");

        // TODO: Limit the open direction that can be opened

        switch(direction)
        {
            case Wall.LEFT: // 0
            // case Wall.WEST: // 0
                _isLeftOpen = true;
                break;
            case Wall.TOP: // 1
            // case Wall.FORWARD: // 1
            // case Wall.NORTH: // 1
                _isTopOpen = true;
                break;
            case Wall.RIGHT: // 2
                _isRightOpen = true;
                break;
            case Wall.BOTTOM: // 3
            // case Wall.BACKWARD: // 3
            // case Wall.SOUTH: // 3
                _isBottomOpen = true;
                break;
        }
    }

    /// <summary>
    /// Checks if at least one of the walls is open.
    /// </summary>
    /// <returns>True if at least one wall is open</returns>
    public bool HasOpenWalls()
    {
        bool hasOpenWalls = false;

        if (_isLeftOpen || _isTopOpen || _isRightOpen || _isBottomOpen)
            hasOpenWalls = true;

        return hasOpenWalls;
    }

    /// <summary>
    /// Counts the numbers of walls that are open in this area. 
    /// </summary>
    /// <returns>The number of walls that are open. </returns>
    public int GetOpenWallsCount()
    {
        int wallsOpen = 0;

        if (_isLeftOpen)
            wallsOpen++;
        if (_isTopOpen)
            wallsOpen++;
        if (_isRightOpen)
            wallsOpen++;
        if (_isBottomOpen)
            wallsOpen++;

        return wallsOpen;
    }

    /// <summary>
    /// Checks whether the left direction is open or is closed by the border
    /// </summary>
    /// <returns>True if the direction is open</returns>
    public bool IsLeftOpen()
    {
        bool isOpen = _isLeftOpen;
        // Check if the square has walls closed by the borders
        if (_closedDirections != null)
        {
            for (int i = 0; i < _closedDirections.Length; i++)
            {
                // Check 
                if(_closedDirections[i] == Wall.LEFT)
                {
                    isOpen = false;
                    break;
                }
            }
        }

        return isOpen;
    }

    public bool IsWallOpen(Wall wallDirection)
    {
        bool isOpen = false;

        // TODO: Check if this needs to be uncommented
        /*switch(wallDirection)
        {
            case Wall.LEFT: // 0
            // case Wall.WEST: // 0
                isOpen = _isLeftOpen;
                break;
            case Wall.TOP: // 1
            // case Wall.FORWARD: // 1
            // case Wall.NORTH: // 1
                isOpen = _isTopOpen;
                break;
            case Wall.RIGHT: // 2
                isOpen = _isRightOpen;
                break;
            case Wall.BOTTOM: // 3
            // case Wall.BACKWARD: // 3
            // case Wall.SOUTH: // 3
                isOpen = _isBottomOpen = true;
                break;
        }*/

        // This part is placed beneth because its result is more important
        // Check if the square has walls closed by the borders
        if (_closedDirections != null)
        {
            for (int i = 0; i < _closedDirections.Length; i++)
            {
                // Check if this direction is closed by a border
                if (_closedDirections[i] == wallDirection)
                {
                    isOpen = false;
                    break;
                }
            }
        }

        return isOpen;
    }



    public Wall[] GetUnchoosenDirections()
    {

        // Check if the left wall is open
        bool leftWall = IsWallOpen(Wall.LEFT);

        // If this direction is not closed check if the square in the left
        // has a direction pointing toward this one
        if(leftWall)
        {
            // Get the location of the sqaure this direction is pointing towards
            Vector2Int nextSquareLocation = GetSquareAtDirection(Wall.LEFT);
            
            // Not necessary check
            // Check if this location exists
            if(nextSquareLocation.x < 0 || nextSquareLocation.y < 0)
            {

            }
            else
            { // There is a square next to this one
                // Check the opposite wall of the next square
                // |This Square ->||<- Opposite wall of next square|

            }
        }
        return null;
    }


    /// <summary>
    /// Get the square next to this square that the parameter
    /// direction is pointing towards. 
    /// </summary>
    /// <param name="direction">The direction ponting direction. </param>
    /// <returns>The square that the parameter direction pointing towards from this square. </returns>
    public Vector2Int GetSquareAtDirection(Wall direction)
    {
        Vector2Int location = new Vector2Int(_location.x, _location.y);

        switch(direction)
        {
            case Wall.LEFT:
                location.x -= 1;
                break;
            case Wall.TOP:
                location.y += 1;
                break;
            case Wall.RIGHT:
                location.x += 1;
                break;
            case Wall.BOTTOM:
                location.y -= 1;
                break;
        }

        return location;
    }

    //************************************
    // Ends Of Walls Methods

    /// <summary>
    /// Gets the size of the square as a 2D vector 
    /// where the Vector2.x is the width and
    /// the Vector2.y is the length of the square. 
    /// </summary>
    /// <returns></returns>
    public Vector2 GetSize()
    {
        return new Vector2(_width, _length);
    }

    //-------------------------------------
}