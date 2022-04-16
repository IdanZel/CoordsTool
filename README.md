# CoordsTool
A tool for writing down Minecraft world coordinates which could be of help during speedruns (especially AA runs).

Based on [Antoine's AA-Texts tool](https://github.com/Antoine-MSL/AA-Texts) and inspired by [Ninjabrain bot](https://github.com/Ninjabrain1/Ninjabrain-Bot).

## Features
* Automatically add world coordinates in `F3`+`C` format from clipboard (can be disabled)
* Manually add coordinates in `x z` format
* Add and edit labels for saved coordinates
* Display coordinates' original dimension and automatically convert between Overworld and Nether coordinates
* Display coordinates as either regular or chunk coordinates
* Display the time in which the coordinates were added

## Legend
* ![Grass block](/CoordsTool.WPF/Resources/Textures/grass-block.png) signifies the **Overworld** dimension
* ![Netherrack](/CoordsTool.WPF/Resources/Textures/netherrack.png) signifies the **Nether** dimension
* ![End stone](/CoordsTool.WPF/Resources/Textures/end-stone.png) signifies the **End** dimension

## User Guide
### Automatically add coordinates (`F3`+`C`)

If a set of coordinates in an `F3`+`C` format is currently present in your clipboard, they will be automatically added.
Each time you press `F3`+`C` the new coordinates will be added.

You can disable this feature by going to the settings (press the gear icon at the bottom-left of the window) and uncheck "Automatically add coordinates from clipboard".

### Manually add coordinates

You can manually add coordinates by entering them into the "Coordinates" text-box in an `x z` format (where `x` and `z` are decimal numbers).
You must also choose a dimension for these coordinates (Overworld/Nether/End) by selecting the appropriate dimension image.

You can attach a label to those coordinates using the "Label" text-box.

Press the "Save" button to add the coordinates you entered.

### Edit labels

You can edit the label of an existing coordinates set by double clicking the "Label" cell in the coordinates' row. To save the label, press `Enter`.

### Display chunk coordinates

You can display chunk coordinates instead of regular coordinates. This setting is available seperatly for each dimension.
To enable this, open the settings, and select for which dimensions you would like to display chunk coordinates (a white border around the dimension image means that dimension is selected).
You can disable this by de-selecting the dimensions.



#### If you experience any bugs or problems when using this tool, please open an issue detailing the problem.
