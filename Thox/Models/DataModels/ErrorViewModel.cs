<<<<<<<< HEAD:Thox/Models/ViewModels/ErrorViewModel.cs
namespace Thox.Models.ViewModels
========
namespace Thox.Models.DataModels
>>>>>>>> d458c49 (init):Thox/Models/DataModels/ErrorViewModel.cs
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
