using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace Benchwarmer.Resources.Code
{
    public partial class VeiwModel:ObservableObject
    {
        [ObservableProperty]
        string text;
    }
}
