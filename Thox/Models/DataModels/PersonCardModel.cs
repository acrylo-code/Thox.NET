
﻿namespace Thox.Models.DataModels
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
