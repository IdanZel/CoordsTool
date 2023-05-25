# CoordsTool
A tool for writing down Minecraft world coordinates which could be of help during speedruns (especially AA runs).

Based on [Antoine's AA-Texts tool](https://github.com/Antoine-MSL/AA-Texts) and inspired by [Ninjabrain bot](https://github.com/Ninjabrain1/Ninjabrain-Bot).

## Features
* Automatically add world coordinates in `F3`+`C` format from clipboard (can be disabled)
* Manually add coordinates in `x z` or `x y z` format
* Add and edit labels for saved coordinates
* Display coordinates' original dimension and automatically convert between Overworld and Nether coordinates
* Display coordinates as either regular or chunk coordinates
* Display or hide Y-level
* Display the time in which the coordinates were added

## Legend
* ![Grass block](/CoordsTool.WPF/Resources/Textures/grass-block.png) signifies the **Overworld** dimension
* ![Netherrack](/CoordsTool.WPF/Resources/Textures/netherrack.png) signifies the **Nether** dimension
* ![End stone](/CoordsTool.WPF/Resources/Textures/end-stone.png) signifies the **End** dimension

## User Guide
### Automatically add coordinates (`F3`+`C`)

If a set of coordinates in an `F3`+`C` format (e.g. `/execute in minecraft:overworld run tp @s 80.50 63.00 -76.50 0.00 0.00`) is currently present in your clipboard, they will be automatically added.
Each time you press `F3`+`C` the new coordinates will be added.

You can disable this feature in settings (gear icon at the bottom-left of the window).

### Manually add coordinates

You can manually add coordinates by entering them into the "Coordinates" text-box in one of these formats:
* `x z`
* `x y z`

`x`, `y` and `z` are decimal numbers.

Entering Y-level is optional. Y-level display can be enabled in settings.
For manual input - if Y-level isn't entered, only `x` and `z` values will be displayed.

You must also choose a dimension for these coordinates (Overworld/Nether/End) by selecting the appropriate dimension image.

You can attach a label to those coordinates using the "Label" text-box.

Press the "Save" button or press `Enter` to add the coordinates.

### Edit labels

You can edit the label of existing coordinates by double clicking the "Label" cell in the coordinates' row. To save the label, press `Enter`.

### Remove / Restore coordinates

You can remove coordinates by clicking the red button at the right of the coordinates' row. You can also remove all coordinates currently saved by double-clicking the trashcan button.

You can restore deleted coordinates by clicking the restore button (next to the "Save" button). This will re-add one set of coordinates each time, starting with the latest coordinates deleted.

### Display chunk coordinates

You can display chunk coordinates instead of regular coordinates. This setting is available separately for each dimension.
To enable this, open the settings, and select for which dimensions you would like to display chunk coordinates.
You can disable this by de-selecting the dimensions.

## Possible Incompatibilities

There might be an issue with automatic clipboard reading when another process also tries to read from the clipboard (e.g. Ninjabrain Bot).
This should not cause the program to crash, but could potentially prevent `F3`+`C` coordinates from being added automatically.

---

#### If you experience any bugs or problems when using this tool, please open an issue detailing the problem.

---
