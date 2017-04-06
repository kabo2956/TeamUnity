Who: Kaleb Bodisch, Chance Roberts, Cale Anderson, Tamim Sha

Title: Space Adventures

Vision:To create an online competitive multiplayer runner game that we all would enjoy to play!


User Acceptance Tests:

Test 1:
Enter Game
    Verify game runs
Description
    Test entering game mode from the main menu after running executable
Pre-conditions
    User has the game executable for their platform
Test steps
    1. Run executable program
    2. Click "Start Game"
    3. Test to see character moves using the directional buttons
Expected result
    User should be able to enter the game, and move the character.
Actual result
    User can enter the game and move the character around
Status (Pass/Fail)
    Pass
Notes
    N/A
Post-conditions
    The characters position actually changes in the game.
   
Test 2:
Use case name
 People can connect to the network.

Description
 Testing network connections with Unity and seeing if the people connected can control their own players

Pre-conditions
Both players are in the Game scene.

Test Steps
 1. One person presses Start Server on the Game Window
 2. Another person presses a Join Server button and joins the server.
 3. Both players appear on screen and are able to be controlled by their respective players.

Expected result
 The players are on the same server and can move their own players, but not the other person's player.

Actual result
 The Join Server button is there, but you can't actually connect to another person's server.

Status (Pass/Fail)
 Fail

Notes
We have a working server but the netcode is not set up yet.

Post-conditions
 Both players are alone in their own little worlds.
 
 Test 3:
 Use Case Name
    Within the running game, the player can exit the game
Description
    Once the game is already running, player is able to exit back to the main menu and then close the game
Pre-conditions
    The user must already be inside the running game
Test steps
    1. Hit the escape key to bring up a pause menu
    2. Inside the pause menu click the "Main Menu" button
    3. The main menu should now appear, then click "exit game" in the main menu to close the game and return to desktop
Expected result
    User is returned to the screen they were in before they ran the executable file.
Actual result
    The user hits escape and nothing happens.
Status (Pass/Fail)
    Fail
Notes
    The pause menu should be finished by the time this submission is due
Post-conditions
    The user has closed the game
    
Automated Test Cases:
