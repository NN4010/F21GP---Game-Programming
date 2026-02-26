SnackScape
SnackScape is 3D prototype game created using unity where the player collects snacks while avoiding enemies. The game includes timer, lives and multiple win-lose conditions.
Game Play:
Objective : Collect snacks (rally points), survive for a certain time along , or reach the finish point to win.
Lives: Player starts with 3 lives and loses a life when hit by enemy
Timer : Complete objectives before timer runs out, or you lose.
Win Conditions :
- Collect all required points
- Survive for a set time 
- Reach the finish point
Lose Conditions :
- Lives reach 0
- Timer reached 0 (if survival conditions not met)
Features:
Player Mechanics : Movement, Snack collection
Enemies : Reduce Player lives on contact
UI Elements : 
- Lives display 
- Snacks display
- Timer
- Floating text for collected rally points
- Game Over / Win Message
Win/Lose Logic :Managed via GameManager Script
 Installation & Setup

 1. Clone the repository 
 git clone https://github.com/NN4010/F21GP---Game-Programming.git
 2. Open Unity - Add the cloned project - Open with Unity 2021.x or later
 3. Open the scene in unity
 4. Press Play in the Unity Editor

 Scripts Overview:
 1. CameraFollow.cs 
 -Handles following the player during gameplay.
 -Smoothly moves and rotates the camera to keep the player centered in view.
 -Optional offset and damping for smooth camera motion.
2. CrowdManager.cs
 -Manages groups of enemies in the scene.
 -Spawns enemies, controls their behavior in crowds, and handles group dynamics.
 -Useful for organizing multiple enemies or moving groups at once.
3. Spider.cs (Enemy)
 -Controls enemy behavior for a single spider.
 -Handles movement, collision detection, and interaction with the player.
 -Can reduce player lives when contact occurs.
4. FinishPoint.cs
 -Represents the goal or finish line in the level.-Detects when the player reaches it and triggers the win condition in GameManager.
 Can be extended to play a particle effect or sound upon completion.
5. GameManager.cs
 -Core script managing overall game logic.
 -Tracks player lives, rally points, and timer.
 -Handles win/lose conditions and updates all relevant UI elements.
 -Controls floating text, start messages, and game state.
6. GroupManager.cs
 -Manages a group of enemies (like “Group Members”) together.
 -Handles group-level behavior such as moving together, splitting, or attacking as a unit.
7. GroupMember.cs (Enemy)
 -Controls individual enemy behavior within a group.-Detects player collisions to reduce lives.
 -Can follow group movement patterns set by GroupManager.
8. Player.cs
 -Handles player-specific logic beyond movement, such as collecting snacks or rally points. 
 -Detects collisions with enemies or finish points. Communicates with GameManager to update score, lives, or trigger win/lose events.
9. PlayerMovement.cs
 -Handles all player movement input (keyboard/controller).
 -Moves the player in the world and optionally rotates based on input.
 -Can include acceleration, speed limits, and smooth movement mechanics.
 10. RallyPoint.cs
 -Represents collectible rally/snack points in the scene.
 -When the player collects a rally point:
 -Adds to the player’s rally points count in GameManager.
 -Displays floating text above the player.
 -Checks if the rally point collection triggers a win condition.