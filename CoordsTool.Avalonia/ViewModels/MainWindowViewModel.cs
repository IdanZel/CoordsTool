using System;
using System.Collections.ObjectModel;
using CoordsTool.Core.Coordinates;
using CoordsTool.Core.UserData;

namespace CoordsTool.Avalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<UserCoordinates> CoordinatesList { get; } = new()
        {
            new UserCoordinates
            {
                Coordinates = new MinecraftCoordinates(MinecraftDimension.Nether, 120, 200),
                Label = "something",
                TimeAdded = DateTime.Now - TimeSpan.FromHours(1),
                Type = UserCoordinatesType.Manual
            }
        };
    }
}