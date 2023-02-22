﻿using MvcMovieDAL.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcMovieDAL.Entities
{
    public class User : DefaultEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PassHash { get; set; }
        public string PassSalt { get; set; }
        public virtual ICollection<Favourite> Favourites { get; set;}
    }
}
