<<<<<<<< HEAD:Thox/Models/ViewModels/Reservation/PersonCardModel.cs
﻿namespace Thox.Models.ViewModels.Reservation
========
﻿namespace Thox.Models.DataModels
>>>>>>>> d458c49 (init):Thox/Models/DataModels/PersonCardModel.cs
{
    public class PersonCardModel
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public int PersonCount { get; set; }
        public double Price { get; set; }
        public double SuccessPercentage { get; set; }
    }

    public class PersonCardModelList
    {
        public List<PersonCardModel>? personCardModelList { get; set; }
    }
}
