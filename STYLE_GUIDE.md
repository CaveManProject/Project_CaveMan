# Style guide
## Editor style guide
1. Every script should use uppercase naming (`PlayerBody.gd`, `Boss.gd`)
2. There are 4 main folders:
   - `Assets` - Holds game assets like textures, sounds, models, etc...
   - `Data` - Resource files like data of inventory: `inventory.tres`
   - `Scenes` - Different scenes used in the game
   - `Scripts` - Scripts used in the game
3. Every entity should have its own folder in `Scripts` folder. For example entity named `Inventory` should have all the scripts in `Scripts/Inventory`. Same rules applies for the assets in `Assets` directory.

## Version control
- Every issue should have its own branch. After you are done with your task, create pull request to `main` branch
- We should use conventional commits. Every message should start with emoji using `:name:` format. You can find meaning of the emojis [here](https://gitmoji.dev/)
  
