![image](Unity_yQaxRQVEBJ.gif)

## Documentation
### Unity Version:
* Unity 2021.3.25f1

### System:
    * Managers:
        * PlayerManager
            * Handles player data like coins, current bet
        * SpinManager
            * Handles the results of the reel. This script also has the data of the reward multiplier
            * Checks if line matrix has duplicate values
        * AudioManager
            * Centralized audiosource for all game object
    
    * UI
        * ReelPanel - Handles the Reel UI
        * PlayerPanel - Handles Player View/UI

    * Data
        * LineScriptableObject - Value for the Matrix Coordinates
        * PayoutScriptableObject - Data for the symbol reward multiplier

### Edit:
    * Line Matrix has an array of int which has a length of 5. Index can be change from 0 to 2 since there are only three rows in the game.
        * Assets/Scriptable/Line
    * Payout multiplier can be change. The symbols used in the project is currently the sprites but once the game starts it will save the name as the key.
        * Assets/Scriptable/Payout

### Additional notes:
    * Scalability
        * Data Caching - stored and reused frequently accessed data to improve performance
        * Audio - Since the current project is simple, I've only used 1 audiosource in the scene

    * Flexibility
        * Designers can change the values especially the min and max speed of the reels, values line of the matrix, and the payout multiplier
        * Currently using Leantween for animation and juicing of the game objects

    * MVC
        * Models - I've used managers to handle calculation of the results like in SpinManager, and player data calculation in PlayerManager.
        * View - Panels/UI in the scene which is the ReelPanel and PlayerPanel
        * Controller - This is to control the spin/top of the reels and also the bet buttons

    * Improvements
        * More Juicing
        * Optimization
        * Editor Customization (ODIN)
        * Online Remote Config

### Melbert Bolocon https://javurtez.vercel.app/