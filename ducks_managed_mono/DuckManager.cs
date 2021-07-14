using Godot;
using System;

public class DuckManager : Node2D
{
    [Signal]
    public delegate void AddedDucks(int duckCount);

    [Export]
    public int startingDucks = 0;
    [Export]
    public PackedScene duckScene;
    [Export]
    public Vector2[] spawnPoints;
    [Export]
    public int gridSize = 16;
    [Export]
    public float moveDelay = 1f;

    private int duckCount = 0;
    private float timeSinceMove = 1f;
    private Random rnd = new Random();


    public override void _Ready()
    {
        for(int i = 0; i < startingDucks; i++)
        {
            AddDuck();
        }
    }


    private void AddDuck()
    {
        Sprite instance = duckScene.Instance<Sprite>();
        instance.Translate(FindSpawnPosition());
        AddChild(instance);
        
        duckCount += 1;
        EmitSignal("AddedDucks", duckCount);
    }


    private Vector2 FindSpawnPosition()
    {
        return spawnPoints[rnd.Next(spawnPoints.Length - 1)];
    }


    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        timeSinceMove += delta;

        if (timeSinceMove > moveDelay)
        {
            timeSinceMove = 0;

            foreach(Sprite duck in GetChildren()) {
                MoveDuck(duck);
            }
        }
    }


    private void MoveDuck(Sprite instance)
    {
        RayCast2D raycastRight = instance.GetNode<RayCast2D>("RayCastRight");
        RayCast2D raycastLeft = instance.GetNode<RayCast2D>("RayCastLeft");
        RayCast2D raycastUp = instance.GetNode<RayCast2D>("RayCastUp");
        RayCast2D raycastDown = instance.GetNode<RayCast2D>("RayCastDown");

        raycastRight.ForceRaycastUpdate();
        raycastLeft.ForceRaycastUpdate();
        raycastUp.ForceRaycastUpdate();
        raycastDown.ForceRaycastUpdate();

        int nextDirection = (int) instance.Get("next_direction");
        int nextLength = (int) instance.Get("next_length");
        int lengthMoved = (int) instance.Get("length_moved");

        if (nextDirection >= 0 && nextDirection < 25)
        {
            if (!raycastRight.IsColliding())
            {
                instance.Translate(new Vector2(gridSize, 0));
                lengthMoved += 1;
            }
            else
            {
                nextDirection = chooseDirection();
            }
        }
        if (nextDirection >= 25 && nextDirection < 50)
        {
            if (!raycastLeft.IsColliding())
            {
                instance.Translate(new Vector2(-gridSize, 0));
                lengthMoved += 1;
            }
            else
            {
                nextDirection = chooseDirection();
            }
        }
        if (nextDirection >= 50 && nextDirection < 75)
        {
            if (!raycastUp.IsColliding())
            {
                instance.Translate(new Vector2(0, -gridSize));
                lengthMoved += 1;
            }
            else
            {
                nextDirection = chooseDirection();
            }
        }
        if (nextDirection >= 75 && nextDirection < 100)
        {
            if (!raycastDown.IsColliding())
            {
                instance.Translate(new Vector2(0, gridSize));
                lengthMoved += 1;
            }
            else
            {
                nextDirection = chooseDirection();
            }
        }

        if (lengthMoved > nextLength)
        {
            nextDirection = chooseDirection();
            nextLength = chooseLength();
        }

        instance.Set("next_direction", nextDirection);
        instance.Set("next_length", nextLength);
        instance.Set("length_moved", lengthMoved);
    }


    private int chooseDirection()
    {
        return rnd.Next(0,100);
    }


    private int chooseLength()
    {
        return rnd.Next(0,40);
    }
}