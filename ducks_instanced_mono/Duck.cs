using Godot;
using System;

public class Duck : Sprite
{
    [Export]
    public int gridSize = 16;
    [Export]
    public int moveDelay = 1;
    
    private int nextDirection = 1;
    private int nextLength = 1;
    private float timeSinceMove = 0f;
    private int lengthMoved = 0;
    private RayCast2D raycastRight;
    private RayCast2D raycastLeft;
    private RayCast2D raycastUp;
    private RayCast2D raycastDown;
    private Random random = new Random();


	public override void _Ready()
	{
        raycastRight = GetNode<RayCast2D>("RayCastRight");
        raycastLeft = GetNode<RayCast2D>("RayCastLeft");
        raycastUp = GetNode<RayCast2D>("RayCastUp");
        raycastDown = GetNode<RayCast2D>("RayCastDown");

        chooseDirection();
        chooseLength();
	}


    public override void _PhysicsProcess(float delta)
    {
        timeSinceMove += delta;

        if (timeSinceMove > moveDelay) 
        {
            timeSinceMove = 0;

            raycastRight.ForceRaycastUpdate();
            raycastLeft.ForceRaycastUpdate();
            raycastUp.ForceRaycastUpdate();
            raycastDown.ForceRaycastUpdate();

            if (nextDirection >= 0 && nextDirection < 25)
            {
                if (!raycastRight.IsColliding())
                {
                    Translate(new Vector2(gridSize, 0));
                    lengthMoved += 1;
                }
                else
                {
                    chooseDirection();
                }
            }
            if (nextDirection >= 25 && nextDirection < 50)
            {
                if (!raycastLeft.IsColliding())
                {
                    Translate(new Vector2(-gridSize, 0));
                    lengthMoved += 1;
                }
                else
                {
                    chooseDirection();
                }
            }
            if (nextDirection >= 50 && nextDirection < 75)
            {
                if (!raycastUp.IsColliding())
                {
                    Translate(new Vector2(0, -gridSize));
                    lengthMoved += 1;
                }
                else
                {
                    chooseDirection();
                }
            }
            if (nextDirection >= 75 && nextDirection < 100)
            {
                if (!raycastDown.IsColliding())
                {
                    Translate(new Vector2(0, gridSize));
                    lengthMoved += 1;
                }
                else
                {
                    chooseDirection();
                }
            }
        }

        if (lengthMoved > nextLength)
        {
            chooseDirection();
            chooseLength();
        }
    }


    private void chooseDirection()
    {
        nextDirection = random.Next(0,100);
    }


    private void chooseLength()
    {
        nextLength = random.Next(0,40);
    }
}