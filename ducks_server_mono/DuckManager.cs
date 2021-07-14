using Godot;
using System;
using System.Collections.Generic;

public class DuckManager : Node2D
{
    struct Duck {
        public RID rid;
        public Vector2 position;
        public int nextDirection;
        public int nextLength;
        public int lengthMoved;
    }

    [Signal]
    public delegate void AddedDucks(int duckCount);

    [Export]
    public int startingDucks = 10;
    [Export]
    public Vector2[] spawnPoints;
    [Export]
    public Texture duckTexture;
    [Export]
    public Vector2 duckSourceGrid;
    [Export]
    public int gridSize = 16;
    [Export]
    public float moveDelay = 1f;

    private List<Duck> allDucks = new List<Duck>();
    private float timeSinceMove = 1f;
    private Physics2DDirectSpaceState spaceState;
    private Random rnd = new Random();
    


    public override void _Ready()
    {
        spaceState = GetWorld2d().DirectSpaceState;

        for(int i = 0; i < startingDucks; i++)
        {
            AddDuck();
        }
    }


    private Vector2 FindSpawnPosition()
    {
        return spawnPoints[rnd.Next(spawnPoints.Length - 1)];
    }


    private void AddDuck()
    {
        Vector2 spawnPosition = FindSpawnPosition();

        RID rid = VisualServer.CanvasItemCreate();
        Rect2 destinationRect = new Rect2(Vector2.Zero, new Vector2(gridSize, gridSize));
        Rect2 sourceRect = new Rect2(duckSourceGrid * gridSize, new Vector2(gridSize, gridSize));
        Transform2D transform = Transform.Translated(spawnPosition);

        VisualServer.CanvasItemSetParent(rid, GetCanvasItem());
        VisualServer.CanvasItemAddTextureRectRegion(rid,destinationRect,duckTexture.GetRid(),sourceRect,Colors.White,false,new RID(null),true);
        //VisualServer.CanvasItemAddTextureRectRegion(rid, destinationRect, duckTexture.GetRid(), sourceRect);
        VisualServer.CanvasItemSetTransform(rid, transform);

        Duck duck = new Duck();
        duck.rid = rid;
        duck.position = transform.origin;
        duck.nextDirection = ChooseDirection();
        duck.nextLength = ChooseLength();

        allDucks.Add(duck);
        EmitSignal("AddedDucks", allDucks.Count);
    }


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        timeSinceMove += delta;

        if (timeSinceMove > moveDelay)
        {
            timeSinceMove = 0;

            for (int i = 0; i < allDucks.Count; i++)
            {
                allDucks[i] = MoveDuck(allDucks[i]);
            }
        }
    }


    private Duck MoveDuck(Duck instance)
    {
        Vector2 positionCenter = instance.position + new Vector2(8,8);

        if (instance.nextDirection >= 0 && instance.nextDirection < 25)
        {
            var result = spaceState.IntersectRay(positionCenter, positionCenter + new Vector2(gridSize, 0));
            
            if (result.Count > 0)
            {
                instance.nextDirection = ChooseDirection();
            } 
            else 
            {
                instance.position.x += gridSize;
                instance.lengthMoved += 1;
            }
        }
        if (instance.nextDirection >= 25 && instance.nextDirection < 50)
        {
            var result = spaceState.IntersectRay(positionCenter, positionCenter + new Vector2(-gridSize, 0));
            
            if (result.Count > 0)
            {
                instance.nextDirection = ChooseDirection();
            } 
            else 
            {
                instance.position.x -= gridSize;
                instance.lengthMoved += 1;
            }
        }
        if (instance.nextDirection >= 50 && instance.nextDirection < 75)
        {
            var result = spaceState.IntersectRay(positionCenter, positionCenter + new Vector2(0, -gridSize));
            
            if (result.Count > 0)
            {
                instance.nextDirection = ChooseDirection();
            } 
            else 
            {
                instance.position.y -= gridSize;
                instance.lengthMoved += 1;
            }
        }
        if (instance.nextDirection >= 75 && instance.nextDirection < 100)
        {
            var result = spaceState.IntersectRay(positionCenter, positionCenter + new Vector2(0, gridSize));
            
            if (result.Count > 0)
            {
                instance.nextDirection = ChooseDirection();
            } 
            else 
            {
                instance.position.y += gridSize;
                instance.lengthMoved += 1;
            }
        }


        if (instance.lengthMoved > instance.nextLength)
        {
            instance.nextDirection = ChooseDirection();
            instance.nextLength = ChooseLength();
        }

        Transform2D transform = Transform.Translated(instance.position);
        VisualServer.CanvasItemSetTransform(instance.rid, transform);

        return instance;
    }


    private int ChooseDirection()
    {
        return rnd.Next(0,100);
    }


    private int ChooseLength()
    {
        return rnd.Next(0,40);
    }
}
