using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Manager : MonoBehaviour
{

    private static Input input;
    
    public static void Init(Player myPlayer, StartMenu menu)
    {
        input = new Input();
        input.StartScreen.Enable();

        input.InGame.Move.performed += ctx =>
        {
            myPlayer.SetMovementDirection(ctx.ReadValue<Vector2>());
        };

        input.InGame.Jump.performed += ctx =>
        {
            myPlayer.Jump();
        };

        input.InGame.Dash.performed += ctx =>
        {
            myPlayer.Dash();

        };


        input.InGame.Flip.performed += ctx =>
        {
            myPlayer.Flip();
        };

        input.InGame.Left.performed += ctx =>
        {
            myPlayer.SetLeft();
        };

        input.InGame.Right.performed += ctx =>
        {
            myPlayer.SetRight();
        };

        input.StartScreen.Start.performed += ctx =>
        {
            SetGameControls();
            menu.StartGame();
        };
    }
    public static void SetGameControls()
    {
        input.StartScreen.Disable();
        input.InGame.Enable();

    } 

    public static void SetStartControls()
    {
        input.InGame.Disable();
        input.StartScreen.Enable();
        
    }
}
