# Style guide
## Editor style guide
1. Every script should use CamelCase naming (`PlayerBody.cs`, `Boss.cs`)
2. There are 4 main folders:
   - `Assets` - Holds game assets like textures, sounds, models, etc...
   - `Data` - Resource files like data of inventory: `inventory.tres`
   - `Scenes` - Different scenes used in the game
   - `Scripts` - Scripts used in the game
3. Every entity should have its own folder in `Scripts` folder. For example entity named `Inventory` should have all the scripts in `Scripts/Inventory`. Same rules applies for the assets in `Assets` directory.
4. Every script should be under namespace `Caveman`, for example inventory related scripts should be start with `namespace Caveman.Inventory{}`. Every class name must be same as the file name.
5. Every function should be also CamelCase like `public void UseThisMethod()`.
6. All enum values should be UPPER_CASE `enum PlayerState { IDLE, WALKING, ATT_MONSTER }`.
7. Please use meaningfull naming of variables. DO: `Inventory inventory = null` instead of: `Inventory i = null`!
8. Private properites/methods should start with underscore `private int _size` or `private void _PrivateMethod()`.

## Version control
- Every issue should have its own branch. After you are done with your task, create pull request to `main` branch.
- We should use conventional commits. Every message should start with emoji using `:name:` format. You can find meaning of the emojis [here](https://gitmoji.dev/).
- Every pull request should have at least one reviewer before merge to mitigate error during development
  
