# Style guide
## Editor style guide
1. Every script should use CamelCase naming (`PlayerBody.gd`, `Boss.gd`)
2. There are 4 main folders:
   - `Assets` - Holds game assets like textures, sounds, models, etc...
   - `Data` - Resource files like data of inventory: `inventory.tres`
   - `Scenes` - Different scenes used in the game
   - `Scripts` - Scripts used in the game
3. Every entity should have its own folder in `Scripts` folder. For example entity named `Inventory` should have all the scripts in `Scripts/Inventory`. Same rules applies for the assets in `Assets` directory.
4. Every script should start with `class_name {name of entity} extends {somthing}` and again, name of entity should be CamelCase.
5. Every function should be also lower_case like `func use_this_method()`.
6. All enum values should be UPPER_CASE `enum PlayerState { IDLE, WALKING, ATT_MONSTER }`.
7. Please use meaningfull naming of variables and typing. DO: `var inventory: Inventory = null` instead of: `var i = null`!
8. Use relevant types everywhere its possible. It eliminates error during development. For example instead of `func test(lambda}` you should write `func test(lambda:int)`.

## Version control
- Every issue should have its own branch. After you are done with your task, create pull request to `main` branch.
- We should use conventional commits. Every message should start with emoji using `:name:` format. You can find meaning of the emojis [here](https://gitmoji.dev/).
- Every pull request should have at least one reviewer before merge to mitigate error during development
  
