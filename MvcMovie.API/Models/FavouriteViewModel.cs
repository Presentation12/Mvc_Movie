﻿using MvcMovieDAL.Entities;

namespace MvcMovie.API.Models
{
    public class FavouriteViewModel
    {
        public int Id { get; set; }
        public virtual User User { get; set; }
        public virtual Movie Movie { get; set; }
    }
}
