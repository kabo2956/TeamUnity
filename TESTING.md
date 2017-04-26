Who: Kaleb Bodisch, Chance Roberts, Cale Anderson, Tamim Sha

Title: Space Adventures

Vision:To create an online competitive multiplayer runner game that we all would enjoy to play!


Automated Test Cases:

   We ended up automating testing by writing a Testing Object that's placed inside a Testing Scene. This Testing Object runs a series of tests on the MonoBehaviour objects that we would not be able to do with the Unity Editor Test Suite, since it is really difficult to actually test GameObjects using that. From there, we have different functions that are the various tests that we want to run on the objects that we choose. Right now, we can run tests based on public functions, which you can(?) do with the Editor Test Suite, collisions with other objects, which you can't do with the Test Suite. (Although some tests do require some waiting time, which can just be a single frame or so.) We cannot test anything that you would actually have the keyboard to test with this object, however, or at least you can't do it very well.
    
   Every test case starts with a print statements before using the Asserts that Unity comes with, so you can see exactly which test case failed when they do fail. Assertion failures, like print statements, show up in the console. Unlike print statements, they show up as errors so you know what has failed, and what is a description of that test case. As such, when running the testing scene, the messages are the number of test cases, and the errors are the number of test cases failed.
   
![image](https://github.com/kabo2956/TeamUnity/blob/master/Automatic%20Testing%20Images/WhatTestCasesLookLike.png)

This is what a test case function looks like. Note how there is a print statement at the beginning of the function detailing what the current test case is and the three asserts that check to see if the modifications done by the function called actually worked.

![image](https://github.com/kabo2956/TeamUnity/blob/master/Automatic%20Testing%20Images/TestingSuiteExample.png)

This is what a perfect test suite looks like. Note that the print statements aren't followed by any errors, and the fact that no errors actually show up in the console at all.

![image](https://github.com/kabo2956/TeamUnity/blob/master/Automatic%20Testing%20Images/ExampleofFailureWhileTesting.png)

This is what a failed test case looks like. Note how the error is after the print statement that actually ran so you can easily see which test cases it failed on.

![image](https://github.com/kabo2956/TeamUnity/blob/master/Automatic%20Testing%20Images/ExampleofAssertionFailureWhileWritingTestSuite.png)

You can also easily debug inside the test cases to see what your code has been doing if a test case fails, although errors that show up here are a mix between failed test cases and stuff that may be GameObjects doing what they shouldn't be doing.


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

    The user exits the game properly

Status (Pass/Fail)

    Pass
Notes

    N/A
Post-conditions

    The user has closed the game
    
Test 4:

 Use Case Name
 
    Picking Up and Throwing Items

Description

    While the player is playing a game, they can pick up items and throw them.

Pre-conditions
   
   The user must already be inside the running game and be near an item

Test steps

    1. Hold the control key while coming in contact with the item.
    2. Press up and down to manuever the item around for the start of a trajectory.
    3. Release control to throw the item.
    
Expected result

    The user picks up an item, moves it around with up and down, and then throws it when they release control.
    
Actual result

    The user picks up an item, moves it around with up and down, and then throws it when they release control.

Status (Pass/Fail)

    Pass
Notes

    There is a noticeable bug where you can move that item into another object, and if you throw an object while that object is connected in the solid it acts unpredictably. We may or may not need to fix this.
    
Post-conditions

    The object has been thrown. The user is not affected by the object at all.
    

