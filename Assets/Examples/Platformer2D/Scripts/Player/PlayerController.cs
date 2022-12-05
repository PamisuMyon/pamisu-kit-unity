using System;
using Pamisu.Gameplay.Controllers;

namespace Pamisu.Platformer2D.Player
{
    public class PlayerController : PlatformerController2D
    {
        private void Update()
        {
            GroundedCheck();
            HandleOrientation();
        }

        private void FixedUpdate()
        {
            HandleMovement();
        }
    }
}